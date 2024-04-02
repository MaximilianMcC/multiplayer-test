class Program
{
	private static Logger logger;

	public static void Main(string[] args)
	{
		// Make the universal logger
		logger = new Logger("Main");

		logger.Info("test 123");
		logger.Event("test 123");
		logger.Error("test 123");

		// Get the IP and port from the args
		if (args.Length != 2)
		{
			
		}

		// Make a new client and server
		
	}
}