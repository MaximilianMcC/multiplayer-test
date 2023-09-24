using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	private static UdpClient server;

	// Keep track of players using their UUID
	private static Dictionary<IPEndPoint, Player> connections = new Dictionary<IPEndPoint, Player>();


	public static void Main(string[] args)
	{
		// Title crap
		Console.Title = "UDP Server";
		Console.CursorVisible = false;

		// Parse the port from the args and create the UDP server
		int port = int.Parse(args[0].Trim());
		server = new UdpClient(port);

		// Begin the sending, and listening threads
		Thread listenThread = new Thread(Listen);
		Thread sendThread = new Thread(Send);
		listenThread.Start();
		sendThread.Start();

		// Stop main thread from closing
		while (true)
		{
			//! This stuff in here isn't needed, but the while loop is to stop the main thread from finishing
			// Print all of the online players
			Console.SetCursorPosition(0, 0);
			Console.WriteLine($"{connections.Count} online players.");
		}
	}

	// Listen to all incoming packets
	// Also add/remove new players
	private static void Listen()
	{
		while (true)
		{
			try
			{
				// Get the client
				IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);

				// Get the data they're sending and decode it
				byte[] receivedPacket = server.Receive(ref client);
				string packetString = Encoding.ASCII.GetString(receivedPacket);

				// Check for if the current client is new/not in player list
				if (connections.ContainsKey(client) == false)
				{
					// Create a new player and add them to the list
					Player player = new Player(client);
					connections.Add(client, player);

					Console.WriteLine("New player joined with UUID of " + connections[client].Uuid);
				}


			}
			catch (Exception)
			{
				Console.SetCursorPosition(0, 10);
				Console.WriteLine("!!!");
			}
		}
	}

	// Send packets
	private static void Send()
	{

	}
}







// TODO: Put in another file
class Player
{
	public IPEndPoint Ip { get; private set; }
	public string Uuid { get; private set; }
	public float XPosition { get; set; }
	public float YPosition { get; set; }

	// Create a new player
	public Player(IPEndPoint ip)
	{
		// Assign the player ip so we know who controls them
		Ip = ip;

		// Assign the player a random UUID so we know who they are
		Uuid = Guid.NewGuid().ToString();
	}
}