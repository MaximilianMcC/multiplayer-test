class Server
{
	public static void Main(string[] args)
	{
		// Title crap
		Console.Title = "UDP Server";
		Console.WriteLine("C# Multiplayer UDP server test");

		// Parse the port from the args
		int port = int.Parse(args[0].Trim());
	}
}