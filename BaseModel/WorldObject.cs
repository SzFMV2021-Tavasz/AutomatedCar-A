using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public abstract class WorldObject
    {
        public int ID { get; set; }
        public abstract bool IsDynamic { get; }

        /// <returns>Null if the object is not collidable or the collision polygons otherwise.</returns>
        public virtual Polygon Polygon { get => null; }
        public enum Type { TREE, ROAD, CAR1, CAR2, CAR3 }

        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex { get; set; }
        // customState?     pl dictionary sajat mezoknek

        public abstract Type ObjectType { get; }
    }
}
