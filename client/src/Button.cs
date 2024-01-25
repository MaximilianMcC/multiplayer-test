using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

class Button
{
	public string Label { get; set; }
	public Rectangle Shape { get; set; }
	public bool BeingHoveredOver;

	private const float fontSize = 30;
	private const float fontSpacing = fontSize / 10;
	private const float padding = 15;
	private const float padding2 = padding * 2;
	private const float paddingH = padding / 2;
	private const float decorationSize = 3f;

	private readonly Color backgroundColor = new Color(70, 75, 50, 255);
	private readonly Color highlightColor = new Color(133, 133, 111, 255);
	private readonly Color shadowColor = new Color(60, 60, 40, 255);
	private readonly Color textColor = new Color(185, 185, 170, 255);
	private readonly Color hoverOverlayColor = new Color(0, 0, 0, 64);

	public Button(string label, Vector2 position)
	{
		// Set the label
		Label = label;

		// Measure out the size for the button
		Vector2 textSize = Raylib.MeasureTextEx(Game.Font, Label, fontSize, fontSpacing);
		float width = padding2 + padding2 + textSize.X;
		float height = padding + paddingH + textSize.Y;

		// Set the shape
		Shape = new Rectangle(position.X, position.Y, width, height);
	}

	public void Update()
	{
		// Check for if the user is hovering over the button
		Vector2 mousePosition = Raylib.GetMousePosition();
		bool currentlyBeingHoveredOver = Raylib.CheckCollisionPointRec(mousePosition, Shape);



		// Change the cursor type if needed
		if (currentlyBeingHoveredOver != BeingHoveredOver)
		{
			if (currentlyBeingHoveredOver) Raylib.SetMouseCursor(MouseCursor.PointingHand);
			else Raylib.SetMouseCursor(MouseCursor.Default);
		}

		// Exit early if we're not hovering over the button
		BeingHoveredOver = currentlyBeingHoveredOver;
		if (BeingHoveredOver == false) return;



		// Check for if the user has clicked on the button
		if (Raylib.IsMouseButtonPressed(MouseButton.Left))
		{
			Debug.WriteLine("Clicked button");
		}
	}

	// TODO: Make it look like win95 button, or use black mesa green
	public void Render()
	{
		// Draw the main background
		Raylib.DrawRectangleRec(Shape, backgroundColor);

		// Draw the shadow
		Raylib.DrawRectangle((int)Shape.X, (int)(Shape.Y + Shape.Height), (int)Shape.Width, (int)decorationSize, shadowColor);
		Raylib.DrawRectangle((int)(Shape.X + Shape.Width) - (int)decorationSize, (int)Shape.Y, (int)decorationSize, (int)Shape.Height, shadowColor);

		// Draw the highlight
		Raylib.DrawRectangle((int)Shape.X, (int)Shape.Y, (int)Shape.Width, (int)decorationSize, highlightColor);
		Raylib.DrawRectangle((int)Shape.X, (int)Shape.Y, (int)decorationSize, (int)Shape.Height, highlightColor);

		// Draw the text
		Raylib.DrawTextEx(Game.Font, Label, (Shape.Position + new Vector2(padding2, padding)), fontSize, fontSpacing, textColor);

		// If the button is being hovered over, darken it a little bit
		if (BeingHoveredOver)
		{
			Raylib.DrawRectangleRec(Shape, hoverOverlayColor);
		}
	}
}