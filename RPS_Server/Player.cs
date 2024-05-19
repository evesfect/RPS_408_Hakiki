using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Player 
{
    public string Username { get; set; }
    public TcpClient Client { get; private set; }
    public int Wins { get; set; }
    public StreamWriter Writer { get; private set; }
    public SemaphoreSlim SemaphoreSlim { get; } = new SemaphoreSlim(1, 1);
    public Player(TcpClient client, string username)
	{
        Client = client;
        Username = username;
        Wins = 0;
        Writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };
    }
}
