﻿using AutomatedCar.Models;
using AutomatedCar.SystemComponents.SystemDebug;
using BaseModel.Interfaces;
using BaseModel.WorldObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace AutomatedCar.Visualization
{
    public class WorldRenderer : FrameworkElement
    {
        public readonly double tickRate = 20;
        private readonly DispatcherTimer renderTimer = new DispatcherTimer();
        private readonly RenderCamera renderCamera = new RenderCamera();
        private readonly StringBuilder log = new StringBuilder();

        private readonly Pen PolyPen = new Pen(Brushes.Red, 2);
        private readonly Pen SensorPen = new Pen(Brushes.Black, 1);
        private readonly Pen PolyPenHighLight = new Pen(Brushes.Yellow, 5);

        private Boolean drawPolygons = false;
        private Boolean drawDebugVideo = false;
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
            
            var car = GetAutomatedCar();
            
            SetRenderCameraMiddle(car);
            var objectsInRange = renderCamera.Filter(World.Renderables);

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

            if (drawDebugVideo && car.Video != null)
            {
                List<StreamGeometry> videoStreamGemometrys = GetStreamGeometryListOf(car.Video, new Point(car.X, car.Y));

                foreach (StreamGeometry streamGeometry in videoStreamGemometrys)
                {                   
                    drawingGroup.Children.Add(
                        createVideoGeometryDrawingBy(streamGeometry)
                    );
                }             
            }

            if (drawDebugRadar && car.Radar != null)
            {
                List<StreamGeometry> radarStreamGemometrys = GetStreamGeometryListOf(car.Radar, new Point(car.X, car.Y));

                foreach (StreamGeometry streamGeometry in radarStreamGemometrys)
                {
                    drawingGroup.Children.Add(
                        createRadarGeometryDrawingBy(streamGeometry)
                    );
                }
            }

            if (drawDebugSonic && car.UltraSonic != null)
            {
                List<StreamGeometry> sonicStreamGemometrys = GetStreamGeometryListOf(car.UltraSonic, new Point(car.X, car.Y));

                foreach (StreamGeometry streamGeometry in sonicStreamGemometrys)
                {
                    drawingGroup.Children.Add(
                        createSonicGeometryDrawingBy(streamGeometry)
                    );
                }
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

        private Drawing createSonicGeometryDrawingBy(StreamGeometry streamGeometry)
        {
            return createGeometryDrawingBy(Brushes.LightGreen, SensorPen, streamGeometry);
        }

        private Drawing createRadarGeometryDrawingBy(StreamGeometry streamGeometry)
        {
            return createGeometryDrawingBy(Brushes.LightPink, SensorPen, streamGeometry);
        }

        private Drawing createVideoGeometryDrawingBy(StreamGeometry streamGeometry)
        {       
            return createGeometryDrawingBy(Brushes.DodgerBlue, SensorPen, streamGeometry);
        }

        private Drawing createGeometryDrawingBy(SolidColorBrush brush, Pen sensorPen, StreamGeometry streamGeometry)
        {
            return new GeometryDrawing(brush, SensorPen, streamGeometry);
        }

        private List<StreamGeometry> GetStreamGeometryListOf(List<IDisplaySensor> sensorList, Point refPoint)
        {
            List<StreamGeometry> geometryList = new List<StreamGeometry>();
            foreach (IDisplaySensor item in sensorList)
            {
                geometryList.Add(GetStreamGeometryOf(item,refPoint));
            }
            return geometryList;
        }

        private StreamGeometry GetStreamGeometryOf(IDisplaySensor sensor, Point refPoint)
        {
            if (sensor != null)
            {
                List<Point> sensorDrawingPointsOnViewPort = GetSensorPointsOnViewPort(sensor, refPoint);

                return getPolyByPointList(sensorDrawingPointsOnViewPort, true);           
            }
            return null;
        }

        private List<Point> GetSensorPointsOnViewPort(IDisplaySensor sensor,Point refPoint)
        {
            List<Point> sensorDrawingPointsOnViewPort = new List<Point>();

            sensorDrawingPointsOnViewPort.Add(renderCamera.TranslateToViewport(sensor.x1.X + refPoint.X, sensor.x1.Y + refPoint.Y));
            sensorDrawingPointsOnViewPort.Add(renderCamera.TranslateToViewport(sensor.x2.X + refPoint.X, sensor.x2.Y + refPoint.Y));
            sensorDrawingPointsOnViewPort.Add(renderCamera.TranslateToViewport(sensor.x3.X + refPoint.X, sensor.x3.Y + refPoint.Y));

            return sensorDrawingPointsOnViewPort;
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
            renderCamera.viewPortSkin = 2;

            renderCamera.WorldWidth = World.Width;
            renderCamera.WorldHeight = World.Height;
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