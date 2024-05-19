using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace RPSClient
{
    public partial class ClientForm : Form
    {
        private TcpClient? client;
        private StreamReader? reader;
        private StreamWriter? writer;
        private Task? listenTask;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string move;
        bool gamelost = false;
        string[] prevplayers;



        public ClientForm()
        {
            InitializeComponent();
            this.FormClosing += ClientForm_FormClosing;
            Player1Name_textBox.ReadOnly = true;
            Player1Point_textBox.ReadOnly = true;
            Player1_richTextBox.ReadOnly = true;
            Player2Name_textBox.ReadOnly = true;
            Player2Point_textBox.ReadOnly = true;
            Player2_richTextBox.ReadOnly = true;
            Player3Name_textBox.ReadOnly = true;
            Player3Point_textBox.ReadOnly = true;
            Player3_richTextBox.ReadOnly = true;
            Username_textBox.ReadOnly = true;
            UserPoint_textBox.ReadOnly = true;
            User_richTextBox.ReadOnly = true;
            Rock_button.Enabled = false;
            Paper_button.Enabled = false;
            Scissors_button.Enabled = false;
            Leave_button.Enabled = false;
        }

        private async void Connect_button_Click(object sender, EventArgs e)
        {
            Disconnect(); // Ensure clean-up of previous connections


            client = new TcpClient();
            try
            {
                
                await client.ConnectAsync(IPBox.Text, int.Parse(PortBox.Text));
                UpdateConsole("Connected to server.");

                var stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                await writer.WriteLineAsync(NameBox.Text); // Send username to server
                listenTask = Task.Run(() => ListenForMessages(), cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to the server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                client?.Close(); // Ensure cleanup on failure
                client = null;
            }
        }

        SemaphoreSlim streamLock = new SemaphoreSlim(1, 1);
        private async void ListenForMessages()
        {
            while (client != null)
            {
                
                try
                {
                    await streamLock.WaitAsync();
                    if (reader == null)
                    {
                        UpdateConsole("Error: Stream reader is not initialized.");
                        return;
                    }
                    
                    SafeUpdateControl(IPBox, () => IPBox.Enabled = false);
                    SafeUpdateControl(PortBox, () => PortBox.Enabled = false);
                    SafeUpdateControl(NameBox, () => NameBox.Enabled = false);
                    SafeUpdateControl(Connect_button, () => Connect_button.Enabled = false);
                    SafeUpdateControl(Leave_button, () => Leave_button.Enabled = true);
                    SafeUpdateControl(Username_textBox, () => Username_textBox.Text = NameBox.Text);

                    string? message;


                    while ((message = await reader.ReadLineAsync()) != null && !cancellationTokenSource.IsCancellationRequested)
                    {
                        
                        ProcessMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    if (!cancellationTokenSource.IsCancellationRequested)
                        UpdateConsole($"Disconnected.");
                }
                finally
                {
                    streamLock.Release();
                }
            }

        }
        private void SafeUpdateControl(Control ctrl, Action updateAction)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new Action(() => SafeUpdateControl(ctrl, updateAction)));
            }
            else
            {
                updateAction();
            }
        }

        private int count= 0;
        private void ProcessMessage(string message)
        {
            // Split the message into the command and the content
            int firstSpaceIndex = message.IndexOf(' ');
            string command = firstSpaceIndex != -1 ? message.Substring(0, firstSpaceIndex) : message;
            string content = firstSpaceIndex != -1 ? message.Substring(firstSpaceIndex + 1) : "";

            // Handling different types of commands
            switch (command) // case-insensitive
            {
                case "DISPLAY":
                    UpdateConsole(content);
                    break;
                case "ERROR":
                    UpdateConsole(content);
                    break;
                case "PLAYERS": //PLAYERS tolga 10 ege 20 ata 15 ozgur 30  
                    UpdateNameandPoints(content);
                    break;
                case "PLAYERMOVES": //PLAYERMOVES tolga rock ege scissors ata paper ozgur rock
                    UpdateMoves(content);
                    break;
                case "COUNT":
                    UpdateConsole(content);
                    break;
                case "GAMESTART":
                    HandleGameStart();
                    break;
                case "SENDMOVE":
                    if(!gamelost)
                    {
                        SendMessage(move);
                    }
                    break;
                case "ROUNDEND":
                    //SendMessage(move);
                    break;
                case "ALIVE":
                    UpdateAlive(content);
                    break;
                case "NEXTROUND":
                    ClearMoveDisplays();
                    break;
                case "GAMEOVER":
                    HandleGameOver();
                    break;
                default:
                    if (count != 0)
                    {
                        UpdateConsole($"UNKNOWN FLAG: {content}");
                    }
                    count++;
                    break;
            }
        }

        private void HandleGameOver()
        {
            move = "";
            gamelost = false;
            EnableMoveButtons(false);
        }
        private void HandleGameStart()
        {
            UpdateConsole("Game started!");
            if (!gamelost)
            {
                EnableMoveButtons(true);
            }
        }

        private void UpdateAlive(string message)
        {
            string[] players = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            bool isUserAlive = players.Any(player => player == Username_textBox.Text);

            // Update console based on the alive status
            if (isUserAlive)
            {
                UpdateConsole("You are still in the game.");
                EnableMoveButtons(true);  // Enable buttons if the user is alive
            }
            else
            {
                UpdateConsole("You are out of this round.");
                EnableMoveButtons(false);  // Disable buttons if the user is not alive
                gamelost = true;
            }
        }

        private void EnableMoveButtons(bool enable)
        {
            if (enable)
            {
                SafeUpdateControl(Rock_button, () => Rock_button.Enabled = true);
                SafeUpdateControl(Paper_button, () => Paper_button.Enabled = true);
                SafeUpdateControl(Scissors_button, () => Scissors_button.Enabled = true);
            }
            else
            {
                SafeUpdateControl(Rock_button, () => Rock_button.Enabled = false);
                SafeUpdateControl(Paper_button, () => Paper_button.Enabled = false);
                SafeUpdateControl(Scissors_button, () => Scissors_button.Enabled = false);
            }
        }

        private void UpdateMoves(string message)
        {
            string[] moves = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < moves.Length; i += 2)
            {
                if (moves[i] == Username_textBox.Text)
                {
                    SafeUpdateControl(User_richTextBox, () => User_richTextBox.AppendText($"You have played {moves[i + 1]}!\n"));
                }

                else if (moves[i] == Player1Name_textBox.Text)
                {
                    SafeUpdateControl(Player1_richTextBox, () => Player1_richTextBox.AppendText($"{Player1Name_textBox.Text} has played {moves[i + 1]}!\n"));
                }

                else if (moves[i] == Player2Name_textBox.Text)
                {
                    SafeUpdateControl(Player2_richTextBox, () => Player2_richTextBox.AppendText($"{Player2Name_textBox.Text} has played {moves[i + 1]}!\n"));
                }

                else if (moves[i] == Player3Name_textBox.Text)
                {
                    SafeUpdateControl(Player3_richTextBox, () => Player3_richTextBox.AppendText($"{Player3Name_textBox.Text} has played {moves[i + 1]}!\n"));
                }
            }
        }

        private void ClearMoveDisplays()
        {
            SafeUpdateControl(User_richTextBox, () => User_richTextBox.Clear());
            SafeUpdateControl(Player1_richTextBox, () => Player1_richTextBox.Clear());
            SafeUpdateControl(Player2_richTextBox, () => Player2_richTextBox.Clear());
            SafeUpdateControl(Player3_richTextBox, () => Player3_richTextBox.Clear());
        }
        private void UpdateNameandPoints(string message)
        {
            List<string> names = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            
            foreach (string name in names)
            {
                if (name == Username_textBox.Text)
                {
                    int index = names.IndexOf(name);
                    SafeUpdateControl(UserPoint_textBox, () => UserPoint_textBox.Text = names[index + 1]);       //Add points of player here
                    names.Remove(name);
                    names.Remove(names[index]);
                    break;
                }

            }
            ClearNameAndPoints(false);
            for (int i = 0; i < names.Count; i += 2)
            {
                if (i == 0)
                {
                    SafeUpdateControl(Player1Name_textBox, () => Player1Name_textBox.Text = names[i]);
                    SafeUpdateControl(Player1Point_textBox, () => Player1Point_textBox.Text = names[i + 1]);

                }
                else if (i == 2)
                {
                    SafeUpdateControl(Player2Name_textBox, () => Player2Name_textBox.Text = names[i]);
                    SafeUpdateControl(Player2Point_textBox, () => Player2Point_textBox.Text = names[i + 1]);

                }
                else if (i == 4)
                {
                    SafeUpdateControl(Player3Name_textBox, () => Player3Name_textBox.Text = names[i]);
                    SafeUpdateControl(Player3Point_textBox, () => Player3Point_textBox.Text = names[i + 1]);
                }

            }
        }
        private void SendMessage(string message)
        {
            if (writer != null && client?.Connected == true)
            {
                try
                {
                    writer.WriteLine(message);
                }
                catch (Exception ex)
                {
                    UpdateConsole($"Error sending message: {ex.Message}");
                }
            }
        }

        private void UpdateConsole(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateConsole(message)));
            }
            else
            {
                Console_richTextBox.AppendText($"{DateTime.Now}: {message}\n");
                Console_richTextBox.ScrollToCaret();
            }
        }

        private void Rock_button_Click(object sender, EventArgs e)
        {
            UpdateConsole("You chose Rock!");
            move = "MOVE " + Username_textBox.Text + " rock";
        }

        private void Paper_button_Click(object sender, EventArgs e)
        {
            UpdateConsole("You chose Paper!");
            move = "MOVE " + Username_textBox.Text + " paper";
        }

        private void Scissors_button_Click(object sender, EventArgs e)
        {
            UpdateConsole("You chose Scissors!");
            move = "MOVE " + Username_textBox.Text + " scissors";
        }
        private void Disconnect()
        {
            if (client != null)
            {
                // Notify server about disconnection
                SendMessage($"DISCONNECT {Username_textBox.Text}");

                // Safe disposal pattern
                try
                {
                    // Close the writer and reader before the underlying stream
                    writer?.Close();
                    reader?.Close();

                    
                }
                catch (Exception ex)
                {
                    UpdateConsole($"Error during disconnection: {ex.Message}");
                }
                finally
                {
                    // Close the TcpClient
                    client.Close();
                    client = null;
                    

                    // Reset stream objects to null to avoid reuse
                    reader = null;
                    writer = null;

                    // Cancel ongoing tasks and dispose the cancellation token source
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = new CancellationTokenSource();

                    // Re-enable connection UI elements
                    SafeUpdateControl(Leave_button, () => Leave_button.Enabled = false);
                    ClearMoveDisplays();
                    ClearNameAndPoints(true);
                }
            }
        }

        private void ClearNameAndPoints(bool usercheck)
        {
            if (usercheck)
            {
                SafeUpdateControl(Username_textBox, () => Username_textBox.Clear());
                SafeUpdateControl(UserPoint_textBox, () => UserPoint_textBox.Clear());
            }
            SafeUpdateControl(Player1Name_textBox, () => Player1Name_textBox.Clear());
            SafeUpdateControl(Player1Point_textBox, () => Player1Point_textBox.Clear());
            SafeUpdateControl(Player2Name_textBox, () => Player2Name_textBox.Clear());
            SafeUpdateControl(Player2Point_textBox, () => Player2Point_textBox.Clear());
            SafeUpdateControl(Player3Name_textBox, () => Player3Name_textBox.Clear());
            SafeUpdateControl(Player3Point_textBox, () => Player3Point_textBox.Clear());
        }

        private void ClientForm_FormClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
            cancellationTokenSource.Dispose();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Player2_richTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Leave_button_Click(object sender, EventArgs e)
        {
            Disconnect();
            EnableConnection();
        }

        private void EnableConnection()
        {
            SafeUpdateControl(NameBox, () => NameBox.Enabled = true);
            SafeUpdateControl(IPBox, () => IPBox.Enabled = true);
            SafeUpdateControl(PortBox, () => PortBox.Enabled = true);
            SafeUpdateControl(Connect_button, () => Connect_button.Enabled = true);
       
        }
    }
}
