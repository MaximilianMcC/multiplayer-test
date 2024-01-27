using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

class Program
{
	private static UdpClient server;

	public static void Main(string[] args)
	{
		// crap heading thing
		Console.Title = "Server test";
		Console.ForegroundColor = ConsoleColor.DarkYellow;
		Console.WriteLine("Multiplayer Testing Server (pideona)\n------------------------------------------\n\n");

		// Make the actual server
		server = new UdpClient(54321);

		// Listen to any client, using any ip on any port
		IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);

		// Listen for incoming requests
		Console.WriteLine("Listening for the incoming packets rn");
		while (true)
		{
			byte[] incomingBytes = server.Receive(ref clientEndpoint);
			Packet request = new Packet(incomingBytes);
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