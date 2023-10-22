
// TODO: put the sending methods in here so you can do like new Packet().Send();

using System.Net;
using System.Text;

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






	// Send a packet
	public static void SendPacket(string packet, IPEndPoint client)
	{
		// Encode, then send the packet
		byte[] packetBytes = Encoding.ASCII.GetBytes(packet);
		Server.UdpServer.Send(packetBytes, packetBytes.Length, client);

		// Log it
		Logger.LogPacket(packet, PacketLogType.OUTGOING, client.ToString());
	}

	// Send a synchronization packet that requires a acknowledgement packet to be sent back
	public static void SendAcknowledgementPacket(string packet, IPEndPoint client)
	{
		// Add a guid to the end of the packet to identify it when it comes back
		string guid = new Guid().ToString();

		// Make a new re-transmissible packet, then send it
		RetransmissionPacket acknowledgementPacket = new RetransmissionPacket(packet, guid, client);
		Server.AcknowledgementPacketQueue.Add(acknowledgementPacket);
		SendPacket(packet, client);
	}
}

// Connection packet
class ConnectionPacket : Packet
{
	protected string Uuid { get; private set; }

	public ConnectionPacket()
	{
		// Assign the mandatory stuff
		Type = PacketType.CONNECT;
		TransmissionType = TransmissionType.SYNCHRONIZATION;

		// Make a new UUID for the player to be identified by
		Uuid = Guid.NewGuid().ToString();
	}

	// Send the packet
	public void Send(IPEndPoint client)
	{
		string packet = $"{Type}{TransmissionType}";
		Console.WriteLine(packet);

		SendAcknowledgementPacket(packet, client);
	}
}
