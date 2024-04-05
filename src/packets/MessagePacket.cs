class MessagePacket : Packet
{
	public readonly PacketId Id = PacketId.Message;

	public string Sender;
	public string Message;
}