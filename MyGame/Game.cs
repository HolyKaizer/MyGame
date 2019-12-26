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
		/// 
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

			_asteroids = new Asteroid[3];
			Random rnd = new Random();

            //Заполнение массива -objs[] звездами со случайной скоростью, позицией и размером
			for (int i = 0; i < _objs.Length; i++)
			{
				Point pos = new Point(rnd.Next(20, Width - 20), rnd.Next(20, Height - 20));
				Point dir = new Point(rnd.Next(1, 14), 0);
				int objWidth = rnd.Next(1, 10);
				Size sz = new Size(objWidth, objWidth);

				_objs[i] = new Star(pos, dir, sz);

			}

			//Заполнение массива _asteroids[] астероидами со случайной скоростью, позицией и размером
			for (int i = 0; i < _asteroids.Length; i++)
			{
				Point pos = new Point(1000, rnd.Next(20, Height - 20));
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

			Timer timer = new Timer {Interval = 75};
			timer.Start();
			timer.Tick += Timer_Tick;

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
        /// Отрисовка игрового поля 
        /// </summary>
		public static void Draw() 
		{
			//Вызываем отрисовку для каждого объекта
			Buffer.Graphics.Clear(Color.Black);
			foreach (BaseObject obj in _objs) 
				obj.Draw();
			foreach (Asteroid obj in _asteroids)
				obj.Draw();
			_bullet.Draw();
			Buffer.Render();
		}

        /// <summary>
        /// Обновление игрового поля
        /// </summary>
		public static void Update() 
		{
			//Обновляем каждый объект
			foreach (BaseObject obj in _objs) 
				obj.Update(); 

			foreach (Asteroid a in _asteroids)
			{
				a.Update();
				if(a.Collision(_bullet)) { System.Media.SystemSounds.Hand.Play(); }
			}

			_bullet.Update();
		}
	}

}


