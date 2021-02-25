using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel.WorldObjects
{
    public abstract class WorldObject
    {
        private Dictionary<Type, Polygon> polyDict;

        public abstract Type[] AllowedTypes { get; }
        
        internal WorldObject(Type type)
        {
            this._objectType = type;
            ValidateType(type, AllowedTypes);
        }

        public int ID { get; set; }        
        public virtual Polygon Polygon { get => null; }

        public abstract Tag Tags { get; }

        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex { get; set; }

        public float Transformation_m11;
        public float Transformation_m12;
        public float Transformation_m21;
        public float Transformation_m22;

        // customState?     pl dictionary sajat mezoknek

        public static void ValidateType(Type type, Type[] allowedTypes)
        {
            foreach (Type t in allowedTypes)
            {
                if (t == type)
                {
                    return;
                }
            }

            throw new NotSupportedException();
        }

        protected Type _objectType;
        
        public Type ObjectType => _objectType;

        public enum Type
        {
            _2_CROSSROAD_1,
            _2_CROSSROAD_2,
            BICYCLE,
            BOUNDARY,
            CAR_1_BLUE,
            CAR_1_RED,
            CAR_1_WHITE,
            CAR_2_BLUE,
            CAR_2_RED,
            CAR_2_WHITE,
            CAR_3_BLACK,
            CIRCLE,
            CROSSWALK,
            GARAGE,
            MAN,
            PARKING_90,
            PARKING_BOLLARD,
            PARKING_SPACE_PARALLEL,
            ROADSIGN_PARKING_RIGHT,
            ROADSIGN_PRIORITY_STOP,
            ROADSIGN_SPEED_40,
            ROADSIGN_SPEED_50,
            ROADSIGN_SPEED_60,
            ROAD_2LANE_45LEFT,
            ROAD_2LANE_45RIGHT,
            ROAD_2LANE_6LEFT,
            ROAD_2LANE_6RIGHT,
            ROAD_2LANE_90LEFT,
            ROAD_2LANE_90RIGHT,
            ROAD_2LANE_ROTARY,
            ROAD_2LANE_STRAIGHT,
            ROAD_2LANE_TJUNCTIONLEFT,
            ROAD_2LANE_TJUNCTIONRIGHT,
            TREE,
            WOMAN,
        }


        [Flags]
        public enum Tag
        {
            NONE = 0,
            DYNAMIC = 1,
            BUILDING = 2,
            ROAD_OBSTACLE = 4,
            ROAD = 8,
            SIGN = 16,
            ROAD_SIGN = 32,
            NATURE = 64,
            VEHICLE = 128,
            PEDESTRIAN = 256
        }

    }
}
