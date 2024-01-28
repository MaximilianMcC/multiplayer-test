using System.Diagnostics;

class Packet
{
	public Handshake? Handshake;

	public Packet(byte[] data)
	{
		// Check for if its a handshake packet or not
		if (data[0] >= 1)
		{
			// Make the handshake info
			Handshake = new Handshake(data[1]);
		}
		else Handshake = null;
	}
}

struct Handshake
{
	public byte SequenceNumber;
	public int Timeouts;
	public DateTime LastTransmissionTime;

	public Handshake(byte sequenceNumber)
	{
		SequenceNumber = sequenceNumber;
	}
}