using System;
using System.Collections.Generic;
using BaseModel.WorldObjects;


namespace BaseModel
{
    // WPF shape ehelyett? vagy WPF geometry?
    public class Polygon
    {
        private Type type;
        public Polygon(Type type)
        {
            this.type = type;
        }
        public enum Type { STANDALONE, LANE, CURVE, CIRCLE }

        /// <summary>
        /// x, y
        /// </summary>
        /// <returns>Null if the object is not collidable, the collision polygons otherwise.</returns>
        public Tuple<int, int>[] points { get; }
    }
}