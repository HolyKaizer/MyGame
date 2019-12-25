using System;
using System.Drawing;

namespace MyGame
{
	class Star : BaseObject
	{

		public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
		{

		}

		public override void Draw()
		{
			Image starImage = Image.FromFile("Work_Star.jpg");
			Rectangle sz = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
			Game.Buffer.Graphics.DrawImage(starImage, sz);
		}

		public override void Update()
		{
			Pos.X = Pos.X - Dir.X;
			if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
		}
	}
}