using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace SpaceBackgrounds
{
	public class BackgroundGenerator
	{
		public BackgroundGenerator()
		{
			FinalTexture = new RenderTexture(800, 600);
			Seed = new Random().Next();
		}
		public BackgroundGenerator(int seed)
		{
			FinalTexture = new RenderTexture(800, 600);
			Seed = seed;
		}
		RenderTexture FinalTexture;
		VertexArray varray;
		public int Seed;
		public void Run()
		{
			Random rand = new Random(Seed);
			FinalTexture.Clear(Color.Black);
            //Background Stars
            Image starsImage = new Image(800, 600);
			int starCount = rand.Next(300, 1500);
			byte c;
			Color randColor;
			for(int i = 0; i <= starCount; i++)
			{
				c = (byte)rand.Next(0, 255);
				randColor = new Color(c, c, c, 255);
				starsImage.SetPixel((uint)rand.Next(0, 799), (uint)rand.Next(599), randColor);
			}
			Texture starsTexture = new Texture(starsImage);
			Sprite starsSprite = new Sprite(starsTexture);
			starsSprite.Position = new Vector2f(0, 0);
			FinalTexture.Draw(starsSprite);
            //Nebula
            Vertex vLT = new Vertex(new Vector2f(0, 0), new Vector2f(0, 0));
            Vertex vRT = new Vertex(new Vector2f(800, 0), new Vector2f(1, 0));
            Vertex vRB = new Vertex(new Vector2f(800, 600), new Vector2f(1, 1));
            Vertex vLB = new Vertex(new Vector2f(0, 600), new Vector2f(0, 1));
            Vertex[] varray = { vLT, vRT, vRB, vLB };
            Shader NebulaShader = new Shader("Resources/Shaders/Nebula.vert", "Resources/Shaders/Nebula.frag");
            NebulaShader.SetParameter("source", starsTexture);
            NebulaShader.SetParameter("offset", new Vector2f(0, 0));
            NebulaShader.SetParameter("scale", 1f);
            NebulaShader.SetParameter("falloff", 4f);
            NebulaShader.SetParameter("color", Color.Red);
            NebulaShader.SetParameter("density", 0.1f);
            RenderStates NebulaRenderStates = new RenderStates(NebulaShader);
            FinalTexture.Draw(varray, PrimitiveType.Quads, NebulaRenderStates);
			FinalTexture.Display();
		}
		public Texture getTexture()
		{
			return FinalTexture.Texture;
		}
	}
}
