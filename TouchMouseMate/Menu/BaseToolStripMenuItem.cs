using System;
using System.Drawing;
using System.Windows.Forms;

namespace TouchMouseMate.Menu
{
	public abstract class BaseToolStripMenuItem : ToolStripMenuItem
	{
		protected BaseToolStripMenuItem(string text, Image image) : base(text, image, Clicked)
		{
		}

		private static void Clicked(object sender, EventArgs args)
		{
			var baseToolStripMenuItem = sender as BaseToolStripMenuItem;
			baseToolStripMenuItem?.Clicked(args);
		}

		protected abstract void Clicked(EventArgs args);
	}
}