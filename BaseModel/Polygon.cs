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
        
        private static Dictionary<WorldObject.Type, string> polygonNames = new Dictionary<WorldObject.Type, string>
        {
            { WorldObject.Type._2_CROSSROAD_1, "2_crossroad_1" },
            { WorldObject.Type._2_CROSSROAD_2, "2_crossroad_2" },
            { WorldObject.Type.BICYCLE, "bicycle" },
            { WorldObject.Type.BOUNDARY, "boundary" },
            { WorldObject.Type.CAR_1_BLUE, "car_1_blue" },
            { WorldObject.Type.CAR_1_RED, "car_1_red" },
            { WorldObject.Type.CAR_1_WHITE, "car_1_white" },
            { WorldObject.Type.CAR_2_BLUE, "car_2_blue" },
            { WorldObject.Type.CAR_2_RED, "car_2_red" },
            { WorldObject.Type.CAR_2_WHITE, "car_2_white" },
            { WorldObject.Type.CAR_3_BLACK, "car_3_black" },
            { WorldObject.Type.CIRCLE, "circle" },
            { WorldObject.Type.CROSSWALK, "crosswalk" },
            { WorldObject.Type.GARAGE, "garage" },
            { WorldObject.Type.MAN, "man" },
            { WorldObject.Type.PARKING_90, "parking_90" },
            { WorldObject.Type.PARKING_BOLLARD, "parking_bollard" },
            { WorldObject.Type.PARKING_SPACE_PARALLEL, "parking_space_parallel" },
            { WorldObject.Type.ROADSIGN_PARKING_RIGHT, "roadsign_parking_right" },
            { WorldObject.Type.ROADSIGN_PRIORITY_STOP, "roadsign_priority_stop" },
            { WorldObject.Type.ROADSIGN_SPEED_40, "roadsign_speed_40" },
            { WorldObject.Type.ROADSIGN_SPEED_50, "roadsign_speed_50" },
            { WorldObject.Type.ROADSIGN_SPEED_60, "roadsign_speed_60" },
            { WorldObject.Type.ROAD_2LANE_45LEFT, "road_2lane_45left" },
            { WorldObject.Type.ROAD_2LANE_45RIGHT, "road_2lane_45right" },
            { WorldObject.Type.ROAD_2LANE_6LEFT, "road_2lane_6left" },
            { WorldObject.Type.ROAD_2LANE_6RIGHT, "road_2lane_6right" },
            { WorldObject.Type.ROAD_2LANE_90LEFT, "road_2lane_90left" },
            { WorldObject.Type.ROAD_2LANE_90RIGHT, "road_2lane_90right" },
            { WorldObject.Type.ROAD_2LANE_ROTARY, "road_2lane_rotary" },
            { WorldObject.Type.ROAD_2LANE_STRAIGHT, "road_2lane_straight" },
            { WorldObject.Type.ROAD_2LANE_TJUNCTIONLEFT, "road_2lane_tjunctionleft" },
            { WorldObject.Type.ROAD_2LANE_TJUNCTIONRIGHT, "road_2lane_tjunctionright" },
            { WorldObject.Type.TREE, "tree" },
            { WorldObject.Type.WOMAN, "woman" },
        };

        public static string PolygonNameFrom (WorldObject.Type type)
        {
            foreach (WorldObject.Type dType in polygonNames.Keys)
            {
                if (type == dType)
                {
                    return polygonNames[dType];
                }
            }

            throw new NotSupportedException();
        }

    }
}

