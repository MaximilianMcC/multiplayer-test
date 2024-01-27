using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

class Program
{
	public static UdpClient Server;

	public static void Main(string[] args)
	{
		// crap heading thing
		Console.Title = "Server test";
		Console.ForegroundColor = ConsoleColor.DarkYellow;
		Console.WriteLine(@"        _     _                        ");
		Console.WriteLine(@"       (_)   | |                       ");
		Console.WriteLine(@"  _ __  _  __| | ___  ___  _ __   __ _ ");
		Console.WriteLine(@" | '_ \| |/ _` |/ _ \/ _ \| '_ \ / _` |");
		Console.WriteLine(@" | |_) | | (_| |  __/ (_) | | | | (_| |");
		Console.WriteLine(@" | .__/|_|\__,_|\___|\___/|_| |_|\__,_|");
		Console.WriteLine(@" | |                                   ");
		Console.WriteLine(@" |_|       UDP SERVER                  ");
		Console.WriteLine("------------------------------------------------------------\n");


		// Check for if the correct args were supplied
		if (args.Length < 1)
		{
			Console.WriteLine("Insufficient arguments provided🤬\nYou are missing the server port. Provide it as a string.\nExample: server.exe 12345");
			return;
		}

		// Make the actual server
		Server = new UdpClient(int.Parse(args[0]));

		// Listen to any client, using any ip on any port
		IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);

		// Listen for incoming requests
		Console.WriteLine("Listening for the incoming packets rn");
		while (true)
		{
			// Get the incoming packet
			byte[] incomingPacket = Server.Receive(ref clientEndpoint);
			Packet request = new Packet(incomingPacket);

			// Handle it
			HandleRequest(request);
		}
	}



	private static void HandleRequest(Packet request)
	{
		// Check for the type of request
		if (request.Handshake != null)
		{
			// Handshake packet
			Console.WriteLine("received handshake packet");

			//? .Value being used here because its a struct
			Networking.AcknowledgeHandshake(request.Handshake.Value);
		}
		else
		{
			// Normal packet
			Console.WriteLine("received normal packet");
		}

		// Unknown packet
		// TODO: Make proper logger and error system
		Debug.WriteLine("Error handing packet. Unknown request type.");
	}
}