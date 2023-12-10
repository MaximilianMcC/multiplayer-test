class Player
{
	private string uuid;

	public Player(string username)
	{
		// Register the player to get a UUID
		uuid = Networking.RegisterPlayer(username);
		Console.WriteLine("Received UUID " + uuid);
	}
}