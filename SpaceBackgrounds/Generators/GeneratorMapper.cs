using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SpaceBackgrounds.Models;

namespace SpaceBackgrounds.Generators
{
    public static class GeneratorMapper
    {
        public static PlanetGenerator getGenerator(PlanetType type, Vector2u size, int seed)
        {
            switch(type)
            {
                case PlanetType.GasGiant:
                    throw new NotImplementedException();
                    return null;
                    break;
                case PlanetType.Ice:
                    return new SnowGenerator(size, seed);
                    break;
                case PlanetType.Terran:
                    return new TerraGenerator(size, seed);
                    break;
                case PlanetType.Water:
                    return new BluePlanetGenerator(size, seed);
                    break;
                default:
                    throw new NotSupportedException();
                    break;
            }
        }
    }
}
