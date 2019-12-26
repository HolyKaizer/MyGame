using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Bullet : BaseObject 
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + 3;
        }

        /// <summary>
        /// Устанавливает положение снаряда на левую сторону
        /// </summary>
        public void Reset()
        {
            Random rnd = new Random();

            Pos.X = 0;
            Pos.Y = rnd.Next(0, Size.Height);
        }
    }
}
