using SFML.Graphics;
using SFML.System;

class RemotePlayer : Player
{
	public RemotePlayer(string uuid, string username, uint color)
	{
		// Assign variables
		Uuid = uuid;
		Username = username;

		// Create the sprite for the player
		Sprite = new RectangleShape(new Vector2f(50, 50));
		Sprite.FillColor = new Color(Color);

		Console.WriteLine("Added new remote player with UUID of " + Uuid);
	}
}