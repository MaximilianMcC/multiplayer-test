using System.Net;
using System.Text;






class PacketHandler
{
	
	// Add a new player to the game
	public static void ConnectPlayer(string[] packet, IPEndPoint client)
	{
		// Use the information received to create a new player
		string username = packet[1];
		uint color = uint.Parse(packet[2]);
		Player player = new Player(username, color);

		// Get the players UUID and send it back to them
		string connectionPacket = $"{(int)PacketType.CONNECT_RESPONSE},{player.Uuid}";
		SendAcknowledgementPacket(connectionPacket, client);
	}

















	// Send a packet
	public static void SendPacket(string packet, IPEndPoint client)
	{
		// Encode, then send the packet
		byte[] packetBytes = Encoding.ASCII.GetBytes(packet);
		Server.UdpServer.Send(packetBytes, packetBytes.Length, client);

		// Log it
		Logger.LogPacket(packet, PacketLogType.OUTGOING, client.ToString());
	}

	// Send an acknowledgement packet
	// Where a response must be sent by the client. If one isn't sent then
	// the packet will be retransmitted until received successfully
	public static void SendAcknowledgementPacket(string packet, IPEndPoint client)
	{
		// Add a guid to the end of the packet to identify it when it comes back
		string guid = new Guid().ToString();

		// Make a new re-transmissible packet, then send it
		RetransmissionPacket acknowledgementPacket = new RetransmissionPacket(packet, guid, client);
		Server.AcknowledgementPacketQueue.Add(acknowledgementPacket);
		SendPacket(packet, client);
	}

}




// TODO: Put in another file!!
struct RetransmissionPacket
{
	public IPEndPoint Client { get; private set; }

	public string Content { get; private set; }
	public string Guid { get; private set; } //? pronounced "goo ID" btw

	public uint TimesSent { get; set; }
	public DateTime SendTime { get; set; }

	public RetransmissionPacket(string packet, string guid, IPEndPoint client)
	{
		//! SendTime might be a bit delayed by a few ms
		SendTime = DateTime.Now;
		Client = client;
		Content = packet += $",{guid}";
		Guid = guid;
	}
}