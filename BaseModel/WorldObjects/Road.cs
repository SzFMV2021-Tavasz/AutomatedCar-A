using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel.WorldObjects
{
    public class Road : WorldObject
    {
        public Road(Type type) : base(type)
        {
        }

        public override Type[] AllowedTypes => new Type[]
        {
            Type.ROAD_2LANE_45LEFT,
            Type.ROAD_2LANE_45RIGHT,
            Type.ROAD_2LANE_6LEFT,
            Type.ROAD_2LANE_6RIGHT,
            Type.ROAD_2LANE_90LEFT,
            Type.ROAD_2LANE_90RIGHT,
            Type.ROAD_2LANE_ROTARY,
            Type.ROAD_2LANE_STRAIGHT,
            Type.ROAD_2LANE_TJUNCTIONLEFT,
            Type.ROAD_2LANE_TJUNCTIONRIGHT,
            Type.CROSSWALK,
            Type._2_CROSSROAD_1,
            Type._2_CROSSROAD_2
        };

        public override Tag Tags => Tag.ROAD;
    }
}
