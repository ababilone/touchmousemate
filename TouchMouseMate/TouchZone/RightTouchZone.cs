namespace TouchMouseMate.TouchZone
{
	public class RightTouchZone : TouchZone
	{
		private readonly StateMachine _stateMachine;

		private static readonly int[,] DefaultMask = {
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 1,   1, 1, 1, 1, 1},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 1,   1, 1, 1, 1, 1},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   1, 1, 1, 1, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   1, 1, 1, 1, 0},

			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   1, 1, 1, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   1, 1, 1, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   1, 1, 1, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},

			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0,   0, 0, 0, 0, 0,   0, 0, 0, 0, 0}
		};

		public RightTouchZone(StateMachine stateMachine, TouchConfiguration touchConfiguration) : base("Right", touchConfiguration, DefaultMask)
		{
			_stateMachine = stateMachine;
		}

		protected override void OnKeyUp()
		{
			_stateMachine.Process(MouseEventFlags.RightUp);
		}

		protected override void OnKeyDown()
		{
			_stateMachine.Process(MouseEventFlags.RightDown);

		}

		protected override void OnMove(double movement)
		{
			_stateMachine.Process(MouseEventFlags.Move);
		}
	}
}