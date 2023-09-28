using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (Might slow down server performance)
	public static bool FancyUi = true;

	// TODO: Don't use IP to identify players. Use UUID then multiple on same pc can join
	private static UdpClient server;
	private static Dictionary<IPEndPoint, Player> playerList = new Dictionary<IPEndPoint, Player>();

	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "sererv";

		// Get the server port from the launch arguments
		int port = int.Parse(args[0]);

		// Create the UDP server
		server = new UdpClient(port);
		try
		{
			Logger.Log($"Server listening on port {port}...");

			while (true)
			{
				// Get the currently connecting client and their data
				IPEndPoint currentClient = new IPEndPoint(IPAddress.Any, 0);
				byte[] receivedPacketBytes = server.Receive(ref currentClient);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);

				// Print the packet
				Logger.LogPacket(receivedPacket, Logger.PacketLogType.OUTGOING);

				// Get the packet type to determine what the client wants to do
				PacketType packetType = (PacketType)byte.Parse(receivedPacket.Split(',')[0]);

				// Add them as a new player (connect)
				if (packetType == PacketType.CONNECT)
				{
					// Parse the packet to get the color and username
					string[] packetData = receivedPacket.Split(',');
					uint color = uint.Parse(packetData[0]);
					string username = packetData[1];

					// Create, then add the player to the player list
					Player player = new Player(color, username);
					playerList.Add(currentClient, player);

					// Start a new thread to handle the player
					Thread handlePlayer = new Thread(player.Handle);
					handlePlayer.Start();
				}
				
				// Remove them from the player (disconnect)
				if (packetType == PacketType.DISCONNECT)
				{
					// Remove the player from the list
					// TODO: Put the message after
					Logger.Log($"Disconnected player {playerList[currentClient].Uuid}");
					playerList.Remove(currentClient);
				}

			}
		}
		catch(Exception e)
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
			server.Close();
			Console.WriteLine("Server shutdown.");
		}
	}
}

public enum PacketType
{
	CONNECT = 1,
	DISCONNECT = 2,

	PLAYER_UPDATE = 3
}