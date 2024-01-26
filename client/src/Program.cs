class Program
{
	public static string[] Args;

	public static void Main(string[] args)
	{
		// Set the args
		Args = args;
		if (Args.Length == 0)
		{
			// Show an error message if wrong args were given
			Console.WriteLine("Insufficient arguments provided🤬\nYou are missing the server port, and ip. Provide them in a single string, separated with a colon. <IP>:<PORT>\nExample: client.exe 127.0.0.1:12345");
			return;
		}

		Game.Run();
	}
}