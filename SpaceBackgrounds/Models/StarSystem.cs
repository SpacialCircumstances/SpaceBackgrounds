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
