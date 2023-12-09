class Program
{
	public static void Main(string[] args)
	{
		Console.Title = "Client";

		// Parse all of the arguments
		LaunchArgsManager.ParseArguments(args);

		// Print the provided ip and port
		Console.WriteLine(LaunchArgsManager.Get("ip"));
		Console.WriteLine(LaunchArgsManager.Get("port"));

		Game.Run();

	}
}