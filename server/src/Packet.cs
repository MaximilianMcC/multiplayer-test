
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
	public IPEndPoint Client { get; private set; }


	public virtual void Send()
	{
		SendPacket("not implemented!!", Client);
	}




	// Send a packet
	// TODO: Put somewhere else
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
		// RetransmissionPacket acknowledgementPacket = new RetransmissionPacket(packet, guid, client);
		// Server.AcknowledgementPacketQueue.Add(acknowledgementPacket);
		// SendPacket(packet, client);
	}
}



class RetransmissionPacket : Packet
{
	public string Guid { get; private set; }
	public uint TimesSent { get; private set; }
	public DateTime LastTimeSent { get; private set; }

	public RetransmissionPacket()
	{
		// Create a new GUID so that the packet can be identified when it comes back
		Guid = new Guid().ToString();

		// Add the packet to the retransmission list
		PacketHandler.RetransmissionPacketQueue.Add(this);
	}

    public override void Send()
    {
        // Increase the values
		TimesSent++;
		LastTimeSent = DateTime.Now;

		// Check for if the packet has been sent too many times
		if (TimesSent > PacketHandler.maxRetransmissions)
		{
			// Remove the packet from the retransmission list thing
			Logger.Log("Packet exceed max transmissions. Ceasing all retransmissions.");
			PacketHandler.RetransmissionPacketQueue.Remove(this);
		}
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
	public override void Send()
	{
		string packet = $"{Type}{TransmissionType}";
		Console.WriteLine(packet);

		SendAcknowledgementPacket(packet, Client);
	}
}
