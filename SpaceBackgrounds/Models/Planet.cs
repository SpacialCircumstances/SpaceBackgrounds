using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBackgrounds.Models
{
    public class Planet
    {
        public Planet()
        {
            Moons = 0;
        }
        public Planet(PlanetType type)
        {
            Type = type;
            Moons = 0; 
        }
        public Planet(PlanetType type, int moons)
        {
        	Type = type;
        	Moons = moons;
        }
        public PlanetType Type;
        public int Moons;
    }
}
