using System;

namespace TouchMouseMate.Menu
{
	class MouseMapStripMenuItem : BaseToolStripMenuItem
	{
		private readonly TouchMouseEventManager _touchMouseEventManager;
		private MouseMapWindow _mouseMapWindow;

		public MouseMapStripMenuItem(TouchMouseEventManager touchMouseEventManager) : base("Show mouse map", Properties.Resources.mouse.ToBitmap())
		{
			_touchMouseEventManager = touchMouseEventManager;
		}

		protected override void Clicked(EventArgs args)
		{
			if (_mouseMapWindow == null)
				_mouseMapWindow = new MouseMapWindow(_touchMouseEventManager);

			_mouseMapWindow.Show();
		}
	}
}
