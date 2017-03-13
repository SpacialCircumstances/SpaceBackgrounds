/*
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
using SharpNoise.Builders;
using SharpNoise.Modules;

namespace SpaceBackgrounds
{
    static public class Utils
    {
        public static NoiseMap getNoiseMap(double seedx, double seedy, int height, int width, double lacunarity, double persistence)
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
        public static Color MixColorAlpha(Color a, Color b, float alpha)
        {
            if (alpha > 1)
            {
                alpha = 1;
            }
            float r = 1 - alpha;
            Color am = new Color((byte)(a.R * alpha), (byte)(a.G * alpha), (byte)(a.B * alpha));
            Color bm = new Color((byte)(b.R * r), (byte)(b.G * r), (byte)(b.B * r));
            return am + bm;
        }
        public static byte NormNoise(double val)
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
        
        public static float ReverseNorm(byte val)
        {
        	float result = (float)val / 256;
        	return result;
        }
        public static Image BlendAlpha(Image a, Image b)
        {
            Color[,] colors = new Color[800, 600];
        	for (int i = 0; i < 800; i++)
        	{
        		for (int j = 0; j < 600; j++)
        		{
        			Color ac = a.GetPixel((uint)i, (uint)j);
                    Color bc = b.GetPixel((uint)i, (uint)j);
        			colors[i, j] = MixColorAlpha(ac, bc,1 - ReverseNorm(bc.A));
        		}
        	}
        	return new Image(colors);
        }
    }
}
