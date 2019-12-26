using System;
using System.Windows.Forms;

namespace MyGame 
{
    /// <summary>
    /// Главный класс с которого начинается приложение
    /// </summary>
	class Program 
	{
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args"></param>
		static void Main(string[] args) 
		{
			Form form = new Form()
			{
				Width = Screen.PrimaryScreen.Bounds.Width,
				Height = Screen.PrimaryScreen.Bounds.Height
				
			};
			form.Width = 1000;
			form.Height = 800;
			Game.Init(form);
			form.Show();
			Game.Draw();
			Application.Run(form);
		}  
	}  
}