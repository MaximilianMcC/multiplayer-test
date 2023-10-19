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
	}

	// Disconnect from the server
	public static void DisconnectFromServer()
	{
		// Send a disconnection packet
		string disconnectionPacket = $"3,{Game.LocalPlayer.Uuid}";
		byte[] disconnectionPacketBytes = Encoding.ASCII.GetBytes(disconnectionPacket);
		client.Send(disconnectionPacketBytes, disconnectionPacketBytes.Length, server);

		// Close the UDP client
		client.Close();
	}


	// Register the player on the server
	// Returns the players UUID
	public static string RegisterPlayer(Color color, string username)
	{
		// Send a connection packet
		string connectionPacket = $"1,{username},{color.ToInteger()}";
		byte[] connectionPacketBytes = Encoding.ASCII.GetBytes(connectionPacket);
		client.Send(connectionPacketBytes, connectionPacketBytes.Length, server);

		// Get the response back with the players new UUID
		byte[] receivedPacketBytes = client.Receive(ref server);
		string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
		string uuid = receivedPacket.Split(',')[1];

		// Give back the UUID for the player to use
		Logger.Log($"Received UUID {uuid} from server");
		return uuid;
	}



	public static void UpdatePlayers()
	{
		UpdateLocalPlayer();
		UpdateRemotePlayers();
	}


	// Update the local players position on the server
	private static void UpdateLocalPlayer()
	{
		// Create a packet with all of the local players info
		string outgoingPacket = $"4,{Game.LocalPlayer.Uuid},{Game.LocalPlayer.Position.X},{Game.LocalPlayer.Position.Y}";

		// Send the local player packet to the server
		byte[] outgoingPacketBytes = Encoding.ASCII.GetBytes(outgoingPacket);
		client.Send(outgoingPacketBytes, outgoingPacketBytes.Length, server);
	}

	// Update all of the local players positions from the server
	private static void UpdateRemotePlayers()
	{
		// Get a response from the server that contains the data of all the other remote players
		byte[] incomingPacketBytes = client.Receive(ref server);
		string incomingPacket = Encoding.ASCII.GetString(incomingPacketBytes);
		
		// Exit early if nothing is being sent (no one else online)
		if (incomingPacket.Length <= 0) return;

		// Parse all the packets
		string[] remotePackets = incomingPacket.Split('+');
		foreach (string remotePlayerPacket in remotePackets)
		{
			string[] remotePlayerData = remotePlayerPacket.Split(',');

			// Get the player from their UUID
			string uuid = remotePlayerData[1];
			RemotePlayer player = Game.RemotePlayers.FirstOrDefault(player => player.Uuid == uuid);

			// Check for if the player is leaving
			if (remotePlayerData[0] == "3") player.Disconnect();

			// Create a new player if a player wasn't returned before
			if (player == null)
			{
				// Get the players static info and create a new player with that
				// Also add them to the player list
				RemotePlayer newPlayer = new RemotePlayer(uuid, remotePlayerData[1], uint.Parse(remotePlayerData[2]));
				Game.RemotePlayers.Add(newPlayer);

				// Set the new player to the player so we can keep working with them
				player = newPlayer;
			}

			// Get the players data, and update it
			player.UpdateData(remotePlayerData);
		}
	}
}