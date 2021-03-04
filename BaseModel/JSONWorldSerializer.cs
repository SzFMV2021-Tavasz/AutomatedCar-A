using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using BaseModel.WorldObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BaseModel
{
    public class JSONWorldSerializer
    {
        private Dictionary<WorldObject.Type, string> typenamesStringDictionary;
        public JSONWorldSerializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary)
        {
            this.typenamesStringDictionary = typenamesStringDictionary;
        }
        
        public World Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<World>(json, new JsonHelper.WorldConverter());
        }

        public void Save(World world, string path)
        {
            string json = JsonConvert.SerializeObject(
                world,
                Formatting.Indented,
                new JsonHelper.WorldObjectJsonConverter(WorldObjectTypes)
            );
            File.WriteAllText(path, json);
        }

        public static Type[] WorldObjectTypes =
        {
            typeof(Building),
            typeof(Iso5218_Person),
            typeof(Obstacle),
            typeof(ParkingSpot),
            typeof(Road),
            typeof(RoadSign),
            typeof(Tree),
            typeof(Vehicle)
        };

        public static Dictionary<WorldObject.Type, string> TypenameStringDictionary =
            new ()
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
    }
}