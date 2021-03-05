using AutomatedCar.Models;
using AutomatedCar.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomatedCar.Rendering
{
    internal class WorldRenderer : FrameworkElement
    {
        public CourseDisplayViewModel ViewModel { get; set; }

        public ImageSource CarImage { get; set; }

        public ObservableCollection<RenderableWorldObject> renderableWorldObjects => ViewModel.World.WorldObjects;

        protected override void OnRender(DrawingContext drawingContext)
        {
            foreach (var worldObject in renderableWorldObjects)
            {
                //worldObject.Render(drawingContext);
            }

            base.OnRender(drawingContext);
        }
    }
}