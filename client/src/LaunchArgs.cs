using System.Dynamic;

class LaunchArgsManager
{
	// Make all of the launch arguments
	private static List<LaunchArg> arguments = new List<LaunchArg>()
	{
		new LaunchArg("Ip", DataType.STRING, true),
		new LaunchArg("Port", DataType.INT, true),
	};

	// "load" all of the args
	// TODO: Check for if all required ones are there
	//! System for getting key/value parsed is really shoddy. Don't be a dick when adding args
	public static dynamic GetArguments(string[] args)
	{
		// ExpandoObject thingy makes the .Whatever happen
		dynamic launchArgs = new ExpandoObject();
		
		// Loop through all of the arguments, and parse them
		for (int i = 0; i < args.Length; i++)
		{
			// Check for if the current argument is a key or a value
			string currentArgument = args[i];
			if (currentArgument.StartsWith('-'))
			{
				// Add the argument
				foreach (LaunchArg argument in arguments)
				{
					if (argument.Key == currentArgument)
					{
						// TODO: Find a better way to do this
						switch (argument.Type)
						{
							case DataType.STRING:
								((IDictionary<string, string>)launchArgs)[argument.Key] = args[i + 1];
								break;

							case DataType.INT:
								((IDictionary<string, int>)launchArgs)[argument.Key] = int.Parse(args[i + 1]);
								break;

							case DataType.FLOAT:
								((IDictionary<string, float>)launchArgs)[argument.Key] = float.Parse(args[i + 1]);
								break;

							case DataType.BOOL:
								((IDictionary<string, bool>)launchArgs)[argument.Key] = bool.Parse(args[i + 1]);
								break;
						}
					}
				}
			}

			
		}

		// Give back the arguments
		return launchArgs;
	}




	// Launch argument template thingy
	//! idk if its correct to use struct here (it works though!!)
	struct LaunchArg
	{
		public string Key { get; private set; }
		public DataType Type { get; private set; }
		public bool Required { get; private set; }

		public LaunchArg(string key, DataType type, bool required = false)
		{
			Key = key;
			Type = type;
			Required = required;
		}
	}

	// TODO: Check for if there is already some built in thing of this
	enum DataType
	{
		STRING,
		INT,
		FLOAT,
		BOOL
	}
}