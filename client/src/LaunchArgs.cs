using System.Dynamic;

class LaunchArgsManager
{
	// All of the arguments
	public static List<LaunchArg> launchArguments = new List<LaunchArg>()
	{
		new LaunchArg("ip", true),
		new LaunchArg("port", true)
	};

	private Dictionary<string, string> parsedArguments = new Dictionary<string, string>();

	// Parse all of the arguments
	public static void ParseArguments(string[] args)
	{
		// Argument format is this:
		// `-myarg myValue`
	}

	// Get an argument from the list of set arguments here
	public static string Get(LaunchArg argument)
	{
		string data = "";



		return data;
	}


}

// Launch argument template thingy
//! idk if its correct to use struct here (it works though!!)
// TODO: Add in a way to choose datatype
struct LaunchArg
{
	public string Key { get; private set; }
	public bool Required { get; private set; }

	public LaunchArg(string key, bool required = false)
	{
		Key = key;
		Required = required;
	}
}