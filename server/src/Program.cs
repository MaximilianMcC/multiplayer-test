using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (slows down server performance)
	public static bool FancyUi = true;


	// General server stuff
	public static UdpClient UdpServer;
	public static List<Player> PlayerList = new List<Player>();


	// Event queues
	private const int packetResendTimeout = 10; //? Seconds
	// TODO: Use struct instead
	private static List<Connection> connectionQueue = new List<Connection>();
	//! private static List<Disconnection> connectionQueue = new List<Disconnection>();






	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "mutliplauyer test sererv";

		// Get the server port from the launch arguments
		int port = int.Parse(args[0]);

		// Create the UDP server
		UdpServer = new UdpClient(port);
		Logger.Log($"Server listening on port {port}...");


		while (true)
		{
			try
			{
				// Get the currently connecting client
				IPEndPoint currentClient = new IPEndPoint(IPAddress.Any, 0);

				// Get the packet data
				byte[] receivedPacketBytes = UdpServer.Receive(ref currentClient);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
				Logger.LogPacket(receivedPacket, Logger.PacketLogType.INCOMING, "UNKNOWN");

				// Get the packet type so we can see what data is being sent
				string[] packet = receivedPacket.Split(',');
				PacketType packetType = (PacketType)int.Parse(packet[0]);



				// Handle everything
				HandleEvents(currentClient);
				HandlePackets(currentClient, packetType, packet);



			}
			catch (Exception e)
			{
				// Print out the error
				Logger.Log("Error while running server!", Logger.LogType.ERROR);
				Logger.Log(e.ToString(), Logger.LogType.ERROR);
			}
		}
	
	}



	// Events are something that is important. Stuff like a player joining/leaving.
	// These require a three-way handshake
	private static void HandleEvents(IPEndPoint client)
	{

		// Check for if a player has sent back a connection acknowledgement packet
		foreach (Connection connectingPlayer in connectionQueue)
		{
			// Check for if the packet has timed out. If it has then either the packet
			// got lost, or the client is ignoring us (rude)
			TimeSpan timeElapsed = DateTime.Now - connectingPlayer.SendTime;
			if (timeElapsed.TotalSeconds <= packetResendTimeout) continue;
			
			// Send the packet again
			
		}
	}



	private static void HandlePackets(IPEndPoint client, PacketType packetType, string[] packet)
	{
		// Check for if a client is trying to connect
		if (packetType == PacketType.CONNECT)
		{
			// Use the information received to create a new player
			string username = packet[1];
			uint color = uint.Parse(packet[2]);
			Player player = new Player(username, color);

			// Get the players UUID and send it back to them
			string connectionPacket = $"{(int)PacketType.CONNECT_RESPONSE},{player.Uuid}";
			SendPacket(connectionPacket, client);

			// Wait for an acknowledgment packet from the player then
			// add them to the game once we know they are connected
			connectionQueue.Add(new Connection(player, connectionPacket));
		}


		// TODO: Check for if a client is trying to disconnect



		// TODO: Send remote player data



	}







	// Send a packet
	private static void SendPacket(string packet, IPEndPoint client)
	{
		// Encode the packet
		byte[] packetBytes = Encoding.ASCII.GetBytes(packet);

		// Send the packet
		UdpServer.Send(packetBytes, packetBytes.Length, client);
	}


}


public enum PacketType
{
	CONNECT = 1,
	CONNECT_RESPONSE = 2,

	DISCONNECT = 3,

	PLAYER_UPDATE = 4
}



struct Connection
{
	public Player Player { get; set; }
	public string Packet { get; set; }
	public DateTime SendTime { get; set; }

	public Connection(Player player, string packet)
	{
		Player = player;
		Packet = packet;
		SendTime = DateTime.Now;
	}
}