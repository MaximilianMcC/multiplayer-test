using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
	// Enable fancy UI or not (Might slow down server performance)
	public static bool FancyUi = true;

	// TODO: Don't use IP to identify players. Use UUID then multiple on same pc can join
	public static UdpClient UdpServer;
	public static List<Player> PlayerList = new List<Player>();

	public static void Main(string[] args)
	{
		// UI setup and stuff
		Console.CursorVisible = false;
		Console.Title = "sererv";

		// Get the server port from the launch arguments
		int port = int.Parse(args[0]);

		// Create the UDP server
		UdpServer = new UdpClient(port);
		Logger.Log($"Server listening on port {port}...");


		while (true)
		{
			try
			{
				// Get the currently connecting client
				IPEndPoint currentClient = new IPEndPoint(IPAddress.Any, 0);

				// Get the packet data
				byte[] receivedPacketBytes = UdpServer.Receive(ref currentClient);
				string receivedPacket = Encoding.ASCII.GetString(receivedPacketBytes);
				Logger.LogPacket(receivedPacket, Logger.PacketLogType.INCOMING, "UNKNOWN");

				// Get the packet type and check for what they want to do
				PacketType packetType = (PacketType)int.Parse(receivedPacket.Split(',')[0]);
				if (packetType == PacketType.CONNECT)
				{
					Logger.Log("New player is asking to connect");
					
					// Parse the packet to get the color and username
					string[] packetData = receivedPacket.Split(',');
					uint color = uint.Parse(packetData[1]);
					string username = packetData[2];

					// Create, then add the player to the player list
					Player player = new Player(username, color);
					PlayerList.Add(player);

					// Send back the players new UUID
					// TODO: If Removing debug stuff them remove connectionPacket string because its useless here
					string connectionPacket = player.Uuid;
					byte[] connectionPacketBytes = Encoding.ASCII.GetBytes(connectionPacket);
					UdpServer.Send(connectionPacketBytes, connectionPacketBytes.Length, currentClient);
					Logger.LogPacket(connectionPacket, Logger.PacketLogType.OUTGOING, player.ToString());
				}
				else
				{
					// Get the player object using the sent UUID
					//! If not in then it will be null. do something with that idk!!!
					string uuid = receivedPacket.Split(',')[1];
					Player player = PlayerList.FirstOrDefault(player => player.Uuid == uuid);

					if (packetType == PacketType.PLAYER_UPDATE)
					{
						player.Update(receivedPacket);
					}
					else if (packetType == PacketType.DISCONNECT)
					{
						Logger.Log($"{player} left the game");
					}


					// Send back the data for all players
					// except the current player
					string outgoingPacket = "";
					foreach (Player currentPlayer in PlayerList)
					{
						if (currentPlayer == player) continue;

						// Get all of the players data and add it to the outgoing packet
						outgoingPacket += $"{currentPlayer.Uuid},{currentPlayer.Username},{currentPlayer.Color},{currentPlayer.PositionX},{currentPlayer.PositionY}+";
					}

					// Send it to the player
					byte[] connectionPacketBytes = Encoding.ASCII.GetBytes(outgoingPacket.Trim('+'));
					UdpServer.Send(connectionPacketBytes, connectionPacketBytes.Length, currentClient);
					Logger.LogPacket(outgoingPacket, Logger.PacketLogType.OUTGOING, player.ToString());
				}
			
			}
			catch (Exception e)
			{
				// Print out the error
				Logger.Log("Error while running server!", Logger.LogType.ERROR);
				Logger.Log(e.ToString(), Logger.LogType.ERROR);
			}
		}
	
	}
}

public enum PacketType
{
	CONNECT = 0,
	DISCONNECT = 1,

	PLAYER_UPDATE = 2
}