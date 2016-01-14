using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TouchMouseMate
{
	public partial class TouchZonesConfigurationWindow
	{
		private readonly TouchMouseEventManager _touchMouseEventManager;

		public static readonly DependencyProperty TouchImageWidthProperty = DependencyProperty.Register("TouchImageWidth", typeof(int), typeof(TouchZonesConfigurationWindow), new PropertyMetadata(default(int)));
		public static readonly DependencyProperty TouchImageHeightProperty = DependencyProperty.Register("TouchImageHeight", typeof(int), typeof(TouchZonesConfigurationWindow), new PropertyMetadata(default(int)));
		public static readonly DependencyProperty TouchZonesProperty = DependencyProperty.Register("TouchZones", typeof(IEnumerable<TouchZone.TouchZone>), typeof(TouchZonesConfigurationWindow), new PropertyMetadata(null));


		public TouchZonesConfigurationWindow()
		{
			InitializeComponent();
		}

		public TouchZonesConfigurationWindow(TouchMouseEventManager touchMouseEventManager, TouchZoneProvider touchZoneProvider) : this()
		{
			_touchMouseEventManager = touchMouseEventManager;
			TouchZones = touchZoneProvider.Get();

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

		public IEnumerable<TouchZone.TouchZone> TouchZones
		{
			get { return (IEnumerable<TouchZone.TouchZone>)GetValue(TouchZonesProperty); }
			set { SetValue(TouchZonesProperty, value); }
		}


		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_touchMouseEventManager.TouchMouseEvent += TouchMouseEventManagerOnTouchMouseEvent;
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			_touchMouseEventManager.TouchMouseEvent -= TouchMouseEventManagerOnTouchMouseEvent;
		}

		private void TouchMouseEventManagerOnTouchMouseEvent(object sender, TouchMouseEventArgs touchMouseEventArgs)
		{
			var imageWidth = touchMouseEventArgs.Status.m_dwImageWidth;
			var imageHeight = touchMouseEventArgs.Status.m_dwImageHeight;
			var image = touchMouseEventArgs.Image;
			var imageSize = touchMouseEventArgs.ImageSize;

			Dispatcher.BeginInvoke((Action)(() =>
		   {
			   InitializeGrid(image, imageWidth, imageHeight);
			   UpdateZones(image, imageWidth, imageHeight);
			   UpdateGrid(image);
		   }));

			//Dispatcher.BeginInvoke((Action)(() => { SensorImage.Source = BitmapSource.Create(imageWidth, imageHeight, 96, 96, PixelFormats.Gray8, null, image, imageWidth); }));
		}

		private void UpdateZones(byte[] image, int imageWidth, int imageHeight)
		{
			for (var i = 0; i < image.Length; i++)
			{
				var currentGrid = UniformGrid.Children[i] as Grid;
				var textBlock = currentGrid?.Children[0] as TextBlock;
				if (textBlock != null)
				{
					var y = i / imageWidth;
					var x = i - y * imageWidth;

					textBlock.Text = "";

					foreach (var touchZone in TouchZones)
					{
						if (touchZone.IsInZone(x, y))
						{
							textBlock.Text += touchZone.Name.FirstOrDefault();
						}
					}
				}
			}
		}


		private void UpdateGrid(byte[] image)
		{
			for (var i = 0; i < image.Length; i++)
			{
				var currentGrid = UniformGrid.Children[i] as Grid;
				if (currentGrid != null)
				{
					currentGrid.Background = new SolidColorBrush(image[i] > 0 ? Colors.Red : Colors.GreenYellow);
				}
			}
		}

		private void InitializeGrid(byte[] image, int imageWidth, int imageHeight)
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