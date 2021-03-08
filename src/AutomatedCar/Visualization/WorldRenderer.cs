using AutomatedCar.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Pen = System.Windows.Media.Pen;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        public readonly double tickRate = 20;
        private readonly DispatcherTimer renderTimer = new DispatcherTimer();
        private readonly RenderCamera renderCamera = new RenderCamera();

        public World World => World.Instance;

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (World == null)
                return;
            
            foreach (var worldObject in World.WorldObjects)
            {
                Draw(drawingContext, worldObject);
            }
        }

        private void Draw(DrawingContext drawingContext, IRenderableWorldObject worldObject)
        {
            var image = WorldObjectTransformer.GetCachedImage(worldObject.Filename);
            var rect = new Rect(worldObject.X, worldObject.Y, worldObject.Width, worldObject.Height);

            var transformMatrix = new Matrix(worldObject.M11, worldObject.M12, worldObject.M21, worldObject.M22, 0, 0);
            var tb = new TransformedBitmap(image, new MatrixTransform(transformMatrix));
            rect.Transform(transformMatrix);

            drawingContext.DrawImage(tb, rect);
        }

        public WorldRenderer()
        {
            Loaded += WorldRenderer_Loaded;
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
    }
}