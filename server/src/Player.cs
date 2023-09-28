using System.Net;
using System.Net.Sockets;
using System.Text;

class Player
{
	// Player values
	public string Uuid { get; private set; }
	public string Username { get; private set; }
	public uint Color { get; set; }
	public float PositionX { get; set; }
	public float PositionY { get; set; }

	private IPEndPoint client;

	// Create a new player
	public Player(IPEndPoint client, uint color, string username)
	{
		this.client = client;
		Uuid = Guid.NewGuid().ToString();
		Logger.Log($"New player joined the game. Assigned UUID {Uuid}");

		// Assign the starting values
		Color = color;
		Username = username;
	}



	// Handle the player
	public void Handle()
	{
		Logger.Log($"Created handle thread for {Uuid}");

		// TODO: Try/catch
		while (true)
		{
			// Get the incoming request from the player
			byte[] receivedPacketBytes = Server.UdpServer.Receive(ref client);
			string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
			PacketType packetType = (PacketType)byte.Parse(receivedPacket.Split(',')[0]);
			Logger.LogPacket(receivedPacket, Logger.PacketLogType.INCOMING);
		
			// Check for what they want to do
			if (packetType == PacketType.PLAYER_UPDATE)
			{
				// Parse the packet to get the info then update it
				string[] packetData = receivedPacket.Split(',');
				PositionX = float.Parse(packetData[3]);
				PositionY = float.Parse(packetData[4]);


				// Loop through all players and add their data to a sending string
				string outgoingPacket = "";
				foreach (Player player in Server.PlayerList.Values)
				{
					// Check for if the player is ourselves and exclude it
					if (player == this) continue;

					// Add all of the players needed info to the packet
					outgoingPacket += $"{player.Uuid},{player.PositionX},{player.PositionY}+";
				}

				// Send the update packet to the player
				byte[] outgoingPacketBytes = Encoding.ASCII.GetBytes(outgoingPacket);
				Server.UdpServer.Send(outgoingPacketBytes, outgoingPacketBytes.Length, client);
				Logger.LogPacket(outgoingPacket, Logger.PacketLogType.OUTGOING);
			}
			else if (packetType == PacketType.DISCONNECT)
			{
				// Leave the server/game
				Server.PlayerList.Remove(client);
				Logger.Log($"{Uuid} Disconnected from the server");
			}
		}
	}
}