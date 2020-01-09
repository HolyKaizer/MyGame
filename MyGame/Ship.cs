using System;
using System.Drawing;

namespace MyGame
{
    /// <summary>
    /// Класс, представляющий корабль игрока
    /// </summary>
    class Ship : BaseObject
    {
        /// <summary>
        /// Событие реагирующее на смерть
        /// </summary>
        public static event Message MessageDie;

        /// <summary>
        /// Текущая энергия корабля
        /// </summary>
        public int Energy { get; private set; } = 100;

        /// <summary>
        /// Метод уменьшающий энергию корабля
        /// </summary>
        /// <param name="n">Значение на которое изменяется</param>
        public void EnergyDecrease(int n)
        {
            Energy -= n;
        }

        /// <summary>
        /// Метод увеличивающий энергию корабля
        /// </summary>
        /// <param name="n">Значение на которое изменяется</param>
        public void EnergyIncrease(int n)
        {
            Energy += n;
        }

        /// <summary>
        /// Конструктор Ship вызвающий базовый конструктор BaseObject
        /// </summary>
        /// <param name="pos">Позиция коробля</param>/param>
        /// <param name="dir">Скорость корабля</param>
        /// <param name="size">Размер коробля</param>
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size) 
        {
            image = Image.FromFile("Space_Ship_Game.png");
        }

        /// <summary>
        /// Метод отрисовывает коробль на игровом поле
        /// </summary>
        public override void Draw()
        {
            Rectangle sz = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(image, sz);
        }

        /// <summary>
        /// Метоод обновляет положение коробля на игровом поле
        /// </summary>
        public override void Update()
        {

        }

        /// <summary>
        /// Изменение положения коробля вверх на игровом поле на значение Dir.Y 
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        /// <summary>
        /// Изменение положения коробля вниз на игровом поле на значение Dir.Y 
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height - Size.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        /// <summary>
        /// Метод обрабатывающий уничтожение коробля  
        /// </summary>
        public void Die()
        {
            MessageDie?.Invoke();
        }

    }
}
