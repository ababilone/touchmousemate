using Microsoft.Research.TouchMouseSensor;

namespace TouchMouseMate
{
	public class TouchMouseEventArgs
	{
		public TOUCHMOUSESTATUS Status { get; set; }
		public byte[] Image { get; set; }
		public int ImageSize { get; set; }
	}
}