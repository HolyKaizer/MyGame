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
        public int BulletSpeed {get; private set;} 
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image = Image.FromFile("Game_Lazer.png");
            BulletSpeed = dir.X;
        }

        public override void Draw()
        {
            Rectangle sz = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(image, sz);
        }

        public override void Update()
        {
            Pos.X = Pos.X + BulletSpeed;
        }

        /// <summary>
        /// Устанавливает положение снаряда на левую сторону
        /// </summary>
        public void Reset()
        {
            Pos.X = 0;
            Pos.Y = random.Next(0, Game.Height);
            
        }
    }
}
