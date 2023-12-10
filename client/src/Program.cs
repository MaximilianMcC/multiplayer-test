class Program
{
	public static void Main(string[] args)
	{
		Console.Title = "Client";

		// Parse all of the arguments
		LaunchArgs.ParseArguments(args);

		Game.Run();

	}
}