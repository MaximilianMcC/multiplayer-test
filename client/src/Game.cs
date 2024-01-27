using System.Numerics;
using Raylib_cs;

class Game
{
	private static Texture2D freeman;
	public static Font Font;
	private static List<Button> buttons;

	public static void Run()
	{
		// Setup raylib
		Raylib.SetTraceLogLevel(TraceLogLevel.Error); //! maybe remove this idk
		Raylib.InitWindow(800, 800, "holey smokes");
		Raylib.SetTargetFPS(60);

		Start();
		while (!Raylib.WindowShouldClose())
		{
			Update();
			Render();
		}
		
		Raylib.UnloadTexture(freeman);
		Raylib.UnloadFont(Font);
		Raylib.CloseWindow();
	}

	private static void Start()
	{
		// Connect to the server
		Networking.ConnectToServer(Program.Args[0]);

		// Load in all the crap
		freeman = Raylib.LoadTexture("./assets/freeman.png");
		Font = Raylib.LoadFont("./assets/trebucbd.ttf");
		Raylib.SetTextureFilter(Font.Texture, TextureFilter.Bilinear);

		// Buttons stuff
		buttons = new List<Button>()
		{
			new Button("Send normal packet", new Vector2(10, 10), (() => Networking.SendNormalPacket("Hello, world!"))),
			new Button("Send handshajke packet", new Vector2(10, 80), (() => Networking.SendHandshakePacket("Hello, world!")))
		};
	}

	private static void Update()
	{
		foreach (Button button in buttons)
		{
			button.Update();
		}
	}

	private static void Render()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(new Color(0, 0, 255, 255));

		Raylib.DrawTexture(freeman, 0, 0, Color.White);
		foreach (Button button in buttons)
		{
			button.Render();
		}

		Raylib.EndDrawing();
	}
}