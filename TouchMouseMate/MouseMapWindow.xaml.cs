using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TouchMouseMate
{
	public partial class MouseMapWindow
	{
		private readonly TouchMouseEventManager _touchMouseEventManager;

		public MouseMapWindow()
		{
			InitializeComponent();
		}

		public MouseMapWindow(TouchMouseEventManager touchMouseEventManager) : this()
		{
			_touchMouseEventManager = touchMouseEventManager;

			Loaded += MainWindow_Loaded;
			Closed += MainWindow_Closed;
		}

		private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			_touchMouseEventManager.TouchMouseEvent += TouchMouseEventManagerOnTouchMouseEvent;

		}

		private void MainWindow_Closed(object sender, System.EventArgs e)
		{
			_touchMouseEventManager.TouchMouseEvent -= TouchMouseEventManagerOnTouchMouseEvent;
		}

		private void TouchMouseEventManagerOnTouchMouseEvent(object sender, TouchMouseEventArgs touchMouseEventArgs)
		{
			var imageWidth = touchMouseEventArgs.Status.m_dwImageWidth;
			var imageHeight = touchMouseEventArgs.Status.m_dwImageHeight;
			var image = touchMouseEventArgs.Image;
			var imageSize = touchMouseEventArgs.ImageSize;

			Dispatcher.BeginInvoke((Action)(() => { SensorImage.Source = BitmapSource.Create(imageWidth, imageHeight, 96, 96, PixelFormats.Gray8, null, image, imageWidth); }));
		}
	}
}