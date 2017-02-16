using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace SpaceBackgrounds.Models
{
    public class Sun
    {
    	public Sun()
    	{
    		Type = SunType.Orange;
    	}
    	public Sun(SunType type)
    	{
    		Type = type;
    	}
    	public SunType Type;
    }
}
