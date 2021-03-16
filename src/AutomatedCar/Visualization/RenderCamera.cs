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

        public double WorldWidth { get; set; }
        public double WorldHeight { get; set; }

        public double viewPortSkin { get; set; }
        public Rect ViewportRect => new Rect(LeftX, TopY, Width, Height);

        public void UpdateMiddlePoint(double originX, double originY)
        {
            EnforceMiddlePointStaysInBoundaries(ref originX, ref originY);

            LeftX = originX - (Width / 2.0);
            MiddleX = originX;

            TopY = originY - (Height / 2.0);
            MiddleY = originY;
        }

        private void EnforceMiddlePointStaysInBoundaries(ref double middleX, ref double middleY)
        {
            middleX = Math.Clamp(middleX, 0, WorldWidth);
            middleY = Math.Clamp(middleY, 0, WorldHeight);
        }

        public List<IRenderableWorldObject> Filter(List<IRenderableWorldObject> renderables)
        {
            var visible = new List<IRenderableWorldObject>();

            foreach (var renderable in renderables)
            {
                if (IsWithinDrawingDistance(renderable))
                {
                    visible.Add(renderable);
                }
            }

            return visible;
        }

        public bool IsWithinDrawingDistance(IRenderableWorldObject renderable)
        {
            var reference = Math.Max(Width, Height);
            var range = reference * (1 + viewPortSkin);
            var distance = Math.Sqrt(Math.Pow(renderable.X - MiddleX, 2) + Math.Pow(renderable.Y - MiddleY, 2));

            return distance <= range;
        }

        private double CushionSide(double side)
        {
            return side + (side * viewPortSkin);
        }

        public Point TranslateToViewport(double worldX, double worldY)
        {
            var x = worldX - LeftX;
            var y = worldY - TopY;
            return new Point(x, y);
        }
    }
}