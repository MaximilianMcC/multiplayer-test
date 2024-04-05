using System.Net;
using System.Net.Sockets;

class Server
{
	private Logger logger;

	private UdpClient server;
	private IPEndPoint sender;

	public Server(int port)
	{
		// Make the logger
		logger = new Logger("server");

		// Start the server
		server = new UdpClient(port);
		logger.Info("Server started on port " + port);

		// Make an endpoint that picks up any connections
		sender = new IPEndPoint(IPAddress.Any, 0);

		// Actually run the server
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
		// Receive all incoming data and handle it
		// if it contains anything
		byte[] incomingPacket = server.Receive(ref sender);
		// if (incomingPacket.Length > 0) HandlePacket();

		// Get what we gotta sent to the client and
		// send it to them
		

	}
}