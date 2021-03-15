using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using BaseModel.WorldObjects;


namespace BaseModel
{
    // WPF shape ehelyett? vagy WPF geometry?
    public class Polygon
    {
        public Type_t Type { get; }
        public Polygon(Type_t type, Tuple<int, int>[] points)
        {
            this.Type = type;
            this.Points = points;
        }
        public Polygon(Type_t type, List<Point> pPoints)
        {
            this.Type = type;
            this.PPoints = pPoints;
        }
        public enum Type_t { STANDALONE, LANE, CURVE, CIRCLE }

        /// <summary>
        /// x, y
        /// </summary>
        /// <returns>Null if the object is not collidable, the collision polygons otherwise.</returns>
        public Tuple<int, int>[] Points { get; }
        public List<Point> PPoints { get; set; }

        private Point TupleToPointConvert(Tuple<int, int> tuple)
        {
            Point item = new Point();
            item.X = tuple.Item1;
            item.Y = tuple.Item2;
            return item;
        }

        public List<Point> TupleListToPointList(Tuple<int, int>[] tupleList)
        {
            List<Point> pointlist = new List<Point>();
            for (int i = 0; i < tupleList.Length; i++)
            {
                Point item = TupleToPointConvert(tupleList[i]);
                pointlist.Add(item);
            }
            return pointlist;
        }
    }


}