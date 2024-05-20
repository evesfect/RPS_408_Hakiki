using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace RPS_Server
{
    public partial class ServerForm : Form
    {

        private TcpListener tcpListener;
        private List<Task> clientTasks = new List<Task>();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private bool serverRunning = false;

        private List<Player> connectedPlayers = new List<Player>(); // Game room list
        private Queue<Player> waitingQueue = new Queue<Player>(); // Waiting queue
        private List<Player> LeaveList = new List<Player>(); //Left Players

        private string leaderboardFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "leaderboard.txt");
        private Dictionary<string, int> allTimeLeaderboard = new Dictionary<string, int>();

        private Dictionary<string, string> playerMoves = new Dictionary<string, string>();

        private Dictionary<string, string> results = new Dictionary<string, string>();

        

        public ServerForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ServerForm_FormClosing);
            IPTextBox.Text = GetLocalIPv4();
            IPTextBox.ReadOnly = true;
            LoadLeaderboard();
            UpdateLeaderboardUI();

        }

      

        private async void Button_listen_Click(object sender, EventArgs e)
        {
            if (!serverRunning)
            {
                int port;
                if (int.TryParse(portTextBox.Text, out port))
                {
                    Button_listen.Enabled = false;
                    tcpListener = new TcpListener(IPAddress.Any, port);
                    tcpListener.Start();
                    serverRunning = true;
                    UpdateGameStatus($"Server started on port {port}...");
                    

                    try
                    {
                        while (serverRunning)
                        {
                            var client = await tcpListener.AcceptTcpClientAsync();
                            UpdateGameStatus("A client is connected.");
                            var clientTask = HandleClientAsync(client, cancellationTokenSource.Token);
                            clientTasks.Add(clientTask);
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // TcpListener stopped exception.
                        UpdateGameStatus($"The server stopped listening on port {port}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateGameStatus($"Error starting the server: {ex.Message}");
                    }

                    finally
                    {
                        foreach (var task in clientTasks)
                        {
                            await task; // Check if all tasks are done, then stop the server.
                        }
                        serverRunning = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid port number.");
                }
                
            }
            else
            {
                StopServer();
                
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            NetworkStream networkStream = client.GetStream();
            using var streamReader = new StreamReader(networkStream, Encoding.UTF8);
            using var streamWriter = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            string username = null;
            try
            {
                
                if (await streamReader.ReadLineAsync() is string line)
                {
                    username = line.Trim();
                }

                if (string.IsNullOrEmpty(username) || username.Length > 30)
                {
                    await streamWriter.WriteLineAsync("ERROR Invalid or too long username.");
                    client.Close();
                    UpdateGameStatus($"Someone tried to join with an Invalid username.");
                    return;
                }

                if (connectedPlayers.Any(p => p.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    await streamWriter.WriteLineAsync("ERROR Username already taken.");
                    client.Close();
                    UpdateGameStatus($"Someone tried to join with an existing username.");
                    return;
                }

                Player player = new Player(client, username);
                int initialWins = allTimeLeaderboard.ContainsKey(username) ? allTimeLeaderboard[username] : 0; // Check if there already is data in the leaderboard
                player.Wins = initialWins;
                UpdateLeaderboard(username, 0);
                AddPlayerToGameOrQueue(player);

                

                while (!cancellationToken.IsCancellationRequested && networkStream.CanRead)
                {
                    if (client.Available > 0)
                    {
                        string message = await streamReader.ReadLineAsync();
                        if (message == null) break; // Client disconnected
                        await ProcessClientMessage(message, player);
                    }
                    await Task.Delay(50); // Reduce CPU usage
                }
            }
            catch (IOException ex)
            {
                // Log disconnection
                UpdateGameStatus($"Someone has disconnected unexpectedly: {ex.Message}");
            }
            catch (Exception ex)
            {
                UpdateGameStatus($"Error with a user: {ex.Message}");
            }
            finally
            {
                if (client.Connected == false)
                {
                    HandleDisconnection(client);
                }
                
            }
        }


        private async void AddPlayerToGameOrQueue(Player player)
        {
            
            if (connectedPlayers.Count < 4)
            {
                
                connectedPlayers.Add(player);
                UpdateUI();
                await SendMessageToClientAsync(player, "man");
                await SendMessageToClientAsync(player, "DISPLAY Welcome! You've successfully joined the game room.");
                BroadcastMessageGlobalAsync($"DISPLAY {player.Username} joined the game room.");
            }
            else
            {
                
                waitingQueue.Enqueue(player);
                UpdateUI();
                await SendMessageToClientAsync(player, "man");
                await SendMessageToClientAsync(player, "DISPLAY You've been added to the waiting queue.");
                await SendMessageToClientAsync(player, "WAIT");
                BroadcastMessageGlobalAsync($"DISPLAY {player.Username} joined the waiting queue.");
            }
            ProcessWaitingQueue();
            BroadcastPlayerRoomStatus();
        }


        private async Task ProcessClientMessage(string message, Player player)
        {
            if (message.StartsWith("MOVE "))
            {
                var parts = message.Split(' ');
                if (parts.Length == 3)
                {
                    var move = parts[2].ToLower(); // 'rock', 'paper', or 'scissors'
                    if (move == "rock" || move == "paper" || move == "scissors")
                    {
                        ReceivePlayerMove(player.Username, move);
                    }
                    else
                    {
                        await SendMessageToClientAsync(player, "ERROR Invalid move. Please choose rock, paper, or scissors.");
                    }
                }
                else
                {
                    await SendMessageToClientAsync(player, "ERROR Malformed move command.");
                }
            }
            else if (message.StartsWith("DISCONNECT"))
            {
                // Handle disconnection without blocking the server
                await Task.Run(() => HandleDisconnection(player.Client));
            }
            else if (message.StartsWith("LEAVE"))
            {
                await Task.Run(() => HandleLeave(player.Client));
            }
            else if (message.StartsWith("JOIN"))
            {
                await Task.Run(() => HandleJoin(player.Client));
            }
            // Other message processing logic here
        }

        private async void HandleJoin(TcpClient client)
        {
            Player player = null;
            lock (LeaveList)
            {
                player = LeaveList.FirstOrDefault(p => p.Client == client);
                if (player != null)
                {
                    LeaveList.Remove(player);
                }
            }

            if (player != null)
            {
                waitingQueue.Enqueue(player);
                UpdateUI();
                await SendMessageToClientAsync(player, "DISPLAY Welcome! You've successfully joined the waiting room.");
                await SendMessageToClientAsync(player, "WAIT");
                BroadcastMessageGlobalAsync($"DISPLAY {player.Username} joined the waiting room.");
            }
            ProcessWaitingQueue();
        }

        private void ReceivePlayerMove(string username, string move)
        {
            lock (playerMoves)  // Ensure thread safety as multiple clients can send moves simultaneously
            {
                if (connectedPlayers.Any(p => p.Username == username))
                {
                    // Overwrite the existing move or add a new one
                    playerMoves[username] = move;

                    // Optionally log the move for server-side tracking
                    UpdateGameStatus($"{username} has chosen {move}");
                }
                else
                {
                    UpdateGameStatus($"Invalid move attempt by an unrecognized player: {username}");
                }
            }
        }

        private void HandleDisconnection(TcpClient client)
        {
            Player player = null;
            
            lock (connectedPlayers)
            {
                player = connectedPlayers.FirstOrDefault(p => p.Client == client);
                if (player != null)
                {
                    connectedPlayers.Remove(player);
                }
            }

            if (player == null)
            {
                lock (waitingQueue)
                {
                    var waitingList = waitingQueue.ToList();
                    player = waitingList.FirstOrDefault(p => p.Client == client);
                    if (player != null)
                    {
                        waitingList.Remove(player);
                        waitingQueue = new Queue<Player>(waitingList);
                    }
                }
            }

            if (player == null)
            {
                lock (LeaveList)
                {
                    player = LeaveList.FirstOrDefault(p => p.Client == client);
                    if (player != null)
                    {
                        LeaveList.Remove(player);
                    }
                }
            }


            if (player != null)
            {
                player.Client.Close();
                BroadcastMessageGlobalAsync($"DISPLAY {player.Username} has left the game.");
                UpdateUI();
                
            }

            if (isCountdownActive && connectedPlayers.Count < 4)
            {
                BroadcastMessageGlobalAsync("DISPLAY Player disconnected, insufficient players to continue the countdown.");
                isCountdownActive = false; // Stop the countdown
            }
            ProcessWaitingQueue();
        }

        private async void HandleLeave(TcpClient client)
        {
            Player player = null;
            lock (connectedPlayers)
            {
                player = connectedPlayers.FirstOrDefault(p => p.Client == client);
                if(player != null)
                {
                    connectedPlayers.Remove(player);
                }
            }


            if (player != null)
            {
                LeaveList.Add(player);
                UpdateUI();
                await SendMessageToClientAsync(player, "DISPLAY You've been added to the left list.");
                await SendMessageToClientAsync(player, "DISPLAY You can join after the game.");
                BroadcastMessageGlobalAsync($"DISPLAY {player.Username} left the game.");
            }

            if (isCountdownActive && connectedPlayers.Count < 4)
            {
                BroadcastMessageGlobalAsync("DISPLAY Player left, insufficient players to continue the countdown.");
                isCountdownActive = false; // Stop the countdown
            }
            ProcessWaitingQueue();
        }



        private void ProcessWaitingQueue()
        {
            // Check if there's room in the game and players in the waiting queue
            while (connectedPlayers.Count < 4 && waitingQueue.Count > 0)
            {
                // Move the player from the waiting queue to the game room
                var player = waitingQueue.Dequeue();
                connectedPlayers.Add(player);

                // Notify the moved player
                _ = SendMessageToClientAsync(player, "DISPLAY You have been moved to the game room!");

                // Update UI and other players as needed
                UpdateUI();
                BroadcastMessageAsync($"DISPLAY {player.Username} has joined the game room.");
            }

            BroadcastPlayerRoomStatus();
            if (connectedPlayers.Count == 4)
            {
                // Restart the countdown or initiate the game start
                StartGameCountdown();
            }
            
        }

        private SemaphoreSlim countdownSemaphore = new SemaphoreSlim(1, 1);
        private bool isCountdownActive = false;

        private async void StartGameCountdown()
        {
            bool hasLock = await countdownSemaphore.WaitAsync(0);
            if (!hasLock) return; // Exit if unable to acquire the lock immediately

            if (isCountdownActive)
            {
                countdownSemaphore.Release();
                return; // Check if already active and exit if so
            }

            isCountdownActive = true;

            try
            {
                await WaitForPlayers(); // Wait for enough players

                int countdownTime = 10;
                while (countdownTime > 0 && connectedPlayers.Count >= 4)
                {
                    BroadcastMessageGlobalAsync($"COUNT {countdownTime}");
                    await Task.Delay(1000);
                    countdownTime--;

                    if (connectedPlayers.Count < 4)
                    {
                        BroadcastMessageGlobalAsync("DISPLAY Countdown stopped. Waiting for players...");
                        await WaitForPlayers();
                        countdownTime = 10;
                    }
                }

                if (connectedPlayers.Count >= 4)
                {
                    StartGame();
                }
                else
                {
                    BroadcastMessageGlobalAsync("DISPLAY Not enough players to start the game.");
                }
            }
            finally
            {
                isCountdownActive = false;
                countdownSemaphore.Release();
            }
        }

        private async Task WaitForPlayers()
        {
            // Check each second for players until the room is full
            while (connectedPlayers.Count < 4)
            {
                await Task.Delay(1000);
                ProcessWaitingQueue();
            }
        }

        private async void StartGame()
        {
            // Game start logic
            BroadcastMessageGlobalAsync("GAMESTART Game started...");
            await Task.Delay(10000); // 10 second timer

            InitializeResults();
            BroadcastMessageGlobalAsync("ROUNDEND");
            ProcessRound();
        }

        private void InitializeResults()
        {
            foreach (var player in connectedPlayers)
            {
                results[player.Username] = "alive";
            }
        }

        private async void NextRound()
        {
            BroadcastMessageGlobalAsync("NEXTROUND Next round started...");
            await Task.Delay(10000);

            BroadcastMessageGlobalAsync("ROUNDEND");
            ProcessRound();
        }

        private async void ProcessRound()
        {
            playerMoves.Clear(); // Clear the moves of the previous round if there is
            BroadcastMessageGlobalAsync("SENDMOVE"); // Receive moves from users
            await Task.Delay(3000);
            // Get only the moves of "alive" players
            var filteredMoves = playerMoves.Where(pm => results[pm.Key] == "alive").ToDictionary(pm => pm.Key, pm => pm.Value);
            playerMoves = filteredMoves; // Assign filtered moves back to playerMoves

            // Store the round results fresh each round
            var moveDetails = new StringBuilder("PLAYERMOVES"); // String to send to the clients (moves of all players)
            var movesSet = new HashSet<string>(playerMoves.Values);  // Set of unique moves made in the round

            foreach (var playerMove in playerMoves)
            {
                moveDetails.AppendFormat(" {0} {1}", playerMove.Key, playerMove.Value);
            }
            if (movesSet.Count == 1)
            {
                // All players chose the same move
                BroadcastMessageGlobalAsync("DISPLAY ROUND DRAW: Everyone chose the same hand.");
            }
            else if (movesSet.Count == 3)
            {
                // No clear winner
                BroadcastMessageGlobalAsync("DISPLAY ROUND DRAW: All three hands are played.");

            }
            else if (movesSet.Count == 2)
            {
                Dictionary<string, string> playerCounterStatus = new Dictionary<string, string>();

                foreach (var playerMove in playerMoves)
                {
                    var player = playerMove.Key;
                    if (playerMove.Value == "")
                    {
                        BroadcastMessageGlobalAsync($"DISPLAY {player} didn't play any move, eliminated!");
                        results[player] = "out";
                    }

                    if ((playerMove.Value == "rock" && movesSet.Contains("paper")) ||
                        (playerMove.Value == "paper" && movesSet.Contains("scissors")) ||
                        (playerMove.Value == "scissors" && movesSet.Contains("rock"))) // if the counter move is played
                    {
                        results[player] = "out";
                    }
                }

            }
            else if (movesSet.Count == 0)
            {
                BroadcastMessageGlobalAsync("DISPLAY Nobody played their hand!");
                BroadcastMessageGlobalAsync("DISPLAY RESTARTING THE GAME AFTER 5 SECONDS.");
                BroadcastMessageGlobalAsync("GAMEOVER");
                await Task.Delay(5000);
                StartGameCountdown();
            }
            else // if no hands are played
            {
                BroadcastMessageGlobalAsync("DISPLAY Something unexpected happened!");
            }


            //BroadcastMessageGlobalAsync("ROUNDEND");
            BroadcastMessageGlobalAsync(moveDetails.ToString());
            BroadcastAlivePlayers(results);
            playerMoves.Clear();  // Clear after processing


            if (results.Count(r => r.Value == "alive") > 1)  // If more than one player is alive, continue the game
            {
                int aliveCount = results.Count(r => r.Value == "alive");
                BroadcastMessageGlobalAsync($"DISPLAY {aliveCount} players are alive, game continues.");
                BroadcastMessageGlobalAsync("DISPLAY Waiting 5 seconds before next round.");
                await Task.Delay(5000);
                NextRound();
            }
            else if (results.Count(r => r.Value == "alive") == 1) // If one player is alive
            {
                string winningPlayerName = results.FirstOrDefault(x => x.Value == "alive").Key;
                var winningPlayer = connectedPlayers.FirstOrDefault(p => p.Username == winningPlayerName); // Get the player object by the username
                if (winningPlayer != null)
                {
                    winningPlayer.Wins++; // Increment local win count
                    UpdateLeaderboard(winningPlayerName, 1); //Increment leaderboard win count
                }
                else
                {
                    BroadcastMessageGlobalAsync("ERROR Winning player cannot be found in the game room!");
                }
                

                BroadcastMessageGlobalAsync("GAMEOVER");
                BroadcastMessageGlobalAsync("DISPLAY Gameover: " + winningPlayerName + " wins!");
                results.Clear();

                BroadcastMessageGlobalAsync("DISPLAY Countdown for the next game will start after 5 seconds.");
                await Task.Delay(5000);
                StartGameCountdown();
            }
            else
            {
                
                BroadcastMessageGlobalAsync("GAMEOVER");
                BroadcastMessageGlobalAsync("DISPLAY There isn't any alive players, something went wrong!");
                BroadcastMessageGlobalAsync("DISPLAY RESTARTING THE GAME AFTER 5 SECONDS.");
                await Task.Delay(5000);
                StartGameCountdown();
            }

        }

        private void SendMessageToClient(Player player, string message)
        {
            // Send message using a Player object
            try
            {
                if (player.Client.Connected)
                {
                    var stream = player.Client.GetStream();
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                UpdateGameStatus($"Error sending message to {player.Username}: {ex.Message}");
                Console.WriteLine($"Error sending message to {player.Username}: {ex.Message}");
            }
        }

        private async Task SendMessageToClientAsync(Player player, string message)
        {
            // Send message using a Player object
            try
            {
                if (player.Client.Connected)
                {
                    await player.SemaphoreSlim.WaitAsync();  // Wait to enter the semaphore
                    try
                    {
                        await player.Writer.WriteLineAsync(message);
                    }
                    finally
                    {
                        player.SemaphoreSlim.Release();  // Release the semaphore
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                UpdateGameStatus($"Error sending message to {player.Username}: {ex.Message}");
                Console.WriteLine($"Error sending message to {player.Username}: {ex.Message}");
            }
        }

        private async void BroadcastMessageAsync(string message)
        {
            // Send message to all players in the room
            var allPlayers = new List<Player>(connectedPlayers.Concat(waitingQueue.ToList()).Concat(LeaveList));

            foreach (var player in allPlayers)
            {
                try
                {
                    await SendMessageToClientAsync(player, message);
                }
                catch (Exception ex)
                {
                    UpdateGameStatus($"Error sending message to {player.Username}: {ex.Message}");
                    Console.WriteLine($"Error sending message to {player.Username}: {ex.Message}");
                }
            }
        }

        private async void BroadcastMessageGlobalAsync(string message)
        {
            UpdateGameStatus(message);

            // Log server-side and send message to the players in the room, waiting queue and the leavequeue
            var allPlayers = new List<Player>(connectedPlayers.Concat(waitingQueue.ToList()).Concat(LeaveList));

            foreach (var player in allPlayers)
            {
                try
                {
                    await SendMessageToClientAsync(player, message);
                }
                catch (Exception ex)
                {
                    UpdateGameStatus($"Error sending message to {player.Username}: {ex.Message}");
                    Console.WriteLine($"Error sending message to {player.Username}: {ex.Message}");
                }
            }
        }

        private void BroadcastPlayerRoomStatus()
        {
            string playersString = "PLAYERS";
            foreach (var InGameplayer in connectedPlayers)
            {
                playersString += $" {InGameplayer.Username} {InGameplayer.Wins}";
            }
            BroadcastMessageAsync(playersString);

        }

        private void BroadcastAlivePlayers(Dictionary<string, string> results)
        {
            var alivePlayers = results.Where(r => r.Value == "alive").Select(r => r.Key).ToList();
            if (alivePlayers.Count > 0)
            {
                BroadcastMessageGlobalAsync("ALIVE " + string.Join(" ", alivePlayers));
            }
            else
            {
                BroadcastMessageGlobalAsync("ALIVE"); // Might need to handle scenario where no players are alive
            }
        }

        private void UpdateGameStatus(string message)
        {
            // Log message server side
            if (InvokeRequired) // If the current thread is not the UI thread
            {
                Invoke(new Action(() => UpdateGameStatus(message)));
                return;
            }

            // Add timestamp
            logTextBox.AppendText($"{DateTime.Now}: {message}\r\n");
            logTextBox.ScrollToCaret();
        }
        
        private void UpdateUI()
        {
            // InvokeRequired ensures running on UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateUI));
                return;
            }

            // Update playersTextBox with the names of connected players
            playersTextBox.Text = string.Join(Environment.NewLine, connectedPlayers.Select(p => p.Username));

            // Update wqueueTextBox with the names of players in the waiting queue
            wqueueTextBox.Text = string.Join(Environment.NewLine, waitingQueue.Select(p => p.Username));

            LeftPlayersTextBox.Text =string.Join(Environment.NewLine, LeaveList.Select(p => p.Username));
        }

        private void LoadLeaderboard()
        {
            if (!File.Exists(leaderboardFilePath))
            {
                File.Create(leaderboardFilePath).Close();
            }

            var lines = File.ReadAllLines(leaderboardFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int wins))
                {
                    allTimeLeaderboard[parts[0]] = wins;
                }
            }
        }
        
        private void UpdateLeaderboard(string username, int additionalWins = 1)
        {
            if (allTimeLeaderboard.ContainsKey(username))
            {
                allTimeLeaderboard[username] += additionalWins;
            }
            else
            {
                allTimeLeaderboard[username] = additionalWins;
            }

            SaveLeaderboardToFile();
            UpdateLeaderboardUI();
        }
        
        private void SaveLeaderboardToFile()
        {
            var lines = allTimeLeaderboard.Select(kvp => $"{kvp.Key},{kvp.Value}");
            File.WriteAllLines(leaderboardFilePath, lines);
        }
        
        private void UpdateLeaderboardUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateLeaderboardUI));
                return;
            }

            Leaderboard_richTextBox.Clear(); // Clear content

            if (allTimeLeaderboard.Count == 0)
            {
                Leaderboard_richTextBox.AppendText("No leaderboard data available.");
                return;
            }

            var leaderboardEntries = allTimeLeaderboard.OrderByDescending(kvp => kvp.Value)
                                                       .Select(kvp => $"{kvp.Key}: {kvp.Value} wins");

            foreach (var entry in leaderboardEntries)
            {
                Leaderboard_richTextBox.AppendText(entry + "\n");
            }
        }
        
        private void RecordWin(string username)
        {
            // To be called whenever a game is won
            UpdateLeaderboard(username);
            var player = connectedPlayers.Find(x => x.Username == username);
            player.Wins = allTimeLeaderboard[username] += 1;
        }
        
        private string GetLocalIPv4()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // Filter for IPv4
                {
                    return ip.ToString();
                }
            }
            return "Local IP Address Not Found";
        }

        private void StopServer()
        {
            if (serverRunning && tcpListener != null)
            {
                serverRunning = false;
                cancellationTokenSource.Cancel();
                tcpListener.Stop();
                 // Stop the timer

                foreach (var player in connectedPlayers.Concat(waitingQueue.ToList()).Concat(LeaveList))
                {
                    player.Client.Close(); // Close all client connections
                }
                connectedPlayers.Clear();
                waitingQueue.Clear();
                UpdateGameStatus("Server stopped.");
                Button_listen.Enabled = true;
            }
        }


        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }
    }
}
