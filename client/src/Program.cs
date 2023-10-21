class Program
{
	public static void Main(string[] args)
	{
		// Check for if the correct amount of args are supplied
		if (args.Length < 3)
		{
			// TODO: Put this in a table and show example args
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Insufficient arguments provided!");
			Console.WriteLine("1)\tserver ip\trequired: yes");
			Console.WriteLine("2)\tserver port\trequired: yes");
			Console.WriteLine("3)\tusername\trequired: yes");
			Console.WriteLine("4)\tdebug messages\trequired: no");
			Console.ResetColor();
		}
		
		// Check for if they wish to enable debug mode
		if (args.Length >= 4 && args[3].ToLower() == "true")
		{
			Logger.LogMessages = true;
			Logger.LogPackets = true;
		}

		// Launch the game
		Game game = new Game();
		game.Run(args[0], args[1], args[2]);
	}
}