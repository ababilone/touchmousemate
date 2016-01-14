namespace TouchMouseMate.TouchZone
{
	public class LeftTouchZone : TouchZone
	{
		private readonly StateMachine _stateMachine;

		private static readonly int[,] DefaultMask = {
			{1, 1, 1, 1, 1,   1, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{1, 1, 1, 1, 1,   1, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 1, 1, 1, 1,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 1, 1, 1, 1,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},

			{0, 0, 1, 1, 1,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 1, 1, 1,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 1, 1, 1,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},

			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0}
		};

		public LeftTouchZone(StateMachine stateMachine, TouchConfiguration touchConfiguration) : base("Left", touchConfiguration, DefaultMask)
		{
			_stateMachine = stateMachine;
		}

		protected override void OnKeyUp()
		{
			_stateMachine.Process(MouseEventFlags.LeftUp);
		}

		protected override void OnKeyDown()
		{
			_stateMachine.Process(MouseEventFlags.LeftDown);
		}

		protected override void OnMove(double movement)
		{
			_stateMachine.Process(MouseEventFlags.Move);
		}
	}
}