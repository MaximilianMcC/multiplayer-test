class LowPriorityPacket : Packet
{
	public LowPriorityPacket()
	{
		// Add on the low-priority marker (lp)
		packetString += "|LP|";
	}
}