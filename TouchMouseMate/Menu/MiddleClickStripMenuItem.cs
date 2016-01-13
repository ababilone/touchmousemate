using System;

namespace TouchMouseMate.Menu
{
	public class MiddleClickStripMenuItem : BaseToolStripMenuItem
	{
		private readonly TouchConfiguration _touchConfiguration;

		public MiddleClickStripMenuItem(TouchConfiguration touchConfiguration) : base("Middle-click", Properties.Resources.start_here)
		{
			_touchConfiguration = touchConfiguration;
		}

		protected override void Clicked(EventArgs args)
		{
			_touchConfiguration.Section.TouchOverClick = Checked;
			_touchConfiguration.Config.Save();
		}
	}
}