using System;
using System.Windows.Forms;

namespace MyGame 
{
	class Program 
	{
		static void Main(string[] args) 
		{ 
			Form form = new Form();
			form.Width = 1200;
			form.Height = 800;
			Game.Init(form);
			form.Show();
			Game.Draw();
			Application.Run(form);
		}  
	}  
}