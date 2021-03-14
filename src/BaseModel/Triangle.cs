using System;
using System.Collections.Generic;
using System.Windows;

namespace BaseModel
{
    public class Triangle: Tuple<Point, Point, Point>
    {
        public List<Point> points { get; set; }
        public Triangle(Point item1, Point item2, Point item3) : base(item1, item2, item3)
        {
            points.Add(item1);
            points.Add(item2);
            points.Add(item3);
        }

        //public Triangle(int p0X, int p0Y, int p1X, int p1Y, int p2X, int p2Y) : base(new Tuple<int, int>(p0X, p0Y),
        //    new Tuple<int, int>(p1X, p1Y), new Tuple<int, int>(p2X, p2Y))
        //{
        //}
    }
}