using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Game
{
	// SFML stuff
	public static RenderWindow Window;
	public static float DeltaTime;

	// Game objects
	public static List<Player> players = new List<Player>();

	// TODO: Don't include player username in here
	public void Run(string ip, string port, string username)
	{
		// Create the SFML window
		Window = new RenderWindow(new VideoMode(800, 600), "gaem");
		Window.SetFramerateLimit(60);
		Window.Closed += (sender, e) => Exit();
		Clock deltaTimeClock = new Clock();

		// Connect to the server
		Network.ConnectToServer(ip, port);

		// Create a local player for the current user to control
		players.Add(new LocalPlayer(username));



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

	
	// Update the game logic for everything
	private void Update()
	{
		// Update the players
		foreach (Player player in players)
		{
			player.Update();
		}
	}

	// Draw everything
	private void Render()
	{
		// Render the players
		foreach (Player player in players)
		{
			player.Render();
		}
	}


	// Safely exit the game
	private void Exit()
	{
		// Close the window
		Window.Close();
	}
}