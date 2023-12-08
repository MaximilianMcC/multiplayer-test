class Game
{
	public static void Run()
	{
		// I am not going to be focusing on graphics or anything here. This is just 
		// so that I can learn how to do the networking aspect. Later on graphics
		// will be added, but only when they are required and a part of the networking

		Start();
		while (true)
		{
			Update();
		}
	}

	private static void Start()
	{
		// Connect to the server
		// TODO: Make a Network.Init() method or something
		Networking.ConnectToServer(Program.LaunchArgs.Ip, Program.LaunchArgs.Port);
	}


	private static void Update()
	{
		const int networkTicksPerSecond = 20;
		const int sleepTime = 1000 / networkTicksPerSecond;

		// Receive network stuff
		NetworkManager.ReceiveHighPriorityPacket();
		NetworkManager.ReceiveLowPriorityPacket();
	
		// TODO: Update game 
		Console.WriteLine("d");

		// Send network stuff
		NetworkManager.SendHighPriorityPacket();
		NetworkManager.SendLowPriorityPacket();


		// Sleep the thread to emulate waiting for ticks
		// TODO: If add gui then don't sleep
		Thread.Sleep(sleepTime);
	}
}