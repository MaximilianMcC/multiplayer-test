using System.Net;
using System.Net.Sockets;

namespace Shared
{
	public static class NetworkManager
	{
		public const string packetConcatenator = "|+|"; // When a new part of a packet is being added
		public const string packetSeparator = "|&|"; // When a new packet is being added



		// Sending queues for high and low priority packets
		public static List<HighPriorityPacket> HighPriorityQueue = new List<HighPriorityPacket>();
		public static List<LowPriorityPacket> LowPriorityQueue = new List<LowPriorityPacket>();



		// TODO: Maybe rename to SendHpPackets
		public static void SendHighPriorityPacket()
		{
			// Combine all packets into a single one for sending
		}

		// TODO: Maybe rename to ReceiveHpPackets
		public static void ReceiveHighPriorityPackets()
		{
			
		}







		// TODO: Maybe rename to SendLpPackets
		public static void SendLowPriorityPacket()
		{
			// Combine all packets into a single one for sending
		}

		// TODO: Maybe rename to ReceiveLpPackets
		public static void ReceiveLowPriorityPackets()
		{
			
		}
	}
}