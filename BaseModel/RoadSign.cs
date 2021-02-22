using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class RoadSign : WorldObject
    {
        public RoadSign(Type type) : base(type) {}

        public override bool IsDynamic => false;

        public override Type[] AllowedTypes => new[] {
            Type.ROADSIGN_PARKING_RIGHT,
            Type.ROADSIGN_PRIORITY_STOP,
            Type.ROADSIGN_SPEED_40,
            Type.ROADSIGN_SPEED_50,
            Type.ROADSIGN_SPEED_60
        };
    }
}
