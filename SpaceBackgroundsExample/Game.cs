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
        Sprite s;
        public void Run()
        {
            Random r = new Random();
            BackgroundGenerator gen = new BackgroundGenerator(r.Next(), r.Next(0, 4), true);
            gen.Run();
            Texture bg = gen.getTexture();
            ContextSettings cs = new ContextSettings();
            cs.AntialiasingLevel = 4;
            VideoMode mode = new VideoMode(800, 600, 32);
            RenderWindow window = new RenderWindow(mode, "Space Scene Window", Styles.Close | Styles.Titlebar, cs);
            window.Closed += (object sender, EventArgs e) => window.Close();
            window.KeyPressed += KeyPress;
            s = new Sprite(bg);
            s.Position = new Vector2f(0, 0);
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw(s);
                window.Display();
            }
        }
        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Return)
            {
                Image img = s.Texture.CopyToImage();
                var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                int i = (int)timeSpan.TotalSeconds;
                img.SaveToFile("../../screenshots/" + Convert.ToString(i) + ".png");
                Console.WriteLine("Screenshot saved!");
            }
            else
            {
                Random r = new Random();
                BackgroundGenerator gen = new BackgroundGenerator(r.Next(), r.Next(0, 4), true);
                gen.Run();
                Texture bg = gen.getTexture();
                s = new Sprite(bg);
            }
        }
    }
}
