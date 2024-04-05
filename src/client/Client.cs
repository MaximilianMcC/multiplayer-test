using System.Net;
using System.Net.Sockets;

class Client
{
	private Logger logger;

	private UdpClient client;
	private IPEndPoint server;

	public Client(string ip, int port)
	{
		// Make the logger
		logger = new Logger("client");

		// Make the client and get the server
		client = new UdpClient();
		server = new IPEndPoint(IPAddress.Parse(ip), port);

		// Actually run the client
		while (true)
		{
			try
			{
				Tick();
			}
			catch (System.Exception e)
			{
				logger.Error(e);
			}
		}
	}

	//? There is no tick speed, but I'm still calling this tick
	private void Tick()
	{
		// Send new packet thing
		if (Console.KeyAvailable)
		{
			char character = Console.ReadKey().KeyChar;

			Packet packet = new MessagePacket();
			packet.

		}
	}
}