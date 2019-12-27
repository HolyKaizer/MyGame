using System;
using System.Drawing;

namespace MyGame
{
    /// <summary>
    /// Класс представляет корабль игрока
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
        /// <param name="n">Значение на которое стоит изменить</param>
        public void EnergyDecrease(int n)
        {

        }

        /// <summary>
        /// Конструктор Ship вызвающий базовый конструктор BaseObject
        /// </summary>
        /// <param name="pos">Позиция коробля</param>/param>
        /// <param name="dir">Скорость корабля</param>
        /// <param name="size">Размер коробля</param>
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        /// <summary>
        /// Метод отрисовывает коробль на игровом поле
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Метоод обновляет положение коробля на игровом поле
        /// </summary>
        public override void Update()
        {

        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        public void Down()
        {
            if (Pos.Y > Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }

    }
}
