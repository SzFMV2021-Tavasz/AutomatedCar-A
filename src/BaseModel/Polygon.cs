using System;
using System.Collections.Generic;
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
        public enum Type_t { STANDALONE, LANE, CURVE, CIRCLE }

        /// <summary>
        /// x, y
        /// </summary>
        /// <returns>Null if the object is not collidable, the collision polygons otherwise.</returns>
        public Tuple<int, int>[] Points { get; }
    }
}