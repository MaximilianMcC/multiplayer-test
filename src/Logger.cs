class Logger
{
	// Identifier thingy so you know whats
	// showing the message
	private string identifier;
	
	//  ANSI color codes.
	//? Using these instead of C# ConsoleColor because its quicker
	private const string reset = "\x1b[0m";
	private const string cyan = "\x1b[36m";
	private const string white = "\x1b[37m";
	private const string magenta = "\x1b[35m";
	private const string blue = "\x1b[34m";
	private const string yellow = "\x1b[33m";
	private const string red = "\x1b[31m";




	public Logger(string identifierName)
	{
		identifier = identifierName;
	}


	public void Info(object content)
	{
		LogMessage("INFO", blue, content);
	}

	public void Event(object content)
	{
		LogMessage("EVENT", yellow, content);
	}

	public void Error(object content)
	{
		LogMessage("ERROR", red, content);
	}


	private void LogMessage(string type, string color, object contents)
	{
		// Get the time
		string time = DateTime.Now.ToString("HH:mm:ss:fff");

		// Make the message and print each line
		// TODO: Make multiple lines for readability
		foreach (string line in contents.ToString().Split("\n"))
		{
			Console.WriteLine($"{white}[{cyan}{time}{white}] [{magenta}{identifier}{white}]\t({color}{type}{white})\t{color}{line}");
		}
	}
}