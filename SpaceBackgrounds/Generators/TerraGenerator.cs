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
    public class TerraGenerator: PlanetGenerator
    {
        public TerraGenerator(Vector2u size, int seed): base(size, seed)
        {
            rand = new Random(seed);
        }
        Random rand;
        public override void RenderPlanetSurface()
        {
            base.RenderPlanetSurface();

            Surface = getPlanetSurface(rand.Next(0, 34000), rand.Next(0, 34000));
        }
        private Image getPlanetSurface(int startx, int starty)
        {

            Color[,] colors = new Color[800, 600];
            NoiseMap mapr = Utils.getNoiseMap(startx, starty, 12, 12, 2.7, 0.8);
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 600; j++)
                {
                    byte r = Utils.NormNoise(mapr.GetValue(i, j));
                    colors[i, j] = new Color(r, r, r);
                }
            }
            GradientBuilder grad = new GradientBuilder();
            grad.AddGradient(new Gradient(0, Color.Blue));
            grad.AddGradient(new Gradient(50, Color.Yellow));
            grad.AddGradient(new Gradient(140, Color.Green));
            grad.AddGradient(new Gradient(255, Color.White));
            grad.PrepareGradients();
            grad.SourceImage = new Image(colors);
            return grad.Render();
        }
    }
}
