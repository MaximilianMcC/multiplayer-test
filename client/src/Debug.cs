using System.Text;

class Logger
{
	public static bool LogMessages { get; set; }
	public static bool LogPackets { get; set; }


	public enum LogType
	{
		INFO,
		WARN,
		ERROR
	}

	public enum PacketLogType
	{
		INCOMING,
		OUTGOING
	}


	public static void Log(string message, LogType logType = LogType.INFO)
	{
		// Check for if we are allowed to log
		if (!LogMessages) return;

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
				Console.ForegroundColor = ConsoleColor.Red;
				break;
		}

		// Print the content
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(message);
		Console.ResetColor();
	}

	public static void LogPacket(string packet, PacketLogType packetType, string client)
	{
		// Check for if we are allowed to log
		if (!LogPackets) return;

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

		// Client thing
		BoxedText(client, ConsoleColor.Blue);

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