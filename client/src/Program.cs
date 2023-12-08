class Program
{
	public static dynamic LaunchArgs;

	public static void Main(string[] args)
	{
		Console.Title = "Client";


		LaunchArgs = LaunchArgsManager.GetArguments(args);


		Console.WriteLine(LaunchArgs.ip);
		Console.WriteLine(LaunchArgs.port);
		Game.Run();

	}
}