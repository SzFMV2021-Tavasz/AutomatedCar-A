using AutomatedCar.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        private readonly DispatcherTimer renderTimer = new DispatcherTimer();

        public World World => World.Instance;

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (World == null || CarImage == null)
                return;

            foreach (var worldObject in World.WorldObjects)
            {
                // worldObject.Render(drawingContext);
            }

            var car = World.ControlledCar;
            drawingContext.DrawImage(CarImage, new Rect(car.X, car.Y, car.Width, car.Height));
        }

        public WorldRenderer()
        {
            renderTimer.Interval = TimeSpan.FromMilliseconds(20);
            renderTimer.Tick += Timer_Tick;
            renderTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        private BitmapImage _carImage;

        public BitmapImage CarImage
        {
            get
            {
                if (_carImage == null)
                {
                    _carImage = new BitmapImage();
                    _carImage.BeginInit();
                    _carImage.UriSource = new System.Uri($"{Directory.GetCurrentDirectory()}/Assets/WorldObjects/{World.ControlledCar.Filename}", System.UriKind.Absolute);
                    _carImage.EndInit();
                }

                return _carImage;
            }
        }
    }
}