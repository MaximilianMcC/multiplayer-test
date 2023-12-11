public enum PacketType
{

	// Packets sent when a client is joining
	CLIENT_CONNECTION_REQUEST = 0,                // Client -> Server
	CLIENT_CONNECTED = 1,                         // Server -> Client

	// Packets sent when a client is leaving
	CLIENT_DISCONNECTING = 2,                     // Client -> Server
	CLIENT_DISCONNECTED = 3                       // Server -> Client

}

public enum HandshakeStage
{
	SYNCHRONIZATION,
	ACKNOWLEDGEMENT
}