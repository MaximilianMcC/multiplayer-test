using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Networking
{
	// Client and server stuff
	private static UdpClient client;
	private static IPEndPoint server;

	// Handshake stuff
	// TODO: Somehow decrease the sequenceNumber. Should be fine, but overtime it will add up and become a giant number
	private static int nextAvailableSequenceNumber = 0;





	public static void ConnectToServer(string connectionDetails)
	{
		// Make a udp client for communicating with the server
		client = new UdpClient();

		// Parse the connection details to extract the ip and port
		string[] connection = connectionDetails.Split(':');
		IPAddress ip = IPAddress.Parse(connection[0]);
		int port = int.Parse(connection[1]);

		// Get the server endpoint from the parsed stuff
		server = new IPEndPoint(ip, port);
	}


	public static void SendNormalPacket(string content)
	{
		// Create the packet header
		byte[] packetHeader = new byte[1];
		packetHeader[0] = 0;

		// Encode the content into bytes for sending
		byte[] data = Encoding.ASCII.GetBytes(content);

		// Combine everything to get the final packet, then send it to the client
		byte[] packet = packetHeader.Concat(data).ToArray();

		// Send the packet
		client.Send(packet, packet.Length, server);
	}


	public static void SendHandshakePacket(string content)
	{
		// Get the sequence number used to identify the packet
		int sequenceNumber = nextAvailableSequenceNumber;
		nextAvailableSequenceNumber++;

		// Create the packet header
		// TODO: Allocate 2 bytes or maybe a little more for the sequence number
		byte[] packetHeader = new byte[2];
		packetHeader[0] = 1;
		packetHeader[1] = (byte)sequenceNumber;

		// Encode the content into bytes for sending
		byte[] data = Encoding.ASCII.GetBytes(content);

		// Combine everything to get the final packet, then send it to the client
		// TODO: Listen for a response with the same sequence number
		// TODO: Retransmit the packet if no response within a reasonable time
		byte[] packet = packetHeader.Concat(data).ToArray();
		client.Send(packet, packet.Length, server);
	}
}





// TODO: Use full names
enum HandshakeStage
{
	SENDING_DATA,
	ACKNOWLEDGING_DATA
}