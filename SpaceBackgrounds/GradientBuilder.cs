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

namespace SpaceBackgrounds
{
    public class GradientBuilder
    {
        public GradientBuilder()
        {
            Gradients = new List<Gradient>();
        }
        public Image SourceImage;
        private Image DestinationImage;
        private List<Gradient> Gradients;
        public Image getRenderedImage()
        {
            return DestinationImage;
        }
        public void FillGreyscaleGradients()
        {
            AddGradient(new Gradient(0, Color.Black));
            AddGradient(new Gradient(255, Color.White));
        }
        public void ClearGradients()
        {
            Gradients.Clear();
        }
        public void PrepareGradients()
        {
            Gradients.Sort((x, y) => x.Value.CompareTo(y.Value));
        }
        public void AddGradient(Gradient g)
        {
            Gradients.Add(g);
        }
        public Image Render()
        {
            if (Gradients.Count < 2)
            {
                throw new Exception();
            }
            else if (Gradients[0].Value != 0 || Gradients[Gradients.Count - 1].Value != 255)
            {
                throw new Exception();
            }
            else
            {
                DestinationImage = new Image(SourceImage.Size.X, SourceImage.Size.Y);
                for (int i = 0; i < (int)SourceImage.Size.X; i++)
                {
                    for (int j = 0; j < (int)SourceImage.Size.Y; j++)
                    {
                        Color c = SourceImage.GetPixel((uint)i, (uint)j);
                        float v = c.R;
                        Gradient low = Gradients.FindLast(x => x.Value <= v);
                        Gradient high = Gradients.Find(x => x.Value >= v);
                        float lowDif = v - low.Value;
                        float highDif = high.Value - v;
                        float totalDif = lowDif + highDif;
                        Color f = new Color();

                        if (low.Value == v)
                        {
                            f = low.GradientColor;
                        }
                        else if (high.Value == v)
                        {
                            f = high.GradientColor;
                        }
                        else
                        {
                            float rel = highDif / totalDif;
                            f = Utils.MixColorAlpha(low.GradientColor, high.GradientColor, rel);
                        }
                        DestinationImage.SetPixel((uint)i, (uint)j, f);
                    }
                }
            }
            return DestinationImage;
        }
    }
}
