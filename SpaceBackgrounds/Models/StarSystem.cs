using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBackgrounds.Models
{
    public class StarSystem
    {
        public StarSystem()
        {
            Suns = new List<Sun>();
            Planets = new List<Planet>();
            Nebula = NebulaType.None;
            Asteroids = false;
            Seed = 0;
        }
        public StarSystem(NebulaType type, Boolean asteroids)
        {
        	Asteroids = asteroids;
        	Nebula = type;
            Suns = new List<Sun>();
            Planets = new List<Planet>();
            Seed = 0;
        }
        public StarSystem(int seed)
        {
        	Seed = seed;
            Suns = new List<Sun>();
            Planets = new List<Planet>();
            Nebula = NebulaType.None;
            Asteroids = false;
        }
        public List<Sun> Suns;
        public List<Planet> Planets;
        public NebulaType Nebula;
        public Boolean Asteroids;
        public int Seed;
    }
}
