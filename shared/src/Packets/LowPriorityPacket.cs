namespace Shared
{
	public class LowPriorityPacket : Packet
	{
		public LowPriorityPacket(PacketType packetType) : base(packetType)
		{
			// Add on the low-priority marker (lp)
			PacketString += "|LP|";
		}

		// Add the packet to a sending queue so that it can be sent
		// when the next network tick rolls around
		public override void AddToSendingQueue()
		{
			NetworkManager.LowPriorityQueue.Add(this);
		}
	}
}