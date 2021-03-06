﻿/*
SpaceBackgroundsGenerator
Copyright(c) 2017 Felix Nolte

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
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
