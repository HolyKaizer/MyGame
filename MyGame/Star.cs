using System;
using System.Drawing;

namespace MyGame
{
    /// <summary>
    /// Игровой объект "звезда"
    /// </summary>
	class Star : BaseObject
	{
        /// <summary>
        /// Конструктор, задающий все поля  
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
		public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
		{

		}
            
        /// <summary>
        /// Virtual метод отрисовывает BaseObject на игровом поле     
        /// </summary>
		public override void Draw()
		{
			Image starImage = Image.FromFile("Work_Star.jpg");
			Rectangle sz = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
			Game.Buffer.Graphics.DrawImage(starImage, sz);
		}

        /// <summary>
        /// Virtual метод обновляет положение BaseObject на игровом поле     
        /// </summary>
        public override void Update()
		{
			Pos.X = Pos.X - Dir.X;
			if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
		}
	}
}