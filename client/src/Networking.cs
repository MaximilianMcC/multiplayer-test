using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

class Networking
{
	private static UdpClient client = new UdpClient();
	private static IPEndPoint server;

	// Connect to a server (only client will need this but still putting it in shared)
	public static void ConnectToServer(string serverIp, string serverPort)
	{
		// Parse the IP and port
		IPAddress ip = IPAddress.Parse(serverIp);
		int port = int.Parse(serverPort);

		// Create the server endpoint
		server = new IPEndPoint(ip, port);

		// Ping the server to check for if its online and we entered 
		// the correct ip and port
		const int PING_TIMEOUT = 1000; //? milliseconds
		Ping ping = new Ping();

		try
		{
			PingReply reply = ping.Send(ip, PING_TIMEOUT);
			if (reply.Status == IPStatus.Success) Console.WriteLine("blutettoth deveice connected suceusfuy.!!!");
		}
		catch (PingException)
		{
			Console.WriteLine("Erhmmm i dont think the server is reachable rn (busy)");
		}

		// TODO: Use .Status enum to get more accurate messages. Cast to string or something
	}
}