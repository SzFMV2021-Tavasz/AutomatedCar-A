using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel.WorldObjects
{
    public class RoadSign : WorldObject
    {
        public RoadSign(Type type) : base(type) {
            validate();
        }


        protected override Type[] AllowedTypes => new[] {
            Type.ROADSIGN_PARKING_RIGHT,
            Type.ROADSIGN_PRIORITY_STOP,
            Type.ROADSIGN_SPEED_40,
            Type.ROADSIGN_SPEED_50,
            Type.ROADSIGN_SPEED_60
        };

        public override Tag Tags => Tag.ROAD_SIGN | Tag.SIGN;
    }
}
