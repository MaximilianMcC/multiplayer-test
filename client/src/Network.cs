using System.Net;
using System.Net.Sockets;
using System.Text;
using SFML.Graphics;
using SFML.System;

class Network
{
	private static UdpClient client;
	private static IPEndPoint server;


	// Connect to the server and create a client
	public static void ConnectToServer(string serverIp, string serverPort)
	{
		// Create a client, and connect to the server
		client = new UdpClient();
		server = new IPEndPoint(IPAddress.Parse(serverIp), int.Parse(serverPort));
		Console.WriteLine("Created client and connected to the server");
	}


	// Register the player on the server
	// Returns the players UUID
	public static string RegisterPlayer(Color color, string username)
	{
		// Send a connection packet
		string connectionPacket = $"0,{color.ToInteger()},{username}";
		byte[] connectionPacketBytes = Encoding.ASCII.GetBytes(connectionPacket);
		client.Send(connectionPacketBytes, connectionPacketBytes.Length, server);
		Console.WriteLine("Sent connection packet");

		// Get the response back with the players new UUID
		byte[] receivedPacketBytes = client.Receive(ref server);
		string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
		string uuid = receivedPacket.Split(',')[0];

		// Give back the UUID for the player to use
		Console.WriteLine($"Received UUID {uuid} from server🤩🥳🥳");
		return uuid;
	}

	

	// Send the players data to the server, and get back the data of the remote players
	public static void UpdatePlayers()
	{
		// Create a packet with all of the local players info
		string outgoingPacket = $"2,{Game.LocalPlayer.Uuid},{Game.LocalPlayer.Username},{Game.LocalPlayer.Position.X},{Game.LocalPlayer.Position.Y}";

		// Send the local player packet to the server
		byte[] outgoingPacketBytes = Encoding.ASCII.GetBytes(outgoingPacket);
		client.Send(outgoingPacketBytes, outgoingPacketBytes.Length, server);





		// Get the response from the server, that contains the data of all the other remote players
		byte[] incomingPacketBytes = client.Receive(ref server);
		string incomingPacket = Encoding.ASCII.GetString(incomingPacketBytes);

		// Parse the incoming packet to get each individual player
		string[] playerList = incomingPacket.Split('+');
		foreach (string playerPacket in playerList)
		{
			// Extract the player data from the packet string
			string[] playerData = playerPacket.Split(',');

			// Get the players UUID to find the matching remote player,
			// or if they don't exist, create a new remote player
			string uuid = playerData[0];
			RemotePlayer remotePlayer = Game.RemotePlayers.FirstOrDefault(player => player.Uuid == uuid);

			// Create a new player if they don't exist
			if (remotePlayer == null)
			{
				// Create the new remote player
				RemotePlayer player = new RemotePlayer(uuid, playerData[1], uint.Parse(playerData[2]));
			}
		
			// Update the existing players info
			remotePlayer.Position = new Vector2f(float.Parse(playerData[3]), float.Parse(playerData[4]));
		}
	}

}