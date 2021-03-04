using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseModel.WorldObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BaseModel
{
    public class JSONPolygonDictionaryDeserializer
    {
        private Dictionary<WorldObject.Type, string> typenamesStringDictionary;
        private Dictionary<string, WorldObject.Type> typenamesStringDictionaryReverse = new Dictionary<string, WorldObject.Type>();
        
        public JSONPolygonDictionaryDeserializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary)
        {
            this.typenamesStringDictionary = typenamesStringDictionary;
            this.typenamesStringDictionaryReverse = typenamesStringDictionary.ToDictionary(e => e.Value, e => e.Key);
        }

        public static Dictionary<string, Polygon.Type_t> PolygonTypeDict = new()
        {
            {"standalone", Polygon.Type_t.STANDALONE},
            {"lane", Polygon.Type_t.LANE},
            {"curve", Polygon.Type_t.CURVE},
            {"circle", Polygon.Type_t.CIRCLE},
        };
        
        /// <summary>
        /// If a typename cannot be found, either in the supplied enum mapping in the polygons file,
        /// then it is going to be interpreted as the object not being able to collide with others.
        /// </summary>
        public Dictionary<WorldObject.Type, List<Polygon>> Load(string polygonsPath)
        {
            Dictionary<WorldObject.Type, List<Polygon>> polygonDictionary = new Dictionary<WorldObject.Type, List<Polygon>>();

            string jsonFileContent = File.ReadAllText(polygonsPath);

            JObject j_full = JsonConvert.DeserializeObject<JObject>(jsonFileContent);
            var j_objects = j_full["objects"];

            foreach (var j_object in j_objects)
            {
                WorldObject.Type type = this.typenamesStringDictionaryReverse[j_object["typename"].ToString()];
                List<Polygon> polys = new();

                foreach (var j_poly in j_object["polys"])
                {
                    Polygon.Type_t polyType = PolygonTypeDict[j_poly["type"].ToString()];
                    List<Tuple<int, int>> points = new();

                    foreach (var j_point in j_poly["points"])
                    {
                        points.Add(new Tuple<int, int>(int.Parse(j_point[0].ToString()), int.Parse(j_point[1].ToString())));
                    }

                    polys.Add(new Polygon(polyType, points.ToArray()));
                }
                
                polygonDictionary.Add(type, polys);
            }

            return polygonDictionary;
        }
    }
}