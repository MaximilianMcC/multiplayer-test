using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	private static UdpClient server;

	// Keep track of players using their UUID
	private static Dictionary<IPEndPoint, Player> connections = new Dictionary<IPEndPoint, Player>();


	public static void Main(string[] args)
	{
		// Title crap
		Console.Title = "UDP Server";
		Console.CursorVisible = false;
		Console.WriteLine($"Server is listening on port {args[0]}...\n");

		// Parse the port from the args and create the UDP server
		int port = int.Parse(args[0].Trim());
		server = new UdpClient(port);

		// TODO: Make multi-threaded
		while (true)
		{
			try
			{
				// Get the client
				IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
				string packet;


				// Get the data they're sending and decode it
				byte[] incomingPacket = server.Receive(ref client);
				packet = Encoding.ASCII.GetString(incomingPacket);

				// Debug print stuff
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("[INCOMING]");
				Console.ResetColor();
				Console.WriteLine("\t" + packet);

				// Check for if the current client is new/not in player list
				if (connections.ContainsKey(client) == false)
				{
					// Make a new UUID to keep track of the player
					string uuid = Guid.NewGuid().ToString();

					// Create a new player and add them to the list
					Player player = new Player(client, uuid);
					connections.Add(client, player);

					// Send the UUID back to the player
					// TODO: Put this in a send method
					byte[] response = Encoding.ASCII.GetBytes(uuid);
					server.Send(response, response.Length, client);

					Console.WriteLine("New player joined with UUID of " + connections[client].Uuid);

					// End because this packet hasn't got any real data
					continue;
				}

				// Get the current player
				Player currentPlayer = connections[client];

				// Check for if the current player wants to disconnect
				if (packet == "disconnect")
				{
					// Remove the client from the list of clients
					connections.Remove(client);
					Console.WriteLine($"{currentPlayer.Uuid} disconnected");
					continue;
				}

				// Update the player's info
				currentPlayer.Update(packet);

				// Add every players info into the packet for sending (including this current player)
				packet = "";
				foreach (Player player in connections.Values)
				{
					packet += $"{player.Uuid},{player.XPosition},{player.YPosition},{player.Color}+";
				}

				// Debug print stuff
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.Write("[OUTGOING]");
				Console.ResetColor();
				Console.WriteLine("\t" + packet);

				// Encode the outgoing packet to bytes for sending then send it
				byte[] outgoingPacket = Encoding.ASCII.GetBytes(packet);
				server.Send(outgoingPacket, outgoingPacket.Length, client);




			}
			catch (Exception error)
			{
				Console.WriteLine("Exception!!!");
				Console.WriteLine(error);
			}



			// Check for if any players disconnect
			/*
			Console.WriteLine(connections.Count);
			foreach (IPEndPoint client in connections.Keys)
			{
				// Get the current player
				Player currentPlayer = connections[client];
				TimeSpan timeSinceLastPing = DateTime.Now - currentPlayer.LastPing;
				Console.WriteLine($"Checking for if player {currentPlayer.Uuid} has disconnected. ({timeSinceLastPing} since last ping)");

				// Check for if it's been more than 10 seconds since the player last
				// send a packet and disconnect them (10 seconds because average lag is shorter)
				// TODO: Make ping play a factor in how long the disconnect timeout is
				if (timeSinceLastPing.TotalSeconds >= 10)
				{
					// Disconnect the player from the server
					connections.Remove(client);
					Console.WriteLine("Player disconnected!");
				}
			}
			*/

		}
	}
}







// TODO: Put in another file
class Player
{
	// Server and identity stuff
	public IPEndPoint Ip { get; private set; }
	public string Uuid { get; private set; }
	public DateTime LastPing { get; set; }

	// Player properties
	public float XPosition { get; set; }
	public float YPosition { get; set; }
	public uint Color { get; set; }



	// Create a new player
	public Player(IPEndPoint ip, string uuid)
	{
		// Assign the player IP and UUID so we know who controls them
		Ip = ip;
		Uuid = uuid;
	}

	// Update the player info
	public void Update(string packet)
	{
		// Parse the packet to get the data
		string[] data = packet.Split(",");
		
		// Update all of the values
		XPosition = float.Parse(data[1]);
		YPosition = float.Parse(data[2]);
		Color = uint.Parse(data[3]);

		// Update the last time that we received a packet so we can
		// detect for if they have disconnected
		LastPing = DateTime.Now;
	}
}