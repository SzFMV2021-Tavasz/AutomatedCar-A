using AutomatedCar.Models;
using AutomatedCar.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        public BitmapImage CarSprite { get; set; }

        public World World { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (World == null) 
                return;

            foreach (var worldObject in World.WorldObjects)
            {
                //worldObject.Render(drawingContext);
            }

            var car = World.ControlledCar;
            drawingContext.DrawImage(CarSprite, new Rect(car.X, car.Y, car.Width, car.Height));

            base.OnRender(drawingContext);
        }

        public WorldRenderer()
        {
            Loaded += WorldRenderer_Loaded;
        }

        private void WorldRenderer_Loaded(object sender, RoutedEventArgs e)
        {
            this.World = World.Instance;

            //CarSprite = new BitmapImage();
            //CarSprite.BeginInit();
            //CarSprite.UriSource = new System.Uri($"{Directory.GetCurrentDirectory()}/Assets/WorldObjects/{World.ControlledCar.Filename}", System.UriKind.Absolute);
            //CarSprite.EndInit();
        }
    }
}