using System.Diagnostics;

class Packet
{
	public HandshakeInfo? Handshake;

	public Packet(byte[] data)
	{
		// Handle the packet header
		if (data[0] >= 1)
		{
			// Get the sequence number
			int sequenceNumber = data[1];
			Handshake = new HandshakeInfo(sequenceNumber);
		}
		else Handshake = null;
	}
}

struct HandshakeInfo
{
	public int SequenceNumber;

	public HandshakeInfo(int sequenceNumber)
	{
		SequenceNumber = sequenceNumber;
	}
}