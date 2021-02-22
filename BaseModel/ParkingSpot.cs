using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class ParkingSpot : WorldObject
    {
        public ParkingSpot(Type type) : base(type) {}

        public override Type[] AllowedTypes => new Type[]
        {
            Type.PARKING_90,
            Type.PARKING_SPACE_PARALLEL,
        };

        public override bool IsDynamic => throw new NotImplementedException();
    }
}
