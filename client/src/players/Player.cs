using SFML.Graphics;
using SFML.System;

class Player
{
	public string Uuid { get; protected set; } = null;
	public Vector2f Position { get; protected set; } = new Vector2f(0, 0);
	public RectangleShape Sprite { get; protected set; } = new RectangleShape();
	public Color Color { get; protected set; } = new Color(0x0);

	// Update the player logic
	public virtual void Update()
	{

	}

	// Render the player on screen
	public virtual void Render()
	{
		Sprite.Position = Position;
		Game.Window.Draw(Sprite);
	}
}