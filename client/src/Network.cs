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
	public static void UpdatePlayerPacket(string uuid, Vector2f position, Color color)
	{
		// Get all of the player's info and put it into a "packet" string
		string packet = $"{uuid},{position.X},{position.Y},{color.ToInteger()}";
		SendToServer(packet);
	}


	



	// Get decoded data from the server
	private static string ReceiveFromServer()
	{
		// Get the message from the server
		byte[] data = Client.Receive(ref Server);

		// Decode the message to a string
		string message = Encoding.ASCII.GetString(data);
		return message;
	}

	// Send data to the server
	private static void SendToServer(string message)
	{
		// Encode the message to bytes for sending
		byte[] data = Encoding.ASCII.GetBytes(message);

		// Send the message to the server
		Client.Send(data, data.Length, Server);
	}
}