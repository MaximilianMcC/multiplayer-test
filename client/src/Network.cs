using System.Net;
using System.Net.Sockets;
using System.Text;
using SFML.Graphics;
using SFML.System;

class Network
{
	public static UdpClient Client;
	public static IPEndPoint Server;


	// Connect to the server
	public static void ConnectToServer(string serverIp, string serverPort)
	{
		// Create a new client
		Client = new UdpClient();

		// Parse the port and IP, then connect to the server
		IPAddress ip = IPAddress.Parse(serverIp);
		int port = int.Parse(serverPort);
		Server = new IPEndPoint(ip, port);
	}

	// Disconnect from the server
	public static void DisconnectFromServer()
	{
		SendToServer("disconnect");
	}



	// Connect to the server
	// Returns the players UUID
	public static string RegisterPlayerOnServer()
	{
		// Send some random garbage to the server asking for a UUID
		SendToServer("kys!! (adding me NOW)");

		// Get the response back with the players UUID
		string uuid = ReceiveFromServer();
		Console.WriteLine("Got UUID " + uuid);
		return uuid;
	}



	// Update the players info on the server
	// TODO: Run this every 2nd frame or something to avoid accidentally ddos the server
	public static void UpdateLocalPlayerPacket(string uuid, Vector2f position, Color color)
	{
		// Get all of the player's info and put it into a "packet" string
		string packet = $"{uuid},{position.X},{position.Y},{color.ToInteger()}";
		SendToServer(packet);
	}

	// Update the remote players with the info from the server
	public static string UpdateRemotePlayer(string uuid)
	{
		// Get the server info
		string packet = ReceiveFromServer();
		if (packet == null) return null;

		// Loop over every player in the server
		foreach (string playerPacket in packet.Split('+'))
		{
			// Get the current players UUID
			string currentUuid = playerPacket.Split(',')[0];
			if (currentUuid == uuid)
			{
				// Return the players info
				return playerPacket;
			}
		}

		return null;
	}


	

	// Check for if new players join the game
	public static void DetectNewPlayers()
	{
		// Get the server info
		// TODO: Don't call this more than once per frame
		string packet = ReceiveFromServer();
		if (packet == null) return;

		// Loop over every player in the server
		foreach (string playerPacket in packet.Split('+'))
		{
			// Get the current players UUID
			string uuid = playerPacket.Split(',')[0];

			// Check for if the player is already in the player list
			bool inPlayerList = Game.players.Any(player => player.Uuid == uuid);
			if (inPlayerList == false)
			{
				// Add the player to the player list
				Game.players.Add(new RemotePlayer(playerPacket));
			}
		}
	}








	// Get decoded data from the server
	// TODO: try/catch
	private static string ReceiveFromServer()
	{
		// Get the message from the server
		byte[] data = Client.Receive(ref Server);

		// Decode the message to a string
		string message = Encoding.ASCII.GetString(data).TrimEnd('+');
		return message;
	}

	// Send data to the server
	// TODO: try/catch
	private static void SendToServer(string message)
	{
		// Encode the message to bytes for sending
		byte[] data = Encoding.ASCII.GetBytes(message);

		// Send the message to the server
		Client.Send(data, data.Length, Server);
	}
}