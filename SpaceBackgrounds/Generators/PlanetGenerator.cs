using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace SpaceBackgrounds.Generators
{
    public class PlanetGenerator
    {
        public PlanetGenerator(Vector2u Size, int seed)
        {
            Surface = new Image(Size.X, Size.Y);
            Seed = seed;
        }
        public Image Surface;
        protected int Seed;
        public virtual void RenderPlanetSurface()
        {

        }
    }
}
