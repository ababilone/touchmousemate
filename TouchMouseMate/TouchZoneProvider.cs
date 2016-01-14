using System.Collections.Generic;
using TouchMouseMate.TouchZone;

namespace TouchMouseMate
{
	public class TouchZoneProvider
	{
		private readonly List<TouchZone.TouchZone> _touchZones;

		public TouchZoneProvider(StateMachine stateMachine, TouchConfiguration touchConfiguration)
		{
			_touchZones = new List<TouchZone.TouchZone>
			{
				new LeftTouchZone(stateMachine, touchConfiguration),
				new RightTouchZone(stateMachine, touchConfiguration)
			};
		}

		public ICollection<TouchZone.TouchZone> Get()
		{
			return _touchZones;
		}
	}
}