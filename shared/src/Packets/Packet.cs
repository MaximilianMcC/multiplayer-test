public class Packet
{    
	protected string PacketString { get; set; } = "";
	protected PacketType PacketType { get; private set; }

	public Packet(PacketType packetType)
	{
		PacketType = packetType;
	}

	// Add data to the packet
	public void AddData(string data)
	{
		PacketString += NetworkManager.packetConcatenator + data;
	}

	// Add the packet to a sending queue so that it can be sent
	// when the next network tick rolls around
	public virtual void AddToSendingQueue()
	{

	}

	public override string ToString()
	{
		return PacketString;
	}
}