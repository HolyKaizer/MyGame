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
            image = Image.FromFile("Work_Star.jpg");
        }

        
        /// <summary>
        /// Метод отрисовывает звезду на игровом поле    
        /// </summary>
		public override void Draw()
		{
			Rectangle sz = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
			Game.Buffer.Graphics.DrawImage(image, sz);
		}

        /// <summary>
        /// Метод управляет поведением звезды на игровом поле     
        /// </summary>
        public override void Update()
		{
			Pos.X = Pos.X - Dir.X;
			if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
		}
	}
}