using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouchMouseMate.TouchZone
{
	public abstract class TouchZone : INotifyPropertyChanged
	{
		private readonly TouchConfiguration _touchConfiguration;
		private TouchPoint _current;
		private TouchPoint _previous;
		private TouchPoint _previousPrevious;
		private int _time;
		internal double Movement;
		private bool _isPressed;

		protected TouchZone(string name, TouchConfiguration touchConfiguration, int[,] mask)
		{
			Name = name;
			_touchConfiguration = touchConfiguration;
			Mask = mask;
			_current = new TouchPoint();
		}

		public bool IsInZone(int x, int y)
		{
			return Mask[y, x] == 1;
		}

		public void Consume(int x, int y, int pixel)
		{
			if (IsInZone(x, y))
				_current.Consume(x, y, pixel);
		}

		public void AppendTime(int mDwTimeDelta)
		{
			_time += mDwTimeDelta;
		}

		public void ComputeCenter()
		{
			if (_current.Pixel > 0)
			{
				_current.ComputeCenter();
				if (_previous.Pixel > 0)
				{
					var dX = _current.CenterX - _previous.CenterX;
					var dY = _current.CenterY - _previous.CenterY;
					Movement += Math.Sqrt(dX * dX + dY * dY);
				}
			}
		}

		public bool KeyUpDetected => _current.Pixel == 0 && _previous.Pixel > 0;

		public bool KeyDownDetected => _previous.Pixel > 0 && _previousPrevious.Pixel == 0;

		public bool MoveDetected => Movement > _touchConfiguration.Section.MoveThreshold;

		protected abstract void OnKeyUp();
		protected abstract void OnKeyDown();
		protected abstract void OnMove(double movement);

		public void DetectEvents()
		{
			if (KeyUpDetected)
			{
				IsPressed = false;
				OnKeyUp();
				
				Movement = 0F;
			}

			if (KeyDownDetected)
			{
				IsPressed = true;
				OnKeyDown();
			}

			if (MoveDetected)
			{
				if (_touchConfiguration.Section.MoveDetect)
				{
					Log.DebugFormat("left move {0}", Movement);
					OnMove(Movement);
				}

				Movement = 0F;
			}
		}

		public bool IsPressed
		{
			get { return _isPressed; }
			set { _isPressed = value; OnPropertyChanged(); }
		}

		public int[,] Mask { get; set; }

		public string Name { get; }

		public void Prepare()
		{
			_previousPrevious = _previous;
			_previous = _current;
			_current.Reset();
		}

		public void Reset()
		{
			_time = 0;
			Movement = 0F;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}