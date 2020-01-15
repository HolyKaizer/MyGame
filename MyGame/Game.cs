using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace MyGame 
{
    /// <summary>
    /// Основной класс, управляющий игрой
    /// </summary>
	static class Game 
	{
        /// <summary>
        /// Делегат отвечающий за вывод логов
        /// </summary>
        /// <param name="info">Лог, записываемый в файл</param>
        public delegate void LogDelegate(string info);

        /// <summary>
        /// Событие, фиксирующее добавление лога в файл
        /// </summary>
        public static LogDelegate logEvent;

        /// <summary>
        /// Предоставляет доступ к главному буферу
        /// графического контекста для текущего приложения 
        /// </summary>
		private static BufferedGraphicsContext _context;
		public static BufferedGraphics Buffer;

        // Поля для расчитывания времени появление аптечки
        private static int MedkitMaxInterval = 90;
        private static int MedkitMinInterval = 50;
        private static int Interval = 0;

        // Таймер отвечающий за частоту обновления игрового поля 
        private static Timer _timer = new Timer { Interval = 75 };
        public static Random Rnd = new Random();

        /// <summary>
        /// Корабль игрока
        /// </summary>
        private static Ship _ship;

        /// <summary>
        /// Аптечка появляющаяся с определенной периодичностью
        /// </summary>
        private static Medkit _medkit = null;

        /// <summary>
        /// Текущий счет игрока
        /// </summary>
        private static int Points = 0;

        /// <summary>
        /// Количество очков за разбитый астероид
        /// </summary>
        private static int PointsPerAsteroid = 25;

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
		/// Текущий список пуль на экране 
		/// </summary>
		private static List<Bullet> _bullets = new List<Bullet>();

        /// <summary>
        /// Стартовое колличество астероидов
        /// </summary>
        private static int InitialAsteroidsSpawn = 15;

		/// <summary>
		/// Список астероидов в игре 
		/// </summary>
		private static List<Asteroid> _asteroids = new List<Asteroid>();

		static Game() 
		{

		}

		/// <summary>
        /// Загружает все объекты на игровом поле
        /// </summary>
		private static void Load() 
		{ 
			_objs = new BaseObject[100];


            // Заполнение массива -objs[] звездами со случайной скоростью, позицией и размером
            for (int i = 0; i < _objs.Length; i++)
			{
				Point pos = new Point(Rnd.Next(20, Width - 20), Rnd.Next(20, Height - 20));
				Point dir = new Point(Rnd.Next(2, 14), 0);
				int objWidth = Rnd.Next(2, 12);
				Size sz = new Size(objWidth, objWidth);

				_objs[i] = new Star(pos, dir, sz);

			}

            // Заполнение массива _asteroids[] астероидами со случайной скоростью, позицией и размером
            SpawnAsteroids();
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

			// Cвязываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
			Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));


            _ship = new Ship(new Point(10, 400), new Point(20, 20), new Size(35, 55));

            // Вызываем метод загрузки всех объектов в сцене
            Load();

            // Велючаем тайме и добавляем метод в его событие 
            _timer.Start();
            _timer.Tick += Timer_Tick;

            // Добавляем обработку событий при нажатии клавиш
            form.KeyDown += Form_KeyDown;

            // Добавляем в событие смерть корабля - метод Finish
            Ship.MessageDie += Finish;
            Game.logEvent += AddToLog;

            StartGame();
        }

        /// <summary>
        /// Метод, вызывающийся каждый "тик"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private static void Timer_Tick(object sender, EventArgs e) 
		{
            // Вызывает рендер методы каждые 75 милисекунд  
            MedkitPut();
			Draw();
			Update(); 
		}

        /// <summary>
        /// Функция инициализирующая начальный список астероидов
        /// </summary>
        private static void SpawnAsteroids()
        {
            for (int i = 0; i < InitialAsteroidsSpawn; i++)
            {
                Point pos = new Point(Width - 50, Rnd.Next(20, Height - 20));
                Point dir = new Point(Rnd.Next(2, 7), Rnd.Next(5, 20));
                int objWidth = Rnd.Next(30, 60);
                Size sz = new Size(objWidth, objWidth);

                _asteroids.Add(new Asteroid(pos, dir, sz));

            }
        }

        /// <summary>
        /// Метод обрабатывающий нажатие 
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Дополнительный класс с аргументами делегата</param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка выстрела - клавиша 'Control'
            Point bulPos = new Point(_ship.Rect.X + _ship.Rect.Size.Width, _ship.Rect.Y + (_ship.Rect.Size.Height / 2));
            Point bulDir = new Point(x: 15, y: 0);
            if (e.KeyCode == Keys.ControlKey)
                _bullets.Add(new Bullet(bulPos, bulDir, new Size(15, 10)));

            // Обработка управления кораблем - клавиши 'Вверх' 'Вниз'
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Метод отвечающий за логику появления аптечки 
        /// </summary>
        private static void MedkitPut()
        {
            if (_medkit == null && Interval == 0)
            {
                Interval = Rnd.Next(MedkitMinInterval, MedkitMaxInterval);

                Size sz = new Size(25, 25);

                Point pos = new Point(sz.Width / 2 + 5, Rnd.Next(sz.Height, Height - (sz.Height / 2)));
                Point dir = new Point(Rnd.Next(10, 20), 0 );

                _medkit = new Medkit(pos, dir, sz);
            } 
            else if (_medkit == null) 
                Interval--;
        }

        /// <summary>
        /// Обновление игрового поля
        /// </summary>
		public static void Update() 
		{
			// Обновляем каждый объект
			foreach (BaseObject obj in _objs) 
				obj.Update();

            // Обновляем снаряд
            foreach (Bullet b in _bullets)
                b.Update();

            // Создание переменной для дальнейшей записи в файл
            string logInfo;

            // Проверяем на сталкновение коробля с аптечкой.
            // Если произошло - убираем аптечку и добавляем энергию караблю
            if (_medkit != null && _medkit.Collision(_ship))
            {
                // Запись лога в файл
                logInfo = $"Ship get medkit for {_medkit.RecoverEnergy} energy at pos (x:{_ship.Rect.X}, y: {_ship.Rect.Y})";
                logEvent?.Invoke(logInfo);

                // Увеличение энергии коробля
                _ship?.EnergyIncrease(_medkit.RecoverEnergy);
                System.Media.SystemSounds.Exclamation.Play();

                _medkit = null;
            }


            // Обновляем каждый астероид
            for (var i = 0; i < _asteroids.Count; i++)
            {
                // Обновляем астероид
                _asteroids[i].Update();

                bool asteroidWasDestroyed = false;
                // Проверяем на сталкновение пули с астероидом.
                // Если произошло - убираем два объекта с поля и переходим на следующую
                // итерацию
                for (int j = 0; j < _bullets.Count; j++)
                {
                    if (_bullets[j].Collision(_asteroids[i]))
                    {
                        // Запись лога в файл
                        logInfo = $"Bullet was destroy asteroid in position and player get {PointsPerAsteroid} points at position (x:{_asteroids[i].Rect.X}, y: {_asteroids[i].Rect.Y}) ";
                        logEvent?.Invoke(logInfo);

                        System.Media.SystemSounds.Hand.Play();
                        _asteroids.RemoveAt(i);
                        asteroidWasDestroyed = true;
                        _bullets.RemoveAt(j);
                        break;
                    }
                }

                if (_asteroids.Count == 0 )
                {
                    InitialAsteroidsSpawn += 1;
                    SpawnAsteroids();

                    // Запись лога в файл
                    logInfo = $"All asteroids was destroyed and spawn another list with {InitialAsteroidsSpawn} asteroids"; 
                    logEvent?.Invoke(logInfo);
                }

                // Если астероид уничтожила пуля 
                if (asteroidWasDestroyed)
                    continue;

                // Обрабатывем столкновение коробля и астероида
                if (!_ship.Collision(_asteroids[i])) continue;

                var rnd = new Random();
                int damage = rnd.Next(1, 10);
                _ship?.EnergyDecrease(damage);

                // Запись лога в файл
                logInfo= $"Ship get damaged for {damage} energy at pos (x:{_ship.Rect.X}, y: {_ship.Rect.Y})";
                logEvent?.Invoke(logInfo);

                System.Media.SystemSounds.Asterisk.Play();
                _asteroids.RemoveAt(i);
                
                // Если энергии после столкновения не осталось - корабль уничтожается
                if (_ship.Energy <= 0)
                {
                    // Запись лога в файл
                    logInfo = $"Ship was destroyed at pos (x:{_ship.Rect.X}, y: {_ship.Rect.Y})";
                    logEvent?.Invoke(logInfo);

                    _ship?.Die();
                }

            }

            // Проверяем размеры экрана
            if (Width > 1000 || Height > 1000)
                throw new ArgumentOutOfRangeException();
		}

        /// <summary>
        /// Отрисовка игрового поля 
        /// </summary>
        public static void Draw()
        {
            // Вызываем отрисовку для каждого объекта
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();

            foreach (Asteroid a in _asteroids)
                a?.Draw();

            foreach (Bullet b in _bullets)
                b.Draw();

            _medkit?.Draw();
            _ship?.Draw();

            // Отрисовка текущей энергии и очков
            if (_ship != null)
            {
                var font = new Font(SystemFonts.DefaultFont.FontFamily, (float)12.0, FontStyle.Bold);
                
                Buffer.Graphics.DrawString($"Energy: {_ship?.Energy}\t Points: {Points}", font, Brushes.White, 0, 0);
            }
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

        private static void AddToLog(string info)
        {
            // Запись в файл
            using (FileStream fstream = new FileStream($"GameLog.txt", FileMode.Append))
            {
                //Добавляем к логу текущее время
                info += $" at time [{DateTime.Now}]\n\n";

                // Преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(info);
                // Запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine($"{info} log был записанзаписан в файл");
            }

        }

        private static void StartGame()
        {
            // Начальный интервал появления аптечки
            Interval = Rnd.Next(MedkitMinInterval, MedkitMaxInterval);

            string logInfo = "\n\nGaming session was started";
            logEvent?.Invoke(logInfo);
        }
    }

}


