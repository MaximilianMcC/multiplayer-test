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



		// Send them back a packet with their new UUID
		ConnectionPacket connectionPacket = new ConnectionPacket();
		connectionPacket.Send(client);
	}



















	// Send an acknowledgement packet
	// Where a response must be sent by the client. If one isn't sent then
	// the packet will be retransmitted until received successfully


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