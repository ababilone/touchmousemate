using System;

namespace TouchMouseMate.Menu
{
	class TouchZoneConfigurationStripMenuItem : BaseToolStripMenuItem
	{
		private readonly TouchMouseEventManager _touchMouseEventManager;
		private readonly TouchZoneProvider _touchZoneProvider;
		private TouchZonesConfigurationWindow _touchZonesConfigurationWindow;

		public TouchZoneConfigurationStripMenuItem(TouchMouseEventManager touchMouseEventManager, TouchZoneProvider touchZoneProvider) : base("Touch Zone Configuration", Properties.Resources.mouse.ToBitmap())
		{
			_touchMouseEventManager = touchMouseEventManager;
			_touchZoneProvider = touchZoneProvider;
		}

		protected override void Clicked(EventArgs args)
		{
			if (_touchZonesConfigurationWindow == null)
				_touchZonesConfigurationWindow = new TouchZonesConfigurationWindow(_touchMouseEventManager, _touchZoneProvider);

			_touchZonesConfigurationWindow.Show();
		}
	}
}