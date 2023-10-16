using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (slows down server performance)
	public static bool FancyUi = true;

	// TODO: Don't use IP to identify players. Use UUID then multiple on same pc can join
	public static UdpClient UdpServer;
	public static List<Player> PlayerList = new List<Player>();
	private static Dictionary<Player, DateTime> disconnectionQueue = new Dictionary<Player, DateTime>();

	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "sererv";

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
				PacketType packetType = (PacketType)int.Parse(receivedPacket.Split(',')[0]);



				// TODO: Check for if a client is trying to connect



				// TODO: Check for if a client is trying to disconnect



				// TODO: Send remote player data


			
			}
			catch (Exception e)
			{
				// Print out the error
				Logger.Log("Error while running server!", Logger.LogType.ERROR);
				Logger.Log(e.ToString(), Logger.LogType.ERROR);
			}
		}
	
	}
}


public enum PacketType
{
	CONNECT = 1,
	CONNECT_RESPONSE = 2,

	DISCONNECT = 3,

	PLAYER_UPDATE = 4
}
