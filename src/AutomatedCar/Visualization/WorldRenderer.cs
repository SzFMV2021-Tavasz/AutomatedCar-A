using AutomatedCar.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BaseModel.Interfaces;
using BaseModel.WorldObjects;
using Point = System.Windows.Point;

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

            var objectsInRange = renderCamera.Filter(World.Renderables);
            foreach (var worldObject in objectsInRange)
            {
                Draw(drawingContext, worldObject);
            }

            Draw(drawingContext, car);
        }

        private void Draw(DrawingContext drawingContext, IRenderableWorldObject worldObject)
        {
            DrawingGroup drawingGroup = new DrawingGroup();

            if (worldObject.Filename == null)
            {
                worldObject.Filename = Enum.GetName((worldObject as WorldObject).ObjectType).ToLower() + ".png";
            }

            BitmapImage bm = getBitMapImageByName(worldObject.Filename);
            worldObject.Width = (int)bm.Width;
            worldObject.Height = (int)bm.Height;

            var relativePos = this.renderCamera.TranslateToViewport(worldObject.X, worldObject.Y);

            var angle = Math.Acos(worldObject.M11);

            drawingGroup.Transform = new RotateTransform(
                angle,
                relativePos.X + worldObject.Width / 2,
                    relativePos.Y + worldObject.Height / 2
            );

            drawingGroup.Children.Add(
                new ImageDrawing(
                    bm,
                    new Rect(relativePos, new Size(worldObject.Width, worldObject.Height)
                    ))
            );

            drawingContext.DrawDrawing(drawingGroup);
        }

        private void SetRenderCameraMiddle(Models.AutomatedCar car)
        {
            Point carMiddleReference = getMiddleReference(car.X, car.Y, car.Width, car.Height);
            this.renderCamera.UpdateMiddlePoint(carMiddleReference.X, carMiddleReference.Y);
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