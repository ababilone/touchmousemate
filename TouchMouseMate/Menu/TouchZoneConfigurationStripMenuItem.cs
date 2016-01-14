using System;

namespace TouchMouseMate.Menu
{
	class TouchZoneConfigurationStripMenuItem : BaseToolStripMenuItem
	{
		private readonly TouchMouseEventManager _touchMouseEventManager;
		private readonly TouchZone.TouchZone[] _touchZones;
		private TouchZonesConfigurationWindow _touchZonesConfigurationWindow;

		public TouchZoneConfigurationStripMenuItem(TouchMouseEventManager touchMouseEventManager, params TouchZone.TouchZone[] touchZones) : base("Touch Zone Configuration", Properties.Resources.mouse.ToBitmap())
		{
			_touchMouseEventManager = touchMouseEventManager;
			_touchZones = touchZones;
		}

		protected override void Clicked(EventArgs args)
		{
			if (_touchZonesConfigurationWindow == null)
				_touchZonesConfigurationWindow = new TouchZonesConfigurationWindow(_touchMouseEventManager, _touchZones);

			_touchZonesConfigurationWindow.Show();
		}
	}
}