using System.Diagnostics;

class Packet
{
	public HandshakeInfo? Handshake;

	public Packet(byte[] data)
	{
		// Handle the packet header
		if (data[0] >= 1)
		{
			// Get the handshake stage
			HandshakeStage stage = (HandshakeStage)data[1];

			// Get the handshake guid to identify the packet
			//? 16 bytes
			string guid = string.Join("", data[3..19]);

			Debug.WriteLine(guid);
		}
		else Handshake = null;
	}





	public override string ToString()
	{
		return "packet!!!";
	}
}

class HandshakeInfo
{
	
}

// TODO: Use full names
enum HandshakeStage
{
	SYN = 0,
	SYN_ACK = 1,
	ACK = 2
}