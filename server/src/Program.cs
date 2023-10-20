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


	// Event queues
	// TODO: Play around with these values. idk the gpt gave them
	public const uint retransmissionTimeout = 350; //? Milliseconds 
	public const uint maxRetransmissions = 15;
	public static List<RetransmissionPacket> AcknowledgementPacketQueue = new List<RetransmissionPacket>();
	







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
		Thread retransmissionThread = new Thread(Retransmit);
		retransmissionThread.Start();
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
					case PacketType.ACK:
						Logger.Log("test 123");
						break;

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
				// Loop over all of the sent acknowledgement packets
				for (int i = 0; i < AcknowledgementPacketQueue.Count; i++)
				{
					RetransmissionPacket packet = AcknowledgementPacketQueue[i];

					// Get the elapsed time since the packet was sent
					TimeSpan elapsedTime = DateTime.Now - packet.SendTime;

					// Check for if the packet timed out (no response)
					if (elapsedTime.TotalMilliseconds >= retransmissionTimeout)
					{
						// Send the packet again
						Logger.Log("Packet transmission failed. Retransmitting.", LogType.WARN);
						PacketHandler.SendPacket(packet.Content, packet.Client);
						packet.SendTime = DateTime.Now;
						packet.TimesSent++;
					}

					// Check for if the packet has been sent more than the max times its allowed (timed-out)
					if (packet.TimesSent > maxRetransmissions)
					{
						// Remove the packet from the acknowledgement packet queue
						Logger.Log($"Connection timed-out after too many failed attempts.", LogType.ERROR);
						AcknowledgementPacketQueue.Remove(packet);
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
