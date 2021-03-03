using System;
using System.Collections.Generic;
using BaseModel.WorldObjects;

namespace BaseModel
{
    // WPF shape ehelyett? vagy WPF geometry?
    public class Polygon
    {
        public string Name { get; }

        /// <summary>
        /// x, y
        /// </summary>
        /// <returns>Null if the object is not collidable, the collision polygons otherwise.</returns>
        public Tuple<int, int>[] points { get; }

    }
}