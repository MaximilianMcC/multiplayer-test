using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Game
{
	// SFML stuff
	public static RenderWindow Window;
	public static float DeltaTime;

	// Game objects

	public void Run(string ip, string port)
	{
		// Create the SFML window
		Window = new RenderWindow(new VideoMode(800, 600), "SFML UDP multiplayer client test");
		Window.SetFramerateLimit(60);
		Window.Closed += (sender, e) => Window.Close();
		Clock deltaTimeClock = new Clock();

		// Main game loop
		while (Window.IsOpen)
		{
			// Handle events
			Window.DispatchEvents();
			DeltaTime = deltaTimeClock.Restart().AsSeconds();

			// Update everything
			Update();

			// Draw everything
			Window.Clear(Color.Magenta);
			Render();
			Window.Display();
		}
	}


	private void Update()
	{

	}


	private void Render()
	{

	}
}