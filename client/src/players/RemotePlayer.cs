using SFML.Graphics;
using SFML.System;

class RemotePlayer : Player
{
	public RemotePlayer(string packet)
	{
		// Setup the player with the received packet stuff
		string[] data = packet.Split(',');

		Uuid = data[0];
		Position = new Vector2f(float.Parse(data[1]), float.Parse(data[2]));
		Color = new Color(uint.Parse(data[3]));

		Sprite = new RectangleShape();
		Sprite.FillColor = Color;
		Console.WriteLine(Color.ToString());

		Console.WriteLine("Added new remote player with UUID of " + Uuid);
	}

    public override void Update()
    {
        base.Update();

		// // Get the players data
		// // TODO: Don't do this here. Do it in the Network class so only a single request needs to be made to the server
		// string packet = Network.UpdateRemotePlayer(Uuid);
		// string[] data = packet.Split(',');

		// // Update all of the players info
		// Position = new Vector2f(float.Parse(data[1]), float.Parse(data[2]));
		// Color = new Color(uint.Parse(data[3]));
    }
}