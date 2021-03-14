using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;

namespace AutomatedCar.Visualization
{
    using BaseModel.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class WorldObjectTransformer : IValueConverter
    {
        private static Dictionary<string, BitmapImage> cache = new Dictionary<string, BitmapImage>();
        private static Dictionary<string, Vector> referencePoints = new Dictionary<string, Vector>();
        public static WorldObjectTransformer Instance { get; } = new WorldObjectTransformer();

        internal static BitmapImage GetCachedImage(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                var image = new BitmapImage(new Uri($"Assets/WorldObjects/{filename}", UriKind.Relative));
                image.Freeze();
                cache.Add(filename, image);
            }

            return cache[filename];
        }

        internal static Vector GetDeltaVector(string filename)
        {
            if (referencePoints.Count == 0)
            {
                XDocument xdoc = XDocument.Load($"{Directory.GetCurrentDirectory()}/Assets/reference_points.xml");
                xdoc.Root.Descendants("Image").ToList().ForEach(x => referencePoints.Add(x.Attribute("name").Value.ToString(), new Vector(int.Parse(x.Element("Refpoint").Attribute("x").Value), int.Parse(x.Element("Refpoint").Attribute("y").Value))));
            }

            if (!referencePoints.ContainsKey(filename))
                return new Vector(0, 0);

            return referencePoints[filename];
        }

        internal static double GetRotationAngle(IRenderableWorldObject renderable)
        {
            var x = new Vector(1, 0);
            Vector rotated = Vector.Multiply(x, new Matrix(renderable.M11, renderable.M12, renderable.M21, renderable.M22, 0, 0));
            return Vector.AngleBetween(x, rotated);
        }

        internal static Rect GetBoundary(IRenderableWorldObject renderable)
        {
            return new Rect(renderable.X, renderable.Y, renderable.Width, renderable.Height);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            GetCachedImage((string)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}