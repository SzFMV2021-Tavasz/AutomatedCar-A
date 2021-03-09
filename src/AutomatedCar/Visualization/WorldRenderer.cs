using AutomatedCar.Models;
using AutomatedCar.Models.Enums;
using BaseModel.Interfaces;
using BaseModel.WorldObjects;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        public readonly double tickRate = 20;
        private readonly DispatcherTimer renderTimer = new DispatcherTimer();
        private readonly RenderCamera renderCamera = new RenderCamera();

        private readonly Pen PolyPen = new Pen(Brushes.White, 2);
        private readonly Pen PolyPenHighLight = new Pen(Brushes.Red, 2);

        private Boolean drawPolygons = true;
        private Boolean drawDebugCamera = false;
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
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (World == null)
            {
                return;
            }

            var car = GetAutomatedCar();
            List<IRenderableWorldObject> visibleWorldObjects = renderCamera.Filter(World.Renderables);

            SetRenderCameraMiddle(car);
            Render_Car(drawingContext, car);
            RenderPolygons(drawingContext, visibleWorldObjects.ConvertAll( wo => (WorldObject)wo));

            foreach (var worldObject in World.Renderables)
            {
                // worldObject.Render(drawingContext);
               
            }
        }

        private void RenderPolygons(DrawingContext drawingContext, List<WorldObject> renderingWolrdObjects)
        {

            if (drawPolygons)
            {
                foreach (var item in renderingWolrdObjects)
                {
                    drawPolylineByFlagAndWorldObject(drawingContext, item);
                }
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

            if (drawPolygons)
            {
                List<Point> carPolyList = new List<Point>();
                foreach (var item in car.Geometry.Points)
                {
                    carPolyList.Add(renderCamera.TranslateToViewport(item.X + car.X, item.Y + car.Y));
                }

                StreamGeometry carPoly = getPolyByPointList(carPolyList);
                drawingContext.DrawGeometry(null, PolyPen, carPoly);

            }

        }

        private StreamGeometry getPolyByPointList(List<Point> poliList)
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(poliList[0], true, true);
                PointCollection points = new PointCollection();

                poliList.ForEach(point => points.Add(point));

                geometryContext.PolyLineTo(points, true, true);
            }

            return streamGeometry;
        }

        private StreamGeometry getPolyByPointList(PointCollection poliCollection)
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(poliCollection[0], true, true);
                                           
                geometryContext.PolyLineTo(poliCollection, true, true);
            }

            return streamGeometry;
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

        private void drawPolylineByFlagAndWorldObject(DrawingContext drawingContext, WorldObject worldObject)
        {
            List<BaseModel.Polygon> poligons = worldObject.Polygons;


                foreach (var poligon in poligons)
                {
                    List<Point> displayPoints = new List<Point>();
                    foreach (var points in poligon.Points)
                    {
                        //TODO itt lehet van neki forgatása
                        displayPoints.Add(renderCamera.TranslateToViewport(points.Item1 + worldObject.X,points.Item2 + worldObject.Y));
                    }

                    StreamGeometry drawingGeometry = getPolyByPointList(displayPoints);
                    drawingContext.DrawGeometry(null, PolyPen, drawingGeometry);

                }
                    
        }
    }
}