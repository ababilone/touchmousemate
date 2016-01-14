using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TouchMouseMate
{
	public partial class TouchZonesConfigurationWindow
	{
		private readonly TouchZone.TouchZone[] _touchZones;
		private readonly TouchMouseEventManager _touchMouseEventManager;

		public static readonly DependencyProperty TouchImageWidthProperty = DependencyProperty.Register("TouchImageWidth", typeof(int), typeof(TouchZonesConfigurationWindow), new PropertyMetadata(default(int)));
		public static readonly DependencyProperty TouchImageHeightProperty = DependencyProperty.Register("TouchImageHeight", typeof(int), typeof(TouchZonesConfigurationWindow), new PropertyMetadata(default(int)));

		public TouchZonesConfigurationWindow()
		{
			InitializeComponent();
		}

		public TouchZonesConfigurationWindow(TouchMouseEventManager touchMouseEventManager, params TouchZone.TouchZone[] touchZones) : this()
		{
			_touchMouseEventManager = touchMouseEventManager;
			_touchZones = touchZones;

			Loaded += MainWindow_Loaded;
			Closed += MainWindow_Closed;
		}

		public int TouchImageWidth
		{
			get { return (int)GetValue(TouchImageWidthProperty); }
			set { SetValue(TouchImageWidthProperty, value); }
		}

		public int TouchImageHeight
		{
			get { return (int)GetValue(TouchImageHeightProperty); }
			set { SetValue(TouchImageHeightProperty, value); }
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

			Dispatcher.BeginInvoke((Action) (() =>
			{
				InitializeGrid(imageWidth, imageHeight, image);
				UpdateGrid(image);
			}));

			//Dispatcher.BeginInvoke((Action)(() => { SensorImage.Source = BitmapSource.Create(imageWidth, imageHeight, 96, 96, PixelFormats.Gray8, null, image, imageWidth); }));
		}

		private void UpdateGrid(byte[] image)
		{
			for (var i = 0; i < image.Length; i++)
			{
				var currentGrid = UniformGrid.Children[i] as Grid;
				if (currentGrid != null)
				{
					currentGrid.Background = new SolidColorBrush(image[i] > 0 ? Colors.Red : Colors.GreenYellow);

					var textBlock = currentGrid.Children[0] as TextBlock;
					if (textBlock != null)
						textBlock.Text = "";
				}
			}
		}

		private void InitializeGrid(int imageWidth, int imageHeight, byte[] image)
		{
			if (TouchImageWidth == imageWidth && TouchImageHeight == imageHeight)
				return;

			TouchImageWidth = imageWidth;
			TouchImageHeight = imageHeight;

			UniformGrid.Children.Clear();
			for (var i = 0; i < image.Length; i++)
			{
				var grid = new Grid();
				grid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center });
				UniformGrid.Children.Add(grid);
			}
		}
	}
}