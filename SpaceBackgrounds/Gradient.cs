using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace SpaceBackgrounds
{
    public class Gradient
    {
        public Gradient()
        {

        }
        public Gradient(float v, Color c)
        {
            GradientColor = c;
            Value = v;
        }
        public Color GradientColor;
        public float Value;
    }
}
