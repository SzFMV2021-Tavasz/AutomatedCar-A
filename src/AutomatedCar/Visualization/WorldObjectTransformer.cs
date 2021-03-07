namespace AutomatedCar.Visualization
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class WorldObjectTransformer : IValueConverter
    {
        private static Dictionary<string, BitmapImage> cache = new Dictionary<string, BitmapImage>();

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