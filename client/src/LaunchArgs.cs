using System.Dynamic;

class LaunchArgsManager
{
	// TODO: Way to have required arguments

	private static Dictionary<string, string> parsedArguments = new Dictionary<string, string>();

	// Parse all of the arguments
	// TODO: Support other data types
	//? Argument format is `-myarg myValue` btw!!
	public static void ParseArguments(string[] args)
	{
		// Temporary key/values
		string key = "";
		string value = "";

		// Loop through all of the arguments
		for (int i = 0; i < args.Length; i++)
		{
			// Check for if we are looking at a key, or a value
			if (args[i].StartsWith('-')) key = args[i].Trim();
			else value = args[i].Trim();
			
			// Check for if a complete pair has been gotten (key and value)
			if (key != "" && value != "")
			{
				// Add the key and value to the list of parsed arguments
				parsedArguments.Add(key.Replace("-", ""), value);

				// Reset them for the next argument
				key = "";
				value = "";
			}
		}

		// TODO: Check for if all of the required arguments have been provided
	}

	// Get an argument from the list of set arguments here
	public static string? Get(string argumentName)
	{
		// Loop through every parsed argument until we find the correct one
		// TODO: Could entirely skip the loop and just use `parsedArguments[argument.Key]`
		foreach (KeyValuePair<string, string> currentArgument in parsedArguments)
		{
			if (currentArgument.Key == argumentName) return parsedArguments[argumentName];
		}

		// Return null if there was no argument given
		return null;
	}
}