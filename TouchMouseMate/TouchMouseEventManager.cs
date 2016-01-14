using System;
using Microsoft.Research.TouchMouseSensor;

namespace TouchMouseMate
{
	public class TouchMouseEventManager : IDisposable
	{
		private readonly StateMachine _machine;
		private readonly TouchZoneProvider _touchZoneProvider;

		public TouchMouseEventManager(StateMachine stateMachine, TouchZoneProvider touchZoneProvider)
		{
			_touchZoneProvider = touchZoneProvider;
			_machine = stateMachine;
		}

		private readonly bool[,] _touchMap = new bool[15, 13];
		private readonly int[] _pixelFound = { 0, 0 };

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

			for (var y = 0; y < pTouchMouseStatus.m_dwImageHeight; y++)
			{
				for (var x = 0; x < pTouchMouseStatus.m_dwImageWidth; x++)
				{
					var pixel = pabImage[pTouchMouseStatus.m_dwImageWidth * y + x];
					if (pixel != 0)
					{
						_touchMap[x, y] = true;
						if (pixel == 0)
						{
							continue;
						}

						foreach (var touchZone in _touchZoneProvider.Get())
							touchZone.Consume(x, y, pixel);
					}
				}
			}

			// Calculate and display the center of mass for the touches present.
			foreach (var touchZone in _touchZoneProvider.Get())
			{
				touchZone.AppendTime(pTouchMouseStatus.m_dwTimeDelta);
				touchZone.ComputeCenter();
			}

			if (pTouchMouseStatus.m_dwTimeDelta == 0)
			{
				// If the time delta is zero then there has been an 
				// undetermined delta since the last report.
				Log.Info("New touch detected");
				_machine.Idle();

				foreach (var touchZone in _touchZoneProvider.Get())
				{
					touchZone.Reset();
				}
			}

			foreach (var touchZone in _touchZoneProvider.Get())
			{
				touchZone.DetectEvents();
			}

			foreach (var touchZone in _touchZoneProvider.Get())
			{
				touchZone.Prepare();
			}

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