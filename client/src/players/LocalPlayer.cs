using SFML.Graphics;
using SFML.System;
using SFML.Window;

class LocalPlayer : Player
{
	private float speed = 350f;

	public LocalPlayer(string username)
	{
		// Generate a random color for the player
		Random random = new Random();
		Color = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255), 255);

		// Make a sprite for the player
		GenerateSprite(Color, username);

		// Get a UUID for the player
		Uuid = Network.RegisterPlayer(Color, username);
		Username = username;
	}

    public override void Update()
    {
        base.Update();

		// Move the player
		Move();
    }



	private void Move()
	{
		// Calculate the movement stuff
		Vector2f newPosition = Position;
		float movement = speed * Game.DeltaTime;

		// Check for if the user has the window selected
		if (Game.Window.HasFocus() == false) return;

		// Get player input and adjust the player movement
		if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) newPosition.X -= movement;
		if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) newPosition.X += movement;
		if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) newPosition.Y -= movement;
		if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) newPosition.Y += movement;

		// Move the player
		Position = newPosition;
	}

}