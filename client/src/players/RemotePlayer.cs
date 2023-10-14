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

		// Make a sprite for the player
		GenerateSprite(Color, username);

		Console.WriteLine("Added new remote player with UUID of " + Uuid);
	}

	// Update the players data
	public void UpdateData(string[] packet)
	{
		Position = new Vector2f(float.Parse(packet[3]), float.Parse(packet[4]));
	}

	// Leave the game
	public void Disconnect()
	{
		// Remove the player from the remote players list
		Game.RemotePlayers.Remove(this);
		
		Console.WriteLine("Player has left the game (sobbing)");
	}
}