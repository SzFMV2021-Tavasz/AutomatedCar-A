using System;
using BaseModel.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace AutomatedCar.Visualization
{
    public class RenderCamera
    {
        public double LeftX { get; set; }
        public double TopY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double MiddleX { get; private set; }
        public double MiddleY { get; private set; }

        public Rect ViewportRect => new Rect(LeftX, TopY, Width, Height);

        public void UpdateMiddlePoint(double originX, double originY)
        {
            if (originX >= 0)
            {
                LeftX = originX - (Width / 2.0);
                MiddleX = originX;
            }

            if (originY >= 0)
            {
                TopY = originY - (Height / 2.0);
                MiddleY = originY;
            }
        }

        public List<IRenderableWorldObject> Filter(List<IRenderableWorldObject> renderables)
        {
            var visible = new List<IRenderableWorldObject>();

            foreach (var renderable in renderables)
            {
                visible.Add(renderable);
            }

            return visible;
        }

        public bool IsVisibleInViewport(IRenderableWorldObject renderable)
        {
            return ViewportRect.IntersectsWith(WorldObjectTransformer.GetBoundary(renderable));
        }

        public Point TranslateToViewport(double worldX, double worldY)
        {
            var x = worldX - LeftX;
            var y = worldY - TopY;
            return new Point(x, y);
        }
    }
}