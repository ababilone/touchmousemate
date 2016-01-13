using System;

namespace TouchMouseMate.Menu
{
	public class TouchOverStripMenuItem : BaseToolStripMenuItem
	{
		private readonly TouchConfiguration _touchConfiguration;

		public TouchOverStripMenuItem(TouchConfiguration touchConfiguration) : base("Touch-over-click", Properties.Resources.user_desktop)
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