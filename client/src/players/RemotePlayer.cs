using SFML.Graphics;
using SFML.System;

class RemotePlayer : Player
{
	public RemotePlayer(string uuid, string username, uint color)
	{
		Console.WriteLine("Adding new played");
		
		// Assign variables
		Uuid = uuid;
		Username = username;

		// Create the sprite for the player
		Sprite = new RectangleShape(new Vector2f(50, 50));
		Sprite.FillColor = new Color(color);

		Console.WriteLine("Added new remote player with UUID of " + Uuid);
	}

	// Update the players data
	public void UpdateData(string[] packet)
	{
		Position = new Vector2f(float.Parse(packet[3]), float.Parse(packet[4]));
	}
}