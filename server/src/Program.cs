using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (Might slow down server performance)
	private static bool fancyUi = true;


	private static UdpClient server;


	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "sererv";

		// Get the server port from the args
		int port = int.Parse(args[0]);

		// Create the UDP server
		server = new UdpClient(port);
		try
		{
			Console.WriteLine($"UDP Sever listening on port {port}...\n");

			while (true)
			{
				// Get the currently connecting client and their data
				IPEndPoint currentClient = new IPEndPoint(IPAddress.Any, 0);
				byte[] receivedPacketBytes = server.Receive(ref currentClient);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);

				// Print the packet
				Log(receivedPacket, LogType.OUTGOING_PACKET);


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
























	enum LogType
	{
		INFO,
		WARN,
		ERROR,

		INCOMING_PACKET,
		OUTGOING_PACKET
	}

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
				StringText(content);
				break;

			case LogType.WARN:
				BoxedText("WARN", ConsoleColor.Yellow);
				StringText(content);
				break;

			case LogType.ERROR:
				BoxedText("ERROR", ConsoleColor.Red);
				StringText(content);
				break;






			case LogType.INCOMING_PACKET:
				BoxedText("INCOMING", ConsoleColor.Magenta);
				BoxedText($"{Encoding.ASCII.GetBytes(content).Length} bytes", ConsoleColor.DarkYellow);
				StringText(content);
				break;

			case LogType.OUTGOING_PACKET:
				BoxedText("OUTGOING", ConsoleColor.DarkCyan);
				BoxedText($"{Encoding.ASCII.GetBytes(content).Length} bytes", ConsoleColor.DarkYellow);
				StringText(content);
				break;
		}

		Console.WriteLine();
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

	private static void StringText(string text)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write($"\"{text}\"");
		Console.ResetColor();
	}
}