using AutomatedCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomatedCar.Visualization
{
    public class RenderCamera
    {
        public double LeftX { get; set; }
        public double TopY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Rect ViewportRect => new Rect(LeftX, TopY, Width, Height);

        public void UpdateMiddlePoint(double originX, double originY)
        {
            LeftX = originX - (Width / 2.0);
            TopY = originY + (Height / 2.0);
        }

        public List<IRenderableWorldObject> Filter(List<IRenderableWorldObject> renderables)
        {
            var visible = new List<IRenderableWorldObject>();

            foreach (var renderable in renderables)
            {
                if (IsVisibleInViewport(renderable))
                    visible.Add(renderable);
            }

            return visible;
        }

        public bool IsVisibleInViewport(IRenderableWorldObject renderable)
        {
            return ViewportRect.IntersectsWith(renderable.Boundary);
        }
    }
}
