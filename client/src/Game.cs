using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Game
{
	// SFML stuff
	public static RenderWindow Window;
	public static float DeltaTime;

	// Game objects
	public static List<Player> players;

	public void Run(string ip, string port)
	{
		// Create the SFML window
		Window = new RenderWindow(new VideoMode(800, 600), "SFML UDP multiplayer client test");
		Window.SetFramerateLimit(60);
		Window.Closed += (sender, e) => Window.Close();
		Clock deltaTimeClock = new Clock();



		// Connect to the server
		Network.ConnectToServer(ip, port);



		// Make the list of players in the game
		// and add a local player
		players = new List<Player>();
		players.Add(new LocalPlayer());



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
}