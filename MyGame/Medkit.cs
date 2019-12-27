using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Medkit : BaseObject
    {
        /// <summary>
        /// Количество востонавливаемой энергии
        /// </summary>
        public int RecoverEnergy { get; private set; }

        public Medkit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image = Image.FromFile("Medkit_Game.png");

            RecoverEnergy = dir.X; 
            Console.WriteLine($"Аптечка была создана в позиции x: {pos.X} y: {pos.Y}");
        }

        public override void Draw()
        {
            Rectangle sz = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(image, sz);
        }

        public override void Update()
        {
        }
    }
}
