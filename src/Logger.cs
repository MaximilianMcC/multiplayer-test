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

	public void Info(object? content)
	{
		Console.WriteLine($"{GetHeader()}{white}({blue}INFO{white})\t{blue}{content}");
	}

	public void Event(object? content)
	{
		Console.WriteLine($"{GetHeader()}{white}({yellow}EVENT{white})\t{yellow}{content}");
	}

	public void Error(object? content)
	{
		Console.WriteLine($"{GetHeader()}{white}({red}ERROR{white})\t{red}{content}");
	}


	private string GetHeader()
	{
		// Get the time
		string time = DateTime.Now.ToString("HH:mm:ss:fff");

		// Give back the string
		return $"{white}[{cyan}{time}{white}] [{magenta}{identifier}{white}] ";
	}
}