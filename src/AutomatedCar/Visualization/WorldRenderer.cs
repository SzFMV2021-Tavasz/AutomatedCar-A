using AutomatedCar.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        public readonly double tickRate = 20;
        private readonly DispatcherTimer renderTimer = new DispatcherTimer();
        private readonly RenderCamera renderCamera = new RenderCamera();

        public World World => World.Instance;

        public WorldRenderer()
        {
            Loaded += WorldRenderer_Loaded;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (World == null)
            {
                return;
            }

            var car = GetAutomatedCar();

            SetRenderCameraMiddle(car);
            Render_Car(drawingContext, car);

            foreach (var worldObject in World.Renderables)
            {
                // worldObject.Render(drawingContext);
            }
        }

        private void SetRenderCameraMiddle(Models.AutomatedCar car)
        {
            Point carMiddleReference = getMiddleReference(car.X, car.Y, car.Width, car.Height);
            this.renderCamera.UpdateMiddlePoint(carMiddleReference.X, carMiddleReference.Y);
        }

        private void Render_Car(DrawingContext drawingContext, Models.AutomatedCar car)
        {
            var carImage = getBitMapImageByName(car.Filename);

            Point carPointOnCanvas = this.renderCamera.TranslateToViewport(car.X , car.Y );

            drawingContext.DrawImage(carImage, new Rect(carPointOnCanvas.X, carPointOnCanvas.Y, car.Width, car.Height));
        }

        private BitmapImage getBitMapImageByName(string filename)
        {
            return WorldObjectTransformer.GetCachedImage(filename);
        }

        private AutomatedCar.Models.AutomatedCar GetAutomatedCar()
        {
            return World.ControlledCar;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        private void WorldRenderer_Loaded(object sender, RoutedEventArgs e)
        {
            renderTimer.Interval = TimeSpan.FromMilliseconds(tickRate);
            renderTimer.Tick += Timer_Tick;
            renderTimer.Start();

            renderCamera.Width = ActualWidth;
            renderCamera.Height = ActualHeight;
        }

        private Point getMiddleReference(Double x, Double y, Double width, Double height)
        {
            return new Point(x + (width / 2), y + (height / 2));
        }
    }
}