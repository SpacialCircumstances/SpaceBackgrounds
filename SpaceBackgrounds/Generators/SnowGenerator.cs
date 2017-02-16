using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SharpNoise;

namespace SpaceBackgrounds.Generators
{
    public class SnowGenerator: PlanetGenerator
    {
        public SnowGenerator(Vector2u size, int seed): base(size, seed)
        {
            rand = new Random(seed);
        }
        Random rand;
        public override void RenderPlanetSurface()
        {
            base.RenderPlanetSurface();
            Surface = getPlanetSurface();
        }
        private Image getPlanetSurface()
        {
            int startx = rand.Next(0, 94544);
            int starty = rand.Next(0, 94544);
            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = Utils.getNoiseMap(startx, starty, 12, 12, 3.1, 0.9);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = Utils.NormNoise(mapr.GetValue(i, j));
                    colors[i, j] = new Color(r, r, r);
                }
            }
            GradientBuilder grad = new GradientBuilder();
            Color grey = new Color(150, 150, 150);
            grad.AddGradient(new Gradient(0, grey));
            grad.AddGradient(new Gradient(255, Color.White));
            grad.PrepareGradients();
            grad.SourceImage = new Image(colors);
            return grad.Render();
        }
    }
}
