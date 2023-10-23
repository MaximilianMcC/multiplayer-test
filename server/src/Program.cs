using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (slows down server performance)
	public static bool FancyUi = true;
	public static bool LogPackets = true;


	// General server stuff
	public static int Port;
	public static UdpClient UdpServer;
	public static List<Player> PlayerList = new List<Player>();
	







	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "mutliplauyer test sererv";

		// Get the server port from the launch arguments
		Port = int.Parse(args[0]);

		// Create the UDP server
		UdpServer = new UdpClient(Port);

		

		// Start listening for packets
		Thread listenThread = new Thread(Listen);
		listenThread.Start();

		// Start handling retransmissions
		// Thread retransmissionThread = new Thread(Retransmit);
		// retransmissionThread.Start();
	}



	private static void Listen()
	{
		Logger.Log($"Server listening on port {Port}...");
		while (true)
		{
			try
			{
				// Get the currently connecting client
				IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);

				// Get the packet data
				byte[] receivedPacketBytes = UdpServer.Receive(ref client);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
				Logger.LogPacket(receivedPacket, PacketLogType.INCOMING, "UNKNOWN");

				// Get the packet type so we can see what data is being sent
				string[] packet = receivedPacket.Split(',');
				PacketType packetType = (PacketType)int.Parse(packet[0]);

				// Do something with the packet
				switch (packetType)
				{
					// Check for if a retransmission was received and sent back
					// case PacketType.:
					// 	Logger.Log("test 123");
					// 	break;

					// New player connect to the server
					case PacketType.CONNECT:
						PacketHandler.ConnectPlayer(packet, client);
						break;



					default:
						Logger.Log("erhmmm, this packet doesn't seem to have a legitimate packet id!!1!", LogType.ERROR);
						break;
				}
			}
			catch (Exception e)
			{
				// Print out the error
				Logger.Log("Error while listening!", LogType.ERROR);
				Logger.Log(e.ToString(), LogType.ERROR);
			}
		}
	}

	private static void Retransmit()
	{
		Logger.Log($"Began handling retransmissions");
		while (true)
		{
			try
			{
				// Loop over all of the retransmission packets
				for (int i = 0; i < PacketHandler.RetransmissionPacketQueue.Count; i++)
				{
					// Get the current packet
					RetransmissionPacket packet = PacketHandler.RetransmissionPacketQueue[i];


					// Check for if the packet needs to be retransmitted
					TimeSpan timeSinceLastTransmitted = DateTime.Now - packet.LastTimeSent;
					if (timeSinceLastTransmitted.Milliseconds >= PacketHandler.retransmissionTimeout)
					{
						// Resend the packet
						packet.Send();
					}

				}
			}
			catch (Exception e)
			{
				// Print out the error
				Logger.Log("Error while retransmitting!", LogType.ERROR);
				Logger.Log(e.ToString(), LogType.ERROR);
			}
		}
	}
}
