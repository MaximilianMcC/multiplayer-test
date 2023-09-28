class Player
{
	// Player values
	public string Uuid { get; private set; }
	public string Username { get; private set; }
	public uint Color { get; set; }
	public float PositionX { get; set; }
	public float PositionY { get; set; }

	// Create a new player
	public Player(uint color, string username)
	{
		// Assign the starting values
		Uuid = Guid.NewGuid().ToString();
		Color = color;
		Username = username;
	}



	// Handle the player
	public void Handle()
	{
		Logger.Log($"Created handle thread for {Uuid}");
	}
}