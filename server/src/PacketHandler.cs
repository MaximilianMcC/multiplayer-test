using System.Net;
using System.Text;






class PacketHandler
{
	// Event queues
	// TODO: Play around with these values. idk the gpt gave them
	public const uint retransmissionTimeout = 350; //? Milliseconds 
	public const uint maxRetransmissions = 15;
	public static List<RetransmissionPacket> RetransmissionPacketQueue = new List<RetransmissionPacket>();


	
	// Add a new player to the game
	public static void ConnectPlayer(string[] packet, IPEndPoint client)
	{
		// Use the information received to create a new player
		string username = packet[1];
		uint color = uint.Parse(packet[2]);
		Player player = new Player(username, color);



		// Send them back a packet with their new UUID
		ConnectionPacket connectionPacket = new ConnectionPacket();
		connectionPacket.Send();
	}

}