using System;

namespace TouchMouseMate.Menu
{
	public class LeftHandedStripMenuItem : BaseToolStripMenuItem
	{
		private readonly TouchConfiguration _touchConfiguration;

		public LeftHandedStripMenuItem(TouchConfiguration touchConfiguration) : base("Left-handed", Properties.Resources.input_mouse)
		{
			_touchConfiguration = touchConfiguration;
			CheckOnClick = true;
		}

		protected override void Clicked(EventArgs args)
		{
			_touchConfiguration.Section.LeftHandMode = Checked;
			_touchConfiguration.Config.Save();
		}
	}
}