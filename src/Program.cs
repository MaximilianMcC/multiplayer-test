class Program
{
	private static Logger logger;

	public static void Main(string[] args)
	{
		// Make the universal logger thing
		logger = new Logger("Main");
		logger.Info("Multiplayer test (pigeon)");

		// Check for if they have the correct arguments
		if (args.Length != 3)
		{
			logger.Error("Insufficient arguments provided!\nPlease include an ip and port, also the type of program that you are launching (client or server)Example:\nmultiplayer-test.exe 127.0.0.1 12345 client");
			return;
		}

		// Get all the info from the arguments
		string ip = args[0];
		int port = int.Parse(args[1]);
		bool isClient = args[2].ToLower() == "client";

		// Check for what the type of program they want
		// then make and launch it
		if (isClient) new Client(ip, port);
		else new Server(port);
	}
}