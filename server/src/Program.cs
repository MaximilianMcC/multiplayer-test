using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (Might slow down server performance)
	private static bool fancyUi = true;

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
			Log($"Server listening on port {port}...");

			while (true)
			{
				// Get the currently connecting client and their data
				IPEndPoint currentClient = new IPEndPoint(IPAddress.Any, 0);
				byte[] receivedPacketBytes = server.Receive(ref currentClient);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);

				// Print the packet
				LogPacket(receivedPacket, PacketLogType.OUTGOING);

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

					// TODO: Start a new thread to handle the player
				}
				
				// Remove them from the player (disconnect)
				if (packetType == PacketType.DISCONNECT)
				{
					// Remove the player from the list
					// TODO: Put the message after
					Log($"Disconnected player {playerList[currentClient].Uuid}");
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
















	// TODO: Make it so that every client/player gets x different lines allowed for them to clear up space and stuff.


	enum LogType
	{
		INFO,
		WARN,
		ERROR
	}

	enum PacketLogType
	{
		INCOMING,
		OUTGOING
	}

	// TODO: Chance the color of stuff in quotes, or numbers in a string to highlight important info
	private static void Log(string content, LogType logType = LogType.INFO)
	{
		// Check for if we allow fancy UI
		if (fancyUi == false) return;

		// Timestamp
		BoxedText(DateTime.Now.ToString("HH:mm:ss.fff"), ConsoleColor.Cyan);

		switch (logType)
		{
			case LogType.INFO:
				BoxedText("INFO", ConsoleColor.Blue);
				break;

			case LogType.WARN:
				BoxedText("WARN", ConsoleColor.Yellow);
				break;

			case LogType.ERROR:
				BoxedText("ERROR", ConsoleColor.Red);
				break;
		}

		// Print the content
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(content);
		Console.ResetColor();
	}

	// TODO: Also show IP
	private static void LogPacket(string packet, PacketLogType packetType)
	{
		// Check for if we allow fancy UI
		if (fancyUi == false) return;

		// Timestamp
		BoxedText(DateTime.Now.ToString("HH:mm:ss.fff"), ConsoleColor.Cyan);

		switch (packetType)
		{
			case PacketLogType.INCOMING:
				BoxedText("INCOMING", ConsoleColor.Magenta);
				break;

			case PacketLogType.OUTGOING:
				BoxedText("OUTGOING", ConsoleColor.DarkCyan);
				break;
		}

		// Bytes
		BoxedText($"{Encoding.ASCII.GetBytes(packet).Length} bytes", ConsoleColor.DarkYellow);

		// Print the packet
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"\"{packet}\"");
		Console.ResetColor();
	}

	private static void BoxedText(string text, ConsoleColor color)
	{
		Console.ForegroundColor = ConsoleColor.Gray;
		Console.Write('[');
		Console.ForegroundColor = color;
		Console.Write(text);
		Console.ForegroundColor = ConsoleColor.Gray;
		Console.Write("] ");
		Console.ResetColor();
	}
}



class Player
{
	// Player values
	public string Uuid { get; private set; }
	public string Username { get; private set; }
	public uint Color { get; set; }
	public float PositionX { get; set; }
	public float PositionY { get; set; }

	// Create a new player
	public Player(uint color, string username)
	{
		// Assign the starting values
		Uuid = Guid.NewGuid().ToString();
		Color = color;
		Username = username;
	}
}

public enum PacketType
{
	CONNECT = 1,
	DISCONNECT = 2,

	PLAYER_UPDATE = 3
}