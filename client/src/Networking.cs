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

		// TODO: Somehow ping the server to check for if we have the ip and port correct
		// TODO: Include port in the ping
		// TODO: Could just make a random packet called "hi" or something that can be used as a custom type of ping
	}
}