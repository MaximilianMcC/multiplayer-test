using SFML.Graphics;
using SFML.System;

class Player
{
	public string Uuid { get; protected set; } = null;
	public string Username { get; protected set; } = null;
	public Vector2f Position { get; set; } = new Vector2f(0, 0);
	public Sprite Sprite { get; protected set; }
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



	// Create the final sprite from the color, and username
	protected void GenerateSprite(Color color, string username)
	{
		// Create the nametag for the player
		uint nametagHoverAmount = 10;
		Font font = new Font("./assets/font/mini_pixel-7.ttf");
		Text nametag = new Text(username, font, 24);

		// Create the sprite for the player
		RectangleShape sprite = new RectangleShape(new Vector2f(50, 50));
		sprite.FillColor = new Color(color);
		sprite.Position = new Vector2f(0, nametag.GetGlobalBounds().Height + nametagHoverAmount);



		// Combine both the sprite and nametag into the same image for rendering
		float maxWidth = sprite.GetLocalBounds().Width > nametag.GetGlobalBounds().Width ? sprite.GetLocalBounds().Width : nametag.GetGlobalBounds().Width;
		RenderTexture spriteTexture = new RenderTexture(((uint)maxWidth), (uint)(sprite.GetLocalBounds().Height + nametag.GetGlobalBounds().Height + nametagHoverAmount));

		// Add the nametag and sprite
		spriteTexture.Draw(nametag);
		spriteTexture.Draw(sprite);



		// Create, and set the final sprite for the player
		spriteTexture.Display();
		Sprite = new Sprite(spriteTexture.Texture);
	}
}