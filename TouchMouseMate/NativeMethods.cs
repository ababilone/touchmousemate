using System.Runtime.InteropServices;

namespace TouchMouseMate
{
	public class NativeMethods
	{
		[DllImport("user32.dll", EntryPoint = "SetCursorPos")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetCursorPos(out MousePoint lpMousePoint);

		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		public  void SetCursorPosition(int x, int y)
		{
			SetCursorPos(x, y);
		}

		public static MousePoint GetCursorPosition()
		{
			MousePoint currentMousePoint;
			var gotPoint = GetCursorPos(out currentMousePoint);
			if (!gotPoint)
			{
				currentMousePoint = new MousePoint(0, 0);
			}

			return currentMousePoint;
		}

		public static void MouseEvent(MouseEventFlags value)
		{
			var position = GetCursorPosition();
			mouse_event((int)value, position.X, position.Y, 0, 0);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MousePoint
		{
			public int X;
			public int Y;

			public MousePoint(int x, int y)
			{
				X = x;
				Y = y;
			}
		}
	}
}