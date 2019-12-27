using System;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame 
{
    /// <summary>
    /// Основной класс, управляющий игрой
    /// </summary>
	static class Game 
	{
        /// <summary>
        /// Предоставляет доступ к главному буферу
        /// графического контекста для текущего приложения 
        /// </summary>
		private static BufferedGraphicsContext _context;
		public static BufferedGraphics Buffer;

        private static Timer _timer = new Timer { Interval = 75 };
        public static Random Rnd = new Random();

        /// <summary>
        /// Корабль игрока
        /// </summary>
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));

        /// <summary>
        /// Ширина игрового поля  
        /// </summary>
        public static int Width { get; set; }

        /// <summary>
        /// Высота игрового поля  
        /// </summary>
		public static int Height { get; set; }

        /// <summary>
        /// Массив всех объектов на игровом поле  
        /// </summary>
		private static BaseObject[] _objs;

		/// <summary>
		/// Текущая пуля на игровом поле
		/// </summary>
		private static Bullet _bullet;

		/// <summary>
		/// Массив астероидов в игре
		/// </summary>
		private static Asteroid[] _asteroids;

		static Game() 
		{

		}

		/// <summary>
        /// Загружает все объекты на игровом поле
        /// </summary>
		private static void Load() 
		{ 
			_objs = new BaseObject[100];

			_bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(10,3));

			_asteroids = new Asteroid[30];
			Random rnd = new Random();

            //Заполнение массива -objs[] звездами со случайной скоростью, позицией и размером
			for (int i = 0; i < _objs.Length; i++)
			{
				Point pos = new Point(rnd.Next(20, Width - 20), rnd.Next(20, Height - 20));
				Point dir = new Point(rnd.Next(2, 14), 0);
				int objWidth = rnd.Next(2, 10);
				Size sz = new Size(objWidth, objWidth);

				_objs[i] = new Star(pos, dir, sz);

			}

			//Заполнение массива _asteroids[] астероидами со случайной скоростью, позицией и размером
			for (int i = 0; i < _asteroids.Length; i++)
			{
				Point pos = new Point(Width - 50, rnd.Next(20, Height - 20));
				Point dir = new Point(rnd.Next(-1, 10), rnd.Next(5, 50));
				int objWidth = rnd.Next(5, 50);
				Size sz = new Size(objWidth, objWidth);

				_asteroids[i] = new Asteroid(pos, dir, sz);

			}
		}

        /// <summary>
        /// Инициализация игрового поля 
        /// </summary>
        /// <param name="form">Объект Form выступающий в роли поля</param>
		public static void Init(Form form) 
		{
			// Графическое устройство для вывода графики 
			Graphics g;

			// Предоставляет доступ к главному буферу
			// графического контеста для текущего приложения 
			_context = BufferedGraphicsManager.Current;
			g = form.CreateGraphics();

			// Создаем объект (поверхность рисования) и связываем его с формой 
			// Запоминаем размеры формы 
			Width = form.ClientSize.Width;
			Height = form.ClientSize.Height;

			//Cвязываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
			Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

			//Вызываем метод загрузки всех объектов в сцене
			Load();


            _timer.Start();
            _timer.Tick += Timer_Tick;

            //Добавляем обработку событий при нажатии клавиш
            form.KeyDown += Form_KeyDown;

            //Добавляем в событие смерть корабля метод Finish
            Ship.MessageDie += Finish;

        }

        /// <summary>
        /// Метод, вызывающийся каждый "тик"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private static void Timer_Tick(object sender, EventArgs e) 
		{
			//Вызывает рендер методы каждые 100 милисекунд  
			Draw();
			Update(); 
		}

        /// <summary>
        /// Метод обрабатывающий нажатие 
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Дополнительный класс с аргументами делегата</param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            //Обработка выстрела - клавиша 'Control'
            Point bulPos = new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4);
            Point bulDir = new Point(0, 4);
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(bulPos, bulDir, new Size(4, 1));

            //Обработка управления кораблем - клавиши 'Вверх' 'Вниз'
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Обновление игрового поля
        /// </summary>
		public static void Update() 
		{
			//Обновляем каждый объект
			foreach (BaseObject obj in _objs) 
				obj.Update();

            //Обновляем снаряд
            _bullet.Update();

            //Обновляем каждый астероид
            for (var i = 0; i < _asteroids.Length; i++)
            {
                //Если текущий астероид не существует - переходим на следующую итерацию
                if (_asteroids[i] == null) continue;

                //Обновляем астероид
                _asteroids[i].Update();

                //Проверяем на сталкновение. Если произошло - убираем два объекта с поля и переходим на следующую итерацию
                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    continue;
                }

                //Обрабатывем столкновение коробля и астероида
                if (!_ship.Collision(_asteroids[i])) continue;

                var rnd = new Random();
                _ship?.EnergyDecrease(rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();

                //Если энергии после столкновения не осталось - корабль уничтожается
                if (_ship.Energy <= 0) _ship?.Die();
            }



            //Проверяем размеры экрана
            if (Width > 1000 || Height > 1000)
                throw new ArgumentOutOfRangeException();
		}

        /// <summary>
        /// Отрисовка игрового поля 
        /// </summary>
        public static void Draw()
        {
            //Вызываем отрисовку для каждого объекта
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();

            foreach (Asteroid a in _asteroids)
                a?.Draw();


            _bullet?.Draw();
            _ship?.Draw();

            //Отрисовка текущей энергии
            if (_ship != null)
                Buffer.Graphics.DrawString($"Energy: {_ship?.Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);


            Buffer.Render();
        }

        /// <summary>
        /// Метод заканчивающий игру 
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }
    }

}


