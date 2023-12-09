class Program
{
	public static dynamic LaunchArgs;

	public static void Main(string[] args)
	{
		Console.Title = "Client";


		LaunchArgs = LaunchArgsManager.GetArguments(args);

		// TODO: Don't do manually
		LaunchArgs.Ip = "127.0.0.1";
		LaunchArgs.Port = "12345";

		Console.WriteLine(LaunchArgs.Ip);
		Console.WriteLine(LaunchArgs.Port);
		Game.Run();

	}
}