using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (Might slow down server performance)
	public static bool FancyUi = true;

	// TODO: Don't use IP to identify players. Use UUID then multiple on same pc can join
	public static UdpClient UdpServer;
	public static Dictionary<IPEndPoint, Player> PlayerList = new Dictionary<IPEndPoint, Player>();

	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "sererv";

		// Get the server port from the launch arguments
		int port = int.Parse(args[0]);

		// Create the UDP server
		UdpServer = new UdpClient(port);
		try
		{
			Logger.Log($"Server listening on port {port}...");

			while (true)
			{
				// Get the currently connecting client
				IPEndPoint currentClient = new IPEndPoint(IPAddress.Any, 0);

				// Get the packet data
				byte[] receivedPacketBytes = UdpServer.Receive(ref currentClient);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
				Logger.LogPacket(receivedPacket, Logger.PacketLogType.INCOMING);

				// Get the packet type and check for if they want to connect
				PacketType packetType = (PacketType)int.Parse(receivedPacket.Split(',')[0]);
				if (packetType == PacketType.CONNECT)
				{
					Logger.Log("connecting rn");

					
					// Parse the packet to get the color and username
					string[] packetData = receivedPacket.Split(',');
					uint color = uint.Parse(packetData[1]);
					string username = packetData[2];

					// Create, then add the player to the player list
					Player player = new Player(currentClient, color, username);
					PlayerList.Add(currentClient, player);

					// Send back the players new UUID
					// TODO: If Removing debug stuff them remove connectionPacket string because its useless here
					string connectionPacket = player.Uuid;
					byte[] connectionPacketBytes = Encoding.ASCII.GetBytes(connectionPacket);
					UdpServer.Send(connectionPacketBytes, connectionPacketBytes.Length, currentClient);
					Logger.LogPacket(connectionPacket, Logger.PacketLogType.OUTGOING);

					// Start a new thread to handle the player
					Thread handlePlayer = new Thread(player.Handle);
					handlePlayer.Start();
				}
			}
		}
		catch (Exception e)
		{
			// Print out the error
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error while running server:");
			Console.WriteLine(e);
			Console.ResetColor();
		}
		finally
		{
			// TODO: Remove. This is never actually run.
			// Close the UDP server
			UdpServer.Close();
			Console.WriteLine("Server shutdown.");
		}
	}
}

public enum PacketType
{
	CONNECT = 0,
	DISCONNECT = 1,

	PLAYER_UPDATE = 2
}