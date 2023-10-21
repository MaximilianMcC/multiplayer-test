
// TODO: put the sending methods in here so you can do like new Packet().Send();

public enum PacketType
{
	CONNECT = 1,
	CONNECT_RESPONSE = 2,
	DISCONNECT = 3,

	PLAYER_UPDATE = 4,
}

public enum TransmissionType
{
	EXPENDABLE = 0,
	SYNCHRONIZATION = 1,
	ACKNOWLEDGEMENT = 2
}


// Standard packet
class Packet
{
	// Mandatory packet stuff
	public PacketType Type { get; protected set; }
	public TransmissionType TransmissionType { get; protected set; }
}

// Connection packet
class ConnectionPacket : Packet
{
	public ConnectionPacket()
	{
		// Assign the mandatory stuff
		Type = PacketType.CONNECT;
		TransmissionType = TransmissionType.SYNCHRONIZATION;
	}
}
