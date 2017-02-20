using System;
using SpaceBackgrounds;

namespace SpaceBackgroundsExample
{
	class Program
	{
		public static void Main(string[] args)
		{
            try
            {
                Game game = new Game();
                game.Run();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.TargetSite);
                Console.Read();
            }
		}
	}
}