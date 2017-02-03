using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using SharpNoise.Utilities;
using SharpNoise.Models;

namespace SpaceBackgrounds
{
    public class BackgroundGenerator
    {
        public BackgroundGenerator()
        {
            FinalTexture = new RenderTexture(800, 600);
            Seed = new Random().Next();
        }
        public BackgroundGenerator(int seed, int sunCount, bool hasNebula)
        {
            FinalTexture = new RenderTexture(800, 600);
            Seed = seed;
            SunCount = sunCount;
            NebulaActivated = hasNebula;
        }
        RenderTexture FinalTexture;
        public int Seed;
        public int SunCount;
        public bool NebulaActivated;
        public void Run()
        {
            Random rand = new Random(Seed);
            FinalTexture.Clear(Color.Black);
            //Background Stars
            Image starsImage = new Image(800, 600);
            int starCount = rand.Next(500, 1800);
            byte c;
            Color randColor;
            for (int i = 0; i <= starCount; i++)
            {
                c = (byte)rand.Next(0, 255);
                randColor = new Color(c, c, c, 255);
                starsImage.SetPixel((uint)rand.Next(0, 799), (uint)rand.Next(599), randColor);
            }
            Texture starsTexture = new Texture(starsImage);
            Sprite starsSprite = new Sprite(starsTexture);
            starsSprite.Position = new Vector2f(0, 0);
            FinalTexture.Draw(starsSprite);

            //Draw Nebula.
            if (NebulaActivated)
            {
                int startx = rand.Next(0, 5000);
                int starty = rand.Next(0, 5000);
                float down = 0.6f + ((float)rand.Next(0, 7) / 10);
                int type = rand.Next(0, 7);
                Image noise;
                if (type == 6)
                {
                    noise = getMCNoise(startx, starty, down, "R");
                }
                else if (type == 5)
                {
                    noise = getMCNoise(startx, starty, down, "G");
                }
                else if (type == 4)
                {
                    noise = getMCNoise(startx, starty, down, "B");
                }
                else
                {
                    noise = getNoise(startx, starty, down);
                }
                Texture noiseTexture = new Texture(noise);
                Sprite noiseSprite = new Sprite(noiseTexture);
                noiseSprite.Position = new Vector2f(0, 0);
                FinalTexture.Draw(noiseSprite);
            }


            //Draw sun(s).
            if (SunCount != 0)
            {
                Image stars = FinalTexture.Texture.CopyToImage();
                for (int i = 0; i < SunCount; i++)
                {
                    Vector2f starPos = new Vector2f(rand.Next(0, 800), rand.Next(0, 600));
                    int co = rand.Next(0, 5);
                    float rad1 = rand.Next(170, 410);
                    RenderStar(stars, rad1, getSunColor(co), getSunColor(co), starPos);
                }
                Texture sunTexture = new Texture(stars);
                Sprite s = new Sprite(sunTexture);
                s.Position = new Vector2f(0, 0);
                FinalTexture.Draw(s);
            }

            //Draw planet
            Image pl = FinalTexture.Texture.CopyToImage();
            Vector2f planetPosition = new Vector2f(rand.Next(0, 800), rand.Next(0, 600));
            RenderPlanet(pl, planetPosition, 70);
            Texture planetTexture = new Texture(pl);
            Sprite p = new Sprite(planetTexture);
            p.Position = new Vector2f(0, 0);
            FinalTexture.Draw(p);

            FinalTexture.Display();
            

        }
        public Image getNoise(int startx, int starty, float down)
        {
            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = getNoiseMap(startx, starty, 3, 3, 2.5, 0.5);
            NoiseMap mapg = getNoiseMap(startx + 100, starty + 100, 3, 3, 2.5, 0.5);
            NoiseMap mapb = getNoiseMap(startx + 200, starty + 200, 3, 3, 2.5, 0.5);
            NoiseMap mapa = getNoiseMap(startx + 700, starty + 700, 2, 2, 0.7, 0.3);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = NormNoise(mapr.GetValue(i, j));
                    byte g = NormNoise(mapg.GetValue(i, j));
                    byte b = NormNoise(mapb.GetValue(i, j));
                    byte a = NormNoise(mapa.GetValue(i, j) - (down + 0.05f));
                    colors[i, j] = new Color(r, g, b, a);
                }
            }
            return new Image(colors);
        }
        public Image getMCNoise(int startx, int starty, float down, string rgb)
        {
            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = getNoiseMap(startx, starty, 6, 6, 2.5, 0.5);
            NoiseMap mapa = getNoiseMap(startx + 300, starty + 300, 3, 3, 0.7, 0.3);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = NormNoise(mapr.GetValue(i, j));
                    byte a = NormNoise(mapa.GetValue(i, j) - down);
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
            return FinalTexture.Texture;
        }
        private byte NormNoise(double val)
        {
            if (val > 1)
            {
                val = 1;
            }
            else if (val < -1)
            {
                val = -1;
            }
            byte result = (byte)((val + 1) * 127);
            return result;
        }
        private NoiseMap getNoiseMap(double seedx, double seedy, int height, int width, double lacunarity, double persistence)
        {
            NoiseMap map = new NoiseMap();
            PlaneNoiseMapBuilder builder = new PlaneNoiseMapBuilder();
            Simplex simplex = new Simplex();
            simplex.Lacunarity = lacunarity;
            simplex.Persistence = persistence;
            builder.SourceModule = simplex;
            builder.DestNoiseMap = map;
            builder.SetDestSize(800, 600);
            builder.SetBounds(seedx, seedx + width, seedy, seedy + height);
            builder.Build();
            return map;
        }
        private Color MixColorAlpha(Color a, Color b, float alpha)
        {
            if(alpha > 1)
            {
                throw new Exception("Alpha can be maximal 1");
            }
            float r = 1 - alpha;
            Color am = new Color((byte)(a.R * alpha), (byte)(a.G * alpha), (byte)(a.B * alpha));
            Color bm = new Color((byte)(b.R * r), (byte)(b.G * r), (byte)(b.B * r));
            return am + bm;
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
                        f = MixColorAlpha(Color.White, color, 1 / (t + 1));
                    }
                    else
                    {
                        f = MixColorAlpha(color, haloColor, function(t, 1 / (0.8f * radius)));
                    }
                    Color c = img.GetPixel((uint)i, (uint)j);
                    Color fi = MixColorAlpha(f, c, function(t, 1 / (0.7f * radius)));
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

        private Color getSunColor(int num)
        {
            Color orange = new Color(255, 128, 0);
            Color cyan = new Color(113, 120, 239);
            Color[] cols = { cyan, Color.White, Color.Yellow, orange, Color.Magenta };
            return cols[num];
        }
        float function(float x, float k)
        {
            float s = 0f;
            float a = 1f;
            float result = s + a * (float)Math.Exp(-k * x);
            return result;
        }
        private void RenderPlanet(Image img, Vector2f pos, float size)
        {
            Random rand = new Random(Seed - 389);
            int x = rand.Next(0, 3000);
            int y = rand.Next(0, 3000);
            Image noise = getPlanetSurface(x, y);
            for(int i = 0; i < 800; i++)
            {
                for(int j = 0; j < 600; j++)
                {
                    float t = distance(new Vector2f(i, j), pos);
                    if(t <= size)
                    {
                        Color c = noise.GetPixel((uint)i, (uint)j);
                        img.SetPixel((uint)i, (uint)j, c);
                    }
                    else if(t <= (size + 3))
                    {
                        Color c = noise.GetPixel((uint)i, (uint)j);
                        Color b = img.GetPixel((uint)i, (uint)j);
                        Color blue = new Color(0, 170, 188);
                        Color f = MixColorAlpha(c, blue, 0.2f);
                        Color fi = MixColorAlpha(f, b, t / (size + 5));
                        img.SetPixel((uint)i, (uint)j, f);
                    }
                }
            }
        }
        private Image getPlanetSurface(int startx, int starty)
        {

            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = getNoiseMap(startx, starty, 6, 6, 2.5, 0.5);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = NormNoise(mapr.GetValue(i, j));
                    colors[i, j] = new Color(r, r, r);
                }
            }
            return new Image(colors);
        }
    }
}
