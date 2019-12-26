using System;
using System.Drawing;

namespace MyGame 
{
    /// <summary>
    /// Интерфейс дающий функционал столкновений 
    /// </summary>
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; } 
    }

    /// <summary>
    /// Базовый класс в иерархии игровых объектов
    /// </summary>
	abstract class BaseObject: ICollision
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
        /// Размер collision box'a у объекта 
        /// </summary>
        public Rectangle Rect => new Rectangle(Pos, Size);

        /// <summary>
        /// Произошло ли столкновение между двумя объектами
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Collision(ICollision obj) => obj.Rect.IntersectsWith(this.Rect);


        /// <summary>
        /// Virtual метод отрисовывает BaseObject на игровом поле  
        /// </summary>
        public abstract void Draw();

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