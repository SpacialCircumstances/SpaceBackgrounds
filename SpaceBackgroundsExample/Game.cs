using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SpaceBackgrounds;

namespace SpaceBackgroundsExample
{
	public class Game
	{
		public Game()
		{
			
		}
		public void Run()
		{
			BackgroundGenerator gen = new BackgroundGenerator();
			gen.Run();
			Texture bg = gen.getTexture();
						
			VideoMode mode = new VideoMode(800, 600, 32);
			RenderWindow window = new RenderWindow(mode, "Space Scene Window", Styles.Close | Styles.Titlebar);
			window.Closed += (object sender, EventArgs e) => window.Close();
			Sprite s = new Sprite(bg);
			s.Position = new Vector2f(0, 0);
			while(window.IsOpen)
			{
				window.DispatchEvents();
				window.Clear();
				window.Draw(s);
				window.Display();
			}
		}
	}
}
