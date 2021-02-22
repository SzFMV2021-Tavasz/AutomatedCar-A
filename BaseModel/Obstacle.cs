using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class Obstacle : WorldObject
    {
        public Obstacle(Type type) : base(type) {}

        public override Type[] AllowedTypes => new Type[] { Type.BOUNDARY, Type.PARKING_BOLLARD };

        public override bool IsDynamic => false;
    }
}
