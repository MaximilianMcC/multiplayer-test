using Raylib_cs;

class Game
{
	private static Texture2D freeman;

	public static void Run()
	{
		// Setup raylib
		Raylib.InitWindow(800, 800, "holey smokes");
		Raylib.SetTargetFPS(60);

		Start();
		while (!Raylib.WindowShouldClose())
		{
			Update();
			Render();
		}
		Raylib.UnloadTexture(freeman);
		Raylib.CloseWindow();
	}

	private static void Start()
	{
		freeman = Raylib.LoadTexture("./assets/freeman.png");
	}

	private static void Update()
	{

	}

	private static void Render()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(new Color(0, 0, 255, 255));

		Raylib.DrawTexture(freeman, 0, 0, Color.White);

		Raylib.EndDrawing();
	}
}