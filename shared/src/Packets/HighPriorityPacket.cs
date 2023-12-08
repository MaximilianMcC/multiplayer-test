class HighPriorityPacket : Packet
{
	public string uuid { get; private set; }

	public HighPriorityPacket()
	{
		// Generate a UUID to identify the packet
		// TODO: Use the whole UUID. Only using the timestamp right now
		// TODO: Could use timestamp to determine ping. It wouldn't be very accurate, but we're already sending it so we may as well use it anyways
		uuid = Guid.NewGuid().ToString().Split('-')[0];

		// Add on the high-priority marker (hp)
		packetString += "|HP|";
	}
}