class Program
{
	private static Logger logger;

	public static void Main(string[] args)
	{
		// Make the universal logger
		logger = new Logger("Main");
		logger.Info("Multiplayer test (pigeon)");

		// Check for if they have the correct arguments
		if (args.Length != 2)
		{
			logger.Error("Insufficient arguments provided!\nPlease include an ip and port (must be in that order) Example:\nmultiplayer-test.exe 127.0.0.1 12345");
			return;
		}

		// Get all the info from the arguments
		string ip = args[0];
		int port = int.Parse(args[1]);
		logger.Info($"Starting client and server on {ip}:{port}");

		// Make a new client and server
		
	}
}