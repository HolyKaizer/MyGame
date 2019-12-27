using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public interface IComparable<T>
    {
        int CompareTo(T obj);
    }

    class Asteroid : BaseObject, ICloneable, IComparable<Asteroid>
    {
        public int Power { get; set; } = 3;

        public Asteroid(Point pos, Point dir, Size size ) : base(pos, dir, size)
        {
        }


        /// <summary>
        /// Отрисовка астероида на игровом поле
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Клонируем объект астероид
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Создаем копию нашего астероида
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height));
            asteroid.Power = Power;

            return asteroid;
        }


        /// <summary>
        /// Логика полета астероида
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }

        /// <summary>
        /// Обновляет позицию астероида на правую часть экрана
        /// </summary>
        public void Reset()
        {
            Pos.X = Size.Width - 100;
            Pos.Y = random.Next(20, Game.Height - 20);
        }


        /// <summary>
        /// Проверяет 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable<Asteroid>.CompareTo(Asteroid obj)
        {
            if (Power > obj.Power) return 1;
            if (Power < obj.Power) return -1;
            return 0;
        }

    }
}
