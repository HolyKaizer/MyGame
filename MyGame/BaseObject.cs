using System;
using System.Drawing;

namespace MyGame 
{
    /// <summary>
    /// Базовый класс в иерархии игровых объектов
    /// </summary>
	class BaseObject
	{
        /// <summary>
        /// Позиция игрового объекта 
        /// </summary>
		protected Point Pos;

        /// <summary>
        /// Направление игрового объекта 
        /// </summary>
		protected Point Dir;

        /// <summary>
        /// Размер игрового объекта
        /// </summary>
		protected Size Size;

        /// <summary>
        /// Базовый конструктор инициализирующий все поля
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
		public BaseObject(Point pos, Point dir, Size size) 
		{ 
			Pos = pos;
			Dir = dir; 
			Size = size;
		}

        /// <summary>
        /// Virtual метод отрисовывает BaseObject на игровом поле  
        /// </summary>
        public virtual void Draw() 
		{ 
			Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, 
												Size.Width, Size.Height);
		}

        /// <summary>
        /// Virtual метод обновляет положение BaseObject на игровом поле 
        /// </summary>
		public virtual void Update() 
		{
			Pos.X = Pos.X + Dir.X;
			Pos.Y = Pos.Y + Dir.Y;
			if (Pos.X < 0) Dir.X = -Dir.X;
			if (Pos.X > Game.Width) Dir.X = -Dir.X;
			if (Pos.Y < 0) Dir.Y = -Dir.Y;
			if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
		}
	}  
}