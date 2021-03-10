using AutomatedCar.Models;
using AutomatedCar.Models.Enums;
using BaseModel.Interfaces;
using BaseModel.WorldObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using BaseModel.Interfaces;
using BaseModel.WorldObjects;
using Point = System.Windows.Point;
using AutomatedCar.SystemComponents.SystemDebug;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        public readonly double tickRate = 20;
        private readonly DispatcherTimer renderTimer = new DispatcherTimer();
        private readonly RenderCamera renderCamera = new RenderCamera();

        private readonly Pen PolyPen = new Pen(Brushes.Red, 2);
        private readonly Pen VideoPen = new Pen(Brushes.Black, 2);
        private readonly Pen PolyPenHighLight = new Pen(Brushes.Yellow, 5);

        private Boolean drawPolygons = true;
        private Boolean drawDebugVideo = true;
        private Boolean drawDebugRadar = false;
        private Boolean drawDebugSonic = false;

        public World World => World.Instance;

        public Boolean DrawPolygons
        {
            private get => drawPolygons;
            set
            {
                this.drawPolygons = value;
            }
        }

        public WorldRenderer()
        {
            Loaded += WorldRenderer_Loaded;   
            HMIDebug.DebugActionEventHandler += Debug_EventCacher;

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (World == null)
            {
                return;
            }

            var objectsInRange = renderCamera.Filter(World.Renderables);
            
            var car = GetAutomatedCar();
            
            SetRenderCameraMiddle(car);
            
            foreach (var worldObject in objectsInRange)
            {
                RenderObject(drawingContext, worldObject);
            }

            RenderCar(drawingContext, car);
        }

        private void RenderObject(DrawingContext drawingContext, IRenderableWorldObject worldObject)
        {
            DrawingGroup drawingGroup = RenderImage(worldObject);

            if (drawPolygons)
                drawingGroup = RenderPolygon(drawingGroup, worldObject);

            drawingGroup = RotateObjectOnDrawingGroup(drawingGroup, worldObject);
            
            drawingContext.DrawDrawing(drawingGroup);
        }

        private DrawingGroup RenderPolygon(DrawingGroup drawingGroup, IRenderableWorldObject renderable)
        {
            WorldObject worldObject = renderable as WorldObject; 

            List<BaseModel.Polygon> poligons = worldObject.Polygons;

            Point refPoint = this.renderCamera.TranslateToViewport(worldObject.X, worldObject.Y);
            
            Vector delta = WorldObjectTransformer.GetDeltaVector(worldObject.Filename);
            
            Point center = new Point(refPoint.X - delta.X, refPoint.Y - delta.Y);

            if (poligons != null)
            {
                foreach (var poligon in poligons)
                {
                    List<Point> displayPoints = new List<Point>();

                    foreach (var points in poligon.Points)
                        displayPoints.Add(new Point(points.Item1 + center.X, points.Item2 + center.Y));

                    StreamGeometry streamGeometry = getPolyByPointList(displayPoints, false);
                    GeometryDrawing drawingGeometry = new GeometryDrawing(null, renderable.IsHighLighted ? PolyPenHighLight : PolyPen, streamGeometry);

                    drawingGroup.Children.Add(drawingGeometry);
                }
            }

            return drawingGroup;
        }

        private DrawingGroup RenderImage(IRenderableWorldObject worldObject)
        {
            DrawingGroup drawingGroup = new DrawingGroup();

            BitmapImage bm = getBitMapImage(ref worldObject);

            Point refPoint = renderCamera.TranslateToViewport(worldObject.X, worldObject.Y);

            Vector delta = WorldObjectTransformer.GetDeltaVector(worldObject.Filename);

            Point topLeft = new Point(refPoint.X - delta.X, refPoint.Y - delta.Y);
            
            drawingGroup.Children.Add(
                new ImageDrawing(
                    bm,
                    new Rect(topLeft, new Size(worldObject.Width, worldObject.Height)
                    ))
            );

            return drawingGroup;
        }

        private void SetRenderCameraMiddle(Models.AutomatedCar car)
        {
            Point carMiddleReference = getMiddleReference(car.X, car.Y, car.Width, car.Height);
            
            this.renderCamera.UpdateMiddlePoint(carMiddleReference.X, carMiddleReference.Y);
        }

        private void RenderCar(DrawingContext drawingContext, Models.AutomatedCar car)
        {
            DrawingGroup drawingGroup = RenderImage(car);

            if (drawDebugVideo)
            {
                List<Point> carPolyList = new List<Point>();

                foreach (var item in car.Video.Points)
                {
                    carPolyList.Add(renderCamera.TranslateToViewport(item.X + car.X, item.Y + car.Y));
                }

                StreamGeometry carPoly = getPolyByPointList(carPolyList, true);

                GeometryDrawing geometryDrawing = new GeometryDrawing(Brushes.Aqua, VideoPen, carPoly);

                drawingGroup.Children.Add(
                    geometryDrawing
                );
            }

            if (drawDebugRadar)
            {

            }

            if (drawDebugSonic)
            {
                List<Point> carPolyList = new List<Point>();

                foreach (var item in car.UltraSonic.Points)
                {
                    carPolyList.Add(renderCamera.TranslateToViewport(item.X + car.X, item.Y + car.Y));
                }

                StreamGeometry carPoly = getPolyByPointList(carPolyList, true);

                GeometryDrawing geometryDrawing = new GeometryDrawing(Brushes.LightGreen, VideoPen, carPoly);

                drawingGroup.Children.Add(
                    geometryDrawing
                );
            }

            if (drawPolygons)
            {
                List<Point> carPolyList = new List<Point>();

                foreach (var item in car.Geometry.Points)
                {
                    carPolyList.Add(renderCamera.TranslateToViewport(item.X + car.X, item.Y + car.Y));
                }

                StreamGeometry carPoly = getPolyByPointList(carPolyList, false);

                GeometryDrawing geometryDrawing = new GeometryDrawing(null, car.IsHighLighted ? PolyPenHighLight : PolyPen, carPoly); 

                drawingGroup.Children.Add(
                    geometryDrawing
                    );
            }

            drawingGroup = RotateObjectOnDrawingGroup(drawingGroup, car);
            
            drawingContext.DrawDrawing(drawingGroup);
        }

        private StreamGeometry getPolyByPointList(List<Point> poliList, bool isClosed)
        {
            StreamGeometry streamGeometry = new StreamGeometry();

            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(poliList[0], true, isClosed);
                
                PointCollection points = new PointCollection();

                poliList.ForEach(point => points.Add(point));

                geometryContext.PolyLineTo(points, true, true);
            }

            return streamGeometry;
        }

        private BitmapImage getBitMapImage(ref IRenderableWorldObject worldObject)
        {
            if (worldObject.Filename == null)
            {
                worldObject.Filename = Enum.GetName((worldObject as WorldObject).ObjectType).ToLower() + ".png";
            }

            BitmapImage bm = WorldObjectTransformer.GetCachedImage(worldObject.Filename);

            if (worldObject.Width <= 0)
                worldObject.Width = (int)bm.PixelWidth;

            if (worldObject.Height <= 0)
                worldObject.Height = (int)bm.PixelHeight;

            return bm;
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

        private void Debug_EventCacher(object sender,DebugActionArgs args)
        {
            drawDebugVideo = args.DebugVideo;
            drawDebugRadar = args.DebugRadar;
            drawDebugSonic = args.DebugSonic;
            drawPolygons = args.DebugPolys;
        }

        private Point getMiddleReference(Double x, Double y, Double width, Double height)
        {
            return new Point(x + (width / 2), y + (height / 2));
        }

        public DrawingGroup RotateObjectOnDrawingGroup(DrawingGroup drawingGroup, IRenderableWorldObject worldObject)
        {
            Point refPoint = renderCamera.TranslateToViewport(worldObject.X, worldObject.Y);

            double rotationAngle = WorldObjectTransformer.GetRotationAngle(worldObject);

            drawingGroup.Transform = new RotateTransform(
                rotationAngle,
                refPoint.X,
                refPoint.Y
            );

            return drawingGroup;
        }
    }
}