using System.Net;

class Networking
{
	private static List<Handshake> packetsToAcknowledge = new List<Handshake>();

	// Timeout stuff
	//? All time values in seconds
	private const float TIMEOUT = 1f; // TODO: Could drop down to 0.5s or something
	private const int MAX_TIMEOUTS = 8;


	// Mark a packet for acknowledgement
	public static void AcknowledgeHandshake(Handshake handshake)
	{
		// Add the sequence number to the list of sequence
		// number to acknowledge so it can be transmitted if needed
		packetsToAcknowledge.Add(handshake);
	}


	// Loop through all handshake packets and send out a retransmission
	// packet if it has exceeded the timeout
	public static void AcknowledgeHandshakePackets(IPEndPoint client)
	{
		foreach (Handshake handshake in packetsToAcknowledge)
		{
			// Check for if the packet has had too many timeouts
			if (handshake.Timeouts > MAX_TIMEOUTS)
			{
				// Remove the packet from the list of packets
				packetsToAcknowledge.Remove(handshake);

				Console.WriteLine($"handshake (sequence number {handshake.SequenceNumber}) took too long to respond (timed out)");
				return;
			}

			// Check for if the packet is due for a retransmission
			// then send a new one
			DateTime currentTime = DateTime.Now;
			if ((currentTime - handshake.lastTransmissionTime).TotalSeconds > TIMEOUT)
			{
				byte[] acknowledgementPacket = new byte[] { handshake.SequenceNumber };
				Program.Server.Send(acknowledgementPacket, acknowledgementPacket.Length, client);
			}
		}
	}
}