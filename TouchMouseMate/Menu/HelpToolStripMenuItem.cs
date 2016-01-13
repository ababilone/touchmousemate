using System;
using System.Windows.Forms;

namespace TouchMouseMate.Menu
{
	public class HelpToolStripMenuItem : BaseToolStripMenuItem
	{
		public HelpToolStripMenuItem() : base("Help", Properties.Resources.help_browser)
		{
			
		}

		protected override void Clicked(EventArgs args)
		{
			Help.ShowHelp(null, @"http://touchmousemate.codeplex.com");
		}
	}
}