using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SpaceBackgrounds;
using SpaceBackgrounds.Models;

namespace SpaceBackgroundsExample
{
    public class Game
    {
        public Game()
        {
            duration = new Text("0", new Font("DejaVuSans.ttf"), 50);
            duration.Position = new Vector2f(500, 530);
            duration.FillColor = Color.Red;
        }
        Sprite s;
        Text duration;
        Random r;

        public void Run()
        {
            r = new Random();
            Clock c = new Clock();
            GenerateImage();
            int dur = c.ElapsedTime.AsMilliseconds();
            duration.DisplayedString = Convert.ToString(dur);
            ContextSettings cs = new ContextSettings();
            cs.AntialiasingLevel = 4;
            VideoMode mode = new VideoMode(800, 600, 32);
            RenderWindow window = new RenderWindow(mode, "Space Scene Window", Styles.Close | Styles.Titlebar, cs);
            window.Closed += (object sender, EventArgs e) => window.Close();
            window.KeyPressed += KeyPress;
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw(s);
                window.Draw(duration);
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
                Clock c = new Clock();
                GenerateImage();
                int dur = c.ElapsedTime.AsMilliseconds();
                duration.DisplayedString = Convert.ToString(dur);
            }
        }
        private void GenerateImage()
        {
            StarSystem sys = new StarSystem(r.Next());
            sys.Asteroids = false;
            sys.Nebula = NebulaType.Monochrome;
            sys.Planets.Add(new Planet(PlanetType.Terran, 2));
            sys.Suns.Add(new Sun(SunType.Giant));
            BackgroundGenerator gen = new BackgroundGenerator(sys);
            gen.Run();
            s = new Sprite(gen.getTexture());
            s.Position = new Vector2f(0, 0);
        }
    }
}
