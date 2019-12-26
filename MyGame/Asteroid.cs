using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{

    class Asteroid : BaseObject, ICloneable
    {
        public int Power { get; set; }

        public Asteroid(Point pos, Point dir, Size size ) : base(pos, dir, size)
        {
            Power = 1;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
            Console.WriteLine($"Asteroid was drawed at position (x: {Pos.X}, y: {Pos.Y})");
        }

        /// <summary>
        /// Клонируем объект астероид
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height));
            asteroid.Power = Power;

            return asteroid;
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }

        public void Reset()
        {
            Pos.X = Size.Width - 100;
            Pos.Y = random.Next(20, Game.Height - 20);
        }
    }
}
