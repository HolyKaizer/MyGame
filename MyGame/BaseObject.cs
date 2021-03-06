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
    /// Базовый класс в иерархии игровых объектов,
    /// реализует интерфейс ICollision для обработки столкновений 
    /// </summary>
	abstract class BaseObject: ICollision
	{
        /// <summary>
        /// Обработчик событий
        /// </summary>
        public delegate void Message();

        /// <summary>
        /// Общий рандом для случайных событий с объектом 
        /// </summary>
        protected static Random random;

        /// <summary>
        /// Картинка у объекта
        /// </summary>
        protected Image image = null;

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
            CheckForAccording(pos, dir, size);
            random = new Random();
            Pos = pos;
			Dir = dir; 
			Size = size;
		}

        /// <summary>
        /// Проверка на выход за допустимые пределы
        /// </summary>
        /// <param name="pos">Положение объекта</param>
        /// <param name="dir">Скорость и направление объекта</param>
        /// <param name="size">Размер объекта</param>
        protected void CheckForAccording(Point pos, Point dir, Size size)
        {
            if (pos.X < -100 || pos.Y < -100 || pos.X > Game.Width + 100 || pos.Y > Game.Height + 100 )
                throw new GameObjectException("Положение объекта не может быть отрицательным и выходить за размер экрана!");
            if (dir.X > 100 || dir.Y > 100)
                throw new GameObjectException("Максимальная скорость объекта - 100. Выход за пределы!");
            if (size.Width < 2 || size.Height < 2)
                throw new GameObjectException("Минимальный размер 2 еденицы. Выход за пределы!");

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
		abstract public void Update();
	}  
}