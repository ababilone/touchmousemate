using System;
using System.Windows.Forms;

namespace TouchMouseMate.Menu
{
	public class ExitStripMenuItem : BaseToolStripMenuItem
	{
		private readonly NotifyIcon _icon;

		public ExitStripMenuItem(NotifyIcon icon) : base("Exit", Properties.Resources.system_log_out)
		{
			_icon = icon;
		}

		protected override void Clicked(EventArgs args)
		{
			_icon.Visible = false;
			Application.Exit();
		}
	}
}