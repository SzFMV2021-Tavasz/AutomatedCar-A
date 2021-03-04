using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using BaseModel.WorldObjects;

namespace BaseModel.JsonHelper
{
    class WorldConverter : CustomCreationConverter<World>
    {
        public override World Create(Type objectType)
        {
            return new World();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<WorldObject> worldObjects = new List<WorldObject>();
            int worldWidth = 0;
            int worldHeight = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "objects")
                {
                    reader.Read();  // StartArray
                    reader.Read();  // StartObject or EndArray

                    while (reader.TokenType != JsonToken.EndArray)
                    {
                        reader.Read();  // type
                        
                        switch (reader.ReadAsString())
                        {
                            // TODO: lehetséges duplikátum - lásd: JSONWorldSerializer-ben a Dictionary-t.
                            case "2_crossroad_1":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type._2_CROSSROAD_1));
                                break;
                            case "2_crossroad_2":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type._2_CROSSROAD_2));
                                break;
                            case "bicycle":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.BICYCLE));
                                break;
                            case "boundary":
                                worldObjects.Add(new WorldObjects.Obstacle(WorldObject.Type.BOUNDARY));
                                break;
                            case "car_1_blue":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_1_BLUE));
                                break;
                            case "car_1_red":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_1_RED));
                                break;
                            case "car_1_white":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_1_WHITE));
                                break;
                            case "car_2_blue":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_2_BLUE));
                                break;
                            case "car_2_red":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_2_RED));
                                break;
                            case "car_2_white":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_2_WHITE));
                                break;
                            case "car_3_black":
                                worldObjects.Add(new WorldObjects.Vehicle(WorldObject.Type.CAR_3_BLACK));
                                break;
                            case "crosswalk":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.CROSSWALK));
                                break;
                            case "garage":
                                worldObjects.Add(new WorldObjects.Building(WorldObject.Type.GARAGE));
                                break;
                            case "man":
                                worldObjects.Add(new WorldObjects.Iso5218_Person(WorldObject.Type.MAN));
                                break;
                            case "parking_90":
                                worldObjects.Add(new WorldObjects.ParkingSpot(WorldObject.Type.PARKING_90));
                                break;
                            case "parking_bollard":
                                worldObjects.Add(new WorldObjects.Obstacle(WorldObject.Type.PARKING_BOLLARD));
                                break;
                            case "parking_space_parallel":
                                worldObjects.Add(new WorldObjects.ParkingSpot(WorldObject.Type.PARKING_SPACE_PARALLEL));
                                break;
                            case "roadsign_parking_right":
                                worldObjects.Add(new WorldObjects.RoadSign(WorldObject.Type.ROADSIGN_PARKING_RIGHT));
                                break;
                            case "roadsign_priority_stop":
                                worldObjects.Add(new WorldObjects.RoadSign(WorldObject.Type.ROADSIGN_PRIORITY_STOP));
                                break;
                            case "roadsign_speed_40":
                                worldObjects.Add(new WorldObjects.RoadSign(WorldObject.Type.ROADSIGN_SPEED_40));
                                break;
                            case "roadsign_speed_50":
                                worldObjects.Add(new WorldObjects.RoadSign(WorldObject.Type.ROADSIGN_SPEED_50));
                                break;
                            case "roadsign_speed_60":
                                worldObjects.Add(new WorldObjects.RoadSign(WorldObject.Type.ROADSIGN_SPEED_60));
                                break;
                            case "road_2lane_45left":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_45LEFT));
                                break;
                            case "road_2lane_45right":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_45RIGHT));
                                break;
                            case "road_2lane_6left":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_6LEFT));
                                break;
                            case "road_2lane_6right":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_6RIGHT));
                                break;
                            case "road_2lane_90left":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_90LEFT));
                                break;
                            case "road_2lane_90right":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_90RIGHT));
                                break;
                            case "road_2lane_rotary":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_ROTARY));
                                break;
                            case "road_2lane_straight":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_STRAIGHT));
                                break;
                            case "road_2lane_tjunctionleft":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_TJUNCTIONLEFT));
                                break;
                            case "road_2lane_tjunctionright":
                                worldObjects.Add(new WorldObjects.Road(WorldObject.Type.ROAD_2LANE_TJUNCTIONRIGHT));
                                break;
                            case "tree":
                                worldObjects.Add(new WorldObjects.Tree());
                                break;
                            case "woman":
                                worldObjects.Add(new WorldObjects.Iso5218_Person(WorldObject.Type.WOMAN));
                                break;
                            case "car1":
                                worldObjects.Add(new Vehicle(WorldObject.Type.CAR_1_WHITE));
                                break;
                            case "car2":
                                worldObjects.Add(new Vehicle(WorldObject.Type.CAR_2_WHITE));
                                break;
                            case "car3":
                                worldObjects.Add(new Vehicle(WorldObject.Type.CAR_3_BLACK));
                                break;
                            default:
                                worldObjects.Add(new Tree());
                                break;
                        }

                        int currentIndex = worldObjects.Count - 1;

                        reader.Read();  // X
                        worldObjects[currentIndex].X = (int)reader.ReadAsInt32();

                        reader.Read();  // Y
                        worldObjects[currentIndex].Y = (int)reader.ReadAsInt32();


                        reader.Read();  // m11
                        worldObjects[currentIndex].Transformation_m11 = (float)reader.ReadAsDouble();
                        reader.Read();  // m12
                        worldObjects[currentIndex].Transformation_m12 = (float)reader.ReadAsDouble();
                        reader.Read();  // m21
                        worldObjects[currentIndex].Transformation_m21 = (float)reader.ReadAsDouble();
                        reader.Read();  // m22
                        worldObjects[currentIndex].Transformation_m22 = (float)reader.ReadAsDouble();

                        reader.Read(); // EndObject
                        reader.Read(); // StartObject or EndArray
                    }
                }
                else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "width")
                {
                    worldWidth = (int)reader.ReadAsInt32();
                }
                else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "height")
                {
                    worldHeight = (int)reader.ReadAsInt32();
                }
            }
            
            World world = new World(worldObjects);
            world.Width = worldWidth;
            world.Height = worldHeight;

            return world;
        }

    }
}
