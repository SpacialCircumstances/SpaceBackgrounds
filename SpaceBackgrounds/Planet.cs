using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace SpaceBackgrounds
{
    public class Planet
    {
        public Planet(Vector2f pos, float size)
        {
            Position = pos;
            Size = size;
        }
        public Vector2f Position;
        public float Size;
    }
}
