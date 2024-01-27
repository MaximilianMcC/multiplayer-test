using System.Diagnostics;

class Packet
{
	public Handshake? Handshake;

	public Packet(byte[] data)
	{
		// Check for if its a handshake packet or not
		if (data[0] >= 1)
		{
			// Get the sequence number
			int sequenceNumber = data[1];
			Handshake = new Handshake(sequenceNumber);
		}
		else Handshake = null;
	}
}

struct Handshake
{
	public byte SequenceNumber;
	public int Timeouts;
	public DateTime lastTransmissionTime;

	public Handshake(byte sequenceNumber)
	{
		SequenceNumber = sequenceNumber;
	}
}