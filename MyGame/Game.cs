using System;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame 
{ 
	static class Game 
	{ 
		private static BufferedGraphicsContext _context;
		public static BufferedGraphics Buffer;
		//Свойства 

		//Ширина и высота игрового поля 
		public static int Width { get; set; } 
		public static int Height { get; set; }

		private static BaseObject[] _objs;


		static Game() 
		{

		}

		//Загру
		private static void Load() 
		{ 
			_objs = new BaseObject[30];
			Random rnd = new Random();
			for (int i = 0; i < _objs.Length; i++)
			{
				Point pos = new Point(rnd.Next(20, Width - 20), rnd.Next(20, Height - 20));
				Point dir = new Point(rnd.Next(2, 20), 0);
				int objWidth = rnd.Next(1, 10);
				Size sz = new Size(objWidth, objWidth);

				_objs[i] = new Star(pos, dir, sz);

			}

			//for(int i = _objs.Length / 2; i < _objs.Length; i++) 
			//	_objs[i] = new Star(new Point(600, i * 20), 
			//							  new Point(35 - i, 35 - i),
			//							  new Size(10, 10)) ;
		}

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

			Timer timer = new Timer {Interval = 100};
			timer.Start();
			timer.Tick += Timer_Tick;

		}

		private static void Timer_Tick(object sender, EventArgs e) 
		{
			//Вызывает рендер методы каждые 100 милисекунд  
			Draw();
			Update(); 
		}

		public static void Draw() 
		{
			//Проверяем вывод графики 

			//Buffer.Graphics.Clear(Color.Black);
			//Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
			//Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
			//Buffer.Render(); 

			//Вызываем отрисовку для каждого объекта
			Buffer.Graphics.Clear(Color.Black);
			foreach (BaseObject obj in _objs) 
				obj.Draw();
			Buffer.Render();
		}

		public static void Update() 
		{
			//Обновляем каждый объект
			foreach (BaseObject obj in _objs) 
				obj.Update(); 
		}
	}

}


