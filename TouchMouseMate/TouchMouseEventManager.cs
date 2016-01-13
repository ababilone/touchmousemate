using System;
using Microsoft.Research.TouchMouseSensor;

namespace TouchMouseMate
{
	public class TouchMouseEventManager : IDisposable
	{
		private readonly StateMachine _machine;
		private readonly TouchConfiguration _touchConfiguration;

		public TouchMouseEventManager(StateMachine stateMachine, TouchConfiguration touchConfiguration)
		{
			_touchConfiguration = touchConfiguration;
			_machine = stateMachine;

			_leftZone = new TouchZone(_touchConfiguration);
			_rightZone = new TouchZone(_touchConfiguration);
		}



		private readonly bool[,] _touchMap = new bool[15, 13];
		private readonly int[] _pixelFound = { 0, 0 };
		private readonly TouchZone _leftZone;
		private readonly TouchZone _rightZone;

		private readonly int[,] _leftMask = {
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

		private readonly int[,] _rightMask = {
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

		public event TouchMouseEventHandler TouchMouseEvent;

		private TouchMouseCallback _callback;

		public void Init()
		{
			_callback = new TouchMouseCallback(TouchMouseCallback);
			TouchMouseSensorInterop.RegisterTouchMouseCallback(_callback);
		}

		/// <summary>
		/// Function receiving callback from mouse.
		/// </summary>
		/// <param name="pTouchMouseStatus">Values indicating status of mouse.</param>
		/// <param name="pabImage">Bytes forming image, 13 rows of 15 columns.</param>
		/// <param name="dwImageSize">Size of image, assumed to always be 195 (13x15).</param>
		internal void TouchMouseCallback(ref TOUCHMOUSESTATUS pTouchMouseStatus, byte[] pabImage, int dwImageSize)
		{
			// Reinizialize Touched Zones
			_pixelFound[0] = 0;
			_pixelFound[1] = 0;
			// Iterate over rows.
			for (int y = 0; y < pTouchMouseStatus.m_dwImageHeight; y++)
			{
				// Iterate over columns.
				for (int x = 0; x < pTouchMouseStatus.m_dwImageWidth; x++)
				{
					if (pabImage[pTouchMouseStatus.m_dwImageWidth * y + x] != 0)
					// analyze captured image for touch gesture.
					{
						_touchMap[x, y] = true; // touch detected
						int pixel = pabImage[pTouchMouseStatus.m_dwImageWidth * y + x]; // touch strength recorded
						if (pixel == 0)
						{
							continue;
						}

						// Get the pixel value at current position.
						if (_leftMask[y, x] == 1)
						{
							// Increment values.
							_leftZone.Consume(x, y, pixel);
						}
						else if (_rightMask[y, x] == 1)
						{
							// Increment values.
							_rightZone.Consume(x, y, pixel);
						}
					}
				}
			}

			// Calculate and display the center of mass for the touches present.
			_leftZone.AppendTime(pTouchMouseStatus.m_dwTimeDelta);
			_rightZone.AppendTime(pTouchMouseStatus.m_dwTimeDelta);
			_leftZone.ComputeCenter();
			_rightZone.ComputeCenter();

			if (pTouchMouseStatus.m_dwTimeDelta == 0)
			{
				// If the time delta is zero then there has been an 
				// undetermined delta since the last report.
				Log.Info("New touch detected");
				_machine.Idle();
				_leftZone.Reset();
				_rightZone.Reset();
			}

			ApproachThree();

			if (pTouchMouseStatus.m_fDisconnect)
			{
				// The mouse is now disconnected, if we had created objects to track 
				// the mouse they would be destroyed here.
				Log.InfoFormat("\nMouse #{0:X4}: Disconnected\n",
					(pTouchMouseStatus.m_dwID & 0xFFFF));
			}

			OnTouchMouseEvent(new TouchMouseEventArgs
			{
				Image = pabImage,
				ImageSize = dwImageSize,
				Status = pTouchMouseStatus
			});
		}


		private void ApproachThree()
		{
			if (_leftZone.KeyUpDetected)
			{
				_machine.Process(MouseEventFlags.LeftUp);
				_leftZone.Movement = 0F;
			}

			if (_rightZone.KeyUpDetected)
			{
				_machine.Process(MouseEventFlags.RightUp);
				_rightZone.Movement = 0F;
			}

			if (_leftZone.KeyDownDetected)
			{
				_machine.Process(MouseEventFlags.LeftDown);
			}

			if (_rightZone.KeyDownDetected)
			{
				_machine.Process(MouseEventFlags.RightDown);
			}

			if (_leftZone.MoveDetected)
			{
				Log.DebugFormat("left move {0}", _leftZone.Movement);
				if (_touchConfiguration.Section.MoveDetect)
				{
					_machine.Process(MouseEventFlags.Move);
				}

				_leftZone.Movement = 0F;
			}

			if (_rightZone.MoveDetected)
			{
				Log.DebugFormat("right move {0}", _rightZone.Movement);
				if (_touchConfiguration.Section.MoveDetect)
				{
					_machine.Process(MouseEventFlags.Move);
				}

				_rightZone.Movement = 0F;
			}

			_leftZone.Prepare();
			_rightZone.Prepare();
		}

		public void Dispose()
		{
			TouchMouseSensorInterop.UnregisterTouchMouseCallback();
		}

		protected virtual void OnTouchMouseEvent(TouchMouseEventArgs touchmouseeventargs)
		{
			TouchMouseEvent?.Invoke(this, touchmouseeventargs);
		}
	}
}