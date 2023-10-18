using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (slows down server performance)
	public static bool FancyUi = true;
	public static bool LogPackets = true;


	// General server stuff
	public static UdpClient UdpServer;
	public static List<Player> PlayerList = new List<Player>();


	// Event queues
	private const int retransmissionTimeout = 3; //? Seconds
	private static Dictionary<string, DateTime> acknowledgementPacketQueue = new Dictionary<string, DateTime>();
	






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



	// Retransmit acknowledgement packets if needed
	private static void HandleEvents(IPEndPoint client)
	{
		// TODO: Count how many retransmission packets have been sent. if over 100 or something then cancel/give up
		
		// Loop over all of the sent acknowledgement packets
		foreach (KeyValuePair<string, DateTime> packet in acknowledgementPacketQueue)
		{
			// Get the elapsed time since the packet was sent
			TimeSpan elapsedTime = DateTime.Now - packet.Value;

			// Check for if the packet timed out (no response)
			if (elapsedTime.TotalSeconds >= retransmissionTimeout)
			{
				// Send the packet again
				Logger.Log("Packet transmission failed. Retransmitting.", Logger.LogType.WARN);
				SendPacket(packet.Key, client);
			}
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
			SendAcknowledgementPacket(connectionPacket, client);
		}


		// TODO: Check for if a client is trying to disconnect



		// TODO: Send remote player data



	}







	// Send a packet
	private static void SendPacket(string packet, IPEndPoint client)
	{
		// Encode, then send the packet
		byte[] packetBytes = Encoding.ASCII.GetBytes(packet);
		UdpServer.Send(packetBytes, packetBytes.Length, client);

		// Log it
		Logger.LogPacket(packet, Logger.PacketLogType.OUTGOING, client.ToString());
	}

	// Send an acknowledgement packet
	// Where a response must be sent by the client. If one isn't sent then
	// the packet will be retransmitted until received successfully
	private static void SendAcknowledgementPacket(string packet, IPEndPoint client)
	{
		// TODO: Add something to the packet type that states its an acknowledgement packet
		// TODO: Add a guid that is used to relate a packet so we know to remove it once its been received
		acknowledgementPacketQueue.Add(packet, DateTime.Now);
	}
}


public enum PacketType
{
	CONNECT = 1,
	CONNECT_RESPONSE = 2,

	DISCONNECT = 3,

	PLAYER_UPDATE = 4
}