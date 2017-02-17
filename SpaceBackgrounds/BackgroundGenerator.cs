using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using SNU = SharpNoise.Utilities.Imaging;
using SharpNoise.Models;
using SpaceBackgrounds.Generators;
using SpaceBackgrounds.Models;

namespace SpaceBackgrounds
{
    public class BackgroundGenerator
    {
        public BackgroundGenerator(StarSystem s)
        {
            SystemDescription = s;
            Planets = new List<CelestialObject>();
            Suns = new List<CelestialObject>();
        }
        Image Main;
        public StarSystem SystemDescription;
        private List<CelestialObject> Planets;
        private List<CelestialObject> Suns;

        public void Run()
        {
            Random rand = new Random(SystemDescription.Seed);
			Main = new Image(800, 600, Color.Black);
            //Background Stars
            int starCount = rand.Next(500, 1800);
            byte c;
            Color randColor;
            for (int i = 0; i <= starCount; i++)
            {
                c = (byte)rand.Next(0, 255);
                randColor = new Color(c, c, c, 255);
                Main.SetPixel((uint)rand.Next(0, 799), (uint)rand.Next(599), randColor);
            }

            //Draw Nebula.
            if (SystemDescription.Nebula == NebulaType.Monochrome)
            {
                int startx = rand.Next(0, 5000);
                int starty = rand.Next(0, 5000);
                float down = 0.6f + ((float)rand.Next(0, 7) / 10);
                int type = rand.Next(0, 3);
                Image noise;
                if (type == 0)
                {
                    noise = getMCNoise(startx, starty, down, "R");
                    Main = Utils.BlendAlpha(Main, noise);
                }
                else if (type == 1)
                {
                    noise = getMCNoise(startx, starty, down, "G");
                    Main = Utils.BlendAlpha(Main, noise);
                }
                else if (type == 2)
                {
                    noise = getMCNoise(startx, starty, down, "B");
                    Main = Utils.BlendAlpha(Main, noise);
                }

            }
            else if (SystemDescription.Nebula == NebulaType.Polychrome)
            {
            	int startx = rand.Next(0, 5000);
                int starty = rand.Next(0, 5000);
                float down = 0.6f + ((float)rand.Next(0, 7) / 10);
                Image noise = getNoise(startx, starty, down);
                Main = Utils.BlendAlpha(Main, noise);
            }


            if (SystemDescription.Suns.Count != 0)
            {
                foreach(Sun sun in SystemDescription.Suns)
                {
                    Vector2f starPos = new Vector2f(rand.Next(0, 800), rand.Next(0, 600));
                    float rad1 = rand.Next(170, 410);
                    RenderStar(Main, rad1, getSunColor(sun.Type), getSunColor(sun.Type), starPos);
                    Suns.Add(new CelestialObject(starPos, rad1));
                }
            }

            //Draw planet

            if (SystemDescription.Planets.Count != 0)
            {
                foreach(Planet p in SystemDescription.Planets)
                {
                    bool cor = false;
                    float size = 0f;
                    Vector2f planetPosition = new Vector2f();
                    //Calculate Shadow
                    while (!cor)
                    {
                        size = rand.Next(70, 140);
                        planetPosition = new Vector2f(rand.Next(0, 800), rand.Next(0, 600));
                        cor = true;
                        foreach (CelestialObject planet in Planets)
                        {
                            if (distance(planet.Position, planetPosition) < planet.Size + size)
                            {
                                cor = false;
                            }
                        }
                    }
                    RenderPlanet(Main, planetPosition, size, p.Type, p.Moons);
                    Planets.Add(new CelestialObject(planetPosition, size));
                }
            }
        }
        private Image getNoise(int startx, int starty, float down)
        {
            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = Utils.getNoiseMap(startx, starty, 3, 3, 2.5, 0.5);
            NoiseMap mapg = Utils.getNoiseMap(startx + 100, starty + 100, 3, 3, 2.5, 0.5);
            NoiseMap mapb = Utils.getNoiseMap(startx + 200, starty + 200, 3, 3, 2.5, 0.5);
            NoiseMap mapa = Utils.getNoiseMap(startx + 700, starty + 700, 2, 2, 0.7, 0.3);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = Utils.NormNoise(mapr.GetValue(i, j));
                    byte g = Utils.NormNoise(mapg.GetValue(i, j));
                    byte b = Utils.NormNoise(mapb.GetValue(i, j));
                    byte a = Utils.NormNoise(mapa.GetValue(i, j) - (down));
                    colors[i, j] = new Color(r, g, b, a);
                }
            }
            return new Image(colors);
        }
        private Image getMCNoise(int startx, int starty, float down, string rgb)
        {
            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = Utils.getNoiseMap(startx, starty, 6, 6, 2.5, 0.5);
            NoiseMap mapa = Utils.getNoiseMap(startx + 300, starty + 300, 3, 3, 0.7, 0.3);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = Utils.NormNoise(mapr.GetValue(i, j));
                    byte a = Utils.NormNoise(mapa.GetValue(i, j) - down);
                    if (rgb == "R")
                    {
                        colors[i, j] = new Color(r, 0, 0, a);
                    }
                    else if (rgb == "G")
                    {
                        colors[i, j] = new Color(0, r, 0, a);
                    }
                    else
                    {
                        colors[i, j] = new Color(0, 0, r, a);
                    }
                }
            }
            return new Image(colors);
        }
        public Texture getTexture()
        {
        	return new Texture(Main);
        }
        private void RenderStar(Image img, float radius, Color color, Color haloColor, Vector2f pos)
        {
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    float t = distance(new Vector2f(i, j), pos);
                    Color f;
                    if (t < radius * 0.2f)
                    {
                        f = Utils.MixColorAlpha(Color.White, color, 1 / (t + 1));
                    }
                    else
                    {
                        f = Utils.MixColorAlpha(color, haloColor, function(t, 1 / (0.8f * radius)));
                    }
                    Color c = img.GetPixel((uint)i, (uint)j);
                    Color fi = Utils.MixColorAlpha(f, c, function(t, 1 / (0.7f * radius)));
                    img.SetPixel((uint)i, (uint)j, fi);
                }
            }
        }

        private float distance(Vector2f a, Vector2f b)
        {
            Vector2f t = a - b;
            double dist = Math.Sqrt(t.X * t.X + t.Y * t.Y);
            return (float)dist;
        }

        private Color getSunColor(SunType type)
        {
            Color orange = new Color(255, 128, 0);
            Color cyan = new Color(113, 120, 239);
            Color[] all = { cyan, orange, Color.Magenta, Color.White, Color.Red, Color.Yellow };
            Random r = new Random(SystemDescription.Seed - 333);
            switch (type)
            {
            	case SunType.Cyan:
            		return cyan;
            	case SunType.Magenta:
            		return Color.Magenta;
            	case SunType.Orange:
            		return orange;
            	case SunType.Red:
            		return Color.Red;
            	case SunType.White:
            		return Color.White;
            	case SunType.Yellow:
            		return Color.Yellow;
                default:
                    return all[r.Next(0, 7)];
            }
            
        }
        float function(float x, float k)
        {
            float s = 0f;
            float a = 1f;
            float result = s + a * (float)Math.Exp(-k * x);
            return result;
        }
        private void RenderPlanet(Image img, Vector2f pos, float size, PlanetType type, int moons)
        {
            Random rand = new Random(SystemDescription.Seed - 389);
            PlanetGenerator gen = getGenerator(type, new Vector2u(800, 600), rand.Next());
            gen.RenderPlanetSurface();
            Image noise = gen.Surface;
            List<CelestialObject> Moons = new List<CelestialObject>();

            for(int i = 0; i < moons; i++)
            {
                if (rand.Next(0, 2) == 1)
                {
                    Vector2f mpos = new Vector2f(rand.Next((int)pos.X - (int)size, (int)pos.X + 2 * (int)size), rand.Next((int)pos.Y - (int)size, (int)pos.Y + 2 * (int)size));
                    Moons.Add(new CelestialObject(mpos, rand.Next(8, 28)));
                }
            }
            float min = 1000f;
            CelestialObject nearestSun = new CelestialObject(new Vector2f(), 0f);
            foreach (CelestialObject sun in Suns)
            {
                float dist = distance(sun.Position, pos);
                if (dist < min)
                {
                    min = dist;
                    nearestSun = sun;
                }
            }
            bool Shadow = true; //Deactivated temporary
            Vector2f ShadowPosition = new Vector2f();
            float ShadowRadius = 0f;
            if (distance(nearestSun.Position, pos) < size)
            {
                //Draw no Shadow
                Shadow = false;
            }
            else
            {
                Vector2f dif = pos - nearestSun.Position;
                ShadowPosition = pos + new Vector2f(dif.X / 4, dif.Y / 4);
                ShadowRadius = size;
            }

            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    Vector2f current = new Vector2f(i, j);
                    float t = distance(current, pos);
                    if (t <= size)
                    {
                        Color c = noise.GetPixel((uint)i, (uint)j);
                        Color f = Utils.MixColorAlpha(Color.Black, c, t / (1.1f * size));

                        if (Shadow)
                        {
                            float d = distance(new Vector2f(i, j), ShadowPosition);
                            f = Utils.MixColorAlpha(f, Color.Black, function(d, 1 / (0.85f * ShadowRadius)));
                        }
                        img.SetPixel((uint)i, (uint)j, f);
                    }
                    foreach (CelestialObject co in Moons)
                    {
                        float dist = distance(co.Position, current);
                        if (dist <= co.Size)
                        {
                            img.SetPixel((uint)i, (uint)j, Color.Black);
                        }
                    }
                }
            }
        }

        private PlanetGenerator getGenerator(PlanetType type, Vector2u size, int seed)
        {
        	PlanetGenerator gen = GeneratorMapper.getGenerator(type, size, seed);
            return gen; 
        }
    }
}