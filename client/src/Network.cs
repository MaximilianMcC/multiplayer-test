using System.Net;
using System.Net.Sockets;
using System.Text;
using SFML.Graphics;

class Network
{
	private static UdpClient client;
	private static IPEndPoint server;


	// Connect to the server and create a client
	public static void ConnectToServer(string serverIp, string serverPort)
	{
		// Create a client, and connect to the server
		client = new UdpClient();
		server = new IPEndPoint(IPAddress.Parse(serverIp), int.Parse(serverPort));
		Console.WriteLine("Created client and connected to the server");
	}


	// Register the player on the server
	// Returns the players UUID
	public static string RegisterPlayer(Color color, string username)
	{
		// Send a connection packet
		string connectionPacket = $"0,{color.ToInteger()},{username}";
		byte[] connectionPacketBytes = Encoding.ASCII.GetBytes(connectionPacket);
		client.Send(connectionPacketBytes, connectionPacketBytes.Length, server);
		Console.WriteLine("Sent connection packet");

		// Get the response back with the players new UUID
		byte[] receivedPacketBytes = client.Receive(ref server);
		string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
		string uuid = receivedPacket.Split(',')[0];

		// Give back the UUID for the player to use
		Console.WriteLine($"Received UUID {uuid} from server🤩🥳🥳");
		return uuid;
	}

}