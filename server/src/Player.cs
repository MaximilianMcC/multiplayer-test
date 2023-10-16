using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Player
{
	// Const player values
	public string Uuid { get; private set; }
	public string Username { get; private set; }
	public uint Color { get; private set; }

	// Dynamic player values
	public float PositionX { get; set; }
	public float PositionY { get; set; }

	// Server/Networking player values
	public bool Disconnected { get; set; }

	// Create a new player
	public Player(string username, uint color)
	{
		Uuid = Guid.NewGuid().ToString();
		Logger.Log($"New player joined the game! Assigned UUID {Uuid}");

		// Assign the starting values that cant be changed
		Color = color;
		Username = username;
	}

	
	// Update the players data
	public void Update(string packet)
	{
		string[] packetData = packet.Split(',');

		PositionX = float.Parse(packetData[2]);
		PositionY = float.Parse(packetData[3]);
	}




    public override string ToString()
    {
        return $"{Uuid}/{Username}";
    }
}