using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseModel.JsonHelper;
using BaseModel.WorldObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BaseModel
{
    class JSONPolygonDictionaryDeserializer: JSONDictionaryDeserializer<List<Polygon>>
    {
        public JSONPolygonDictionaryDeserializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary) : base(typenamesStringDictionary)
        {
        }

        public static Dictionary<string, Polygon.Type_t> PolygonTypeDict = new()
        {
            {"standalone", Polygon.Type_t.STANDALONE},
            {"lane", Polygon.Type_t.LANE},
            {"curve", Polygon.Type_t.CURVE},
            {"circle", Polygon.Type_t.CIRCLE},
        };
        
        protected override Dictionary<WorldObject.Type, List<Polygon>> ParseJson(JObject j_full)
        {
            try
            {
                return ParseJson_Objects(j_full["objects"]);
            }
            catch (NullReferenceException e)
            {
                throw new JSONReadingException("Failed to parse JSON.", e);
            }
        }

        private Dictionary<WorldObject.Type, List<Polygon>> ParseJson_Objects(JToken j_objects)
        {
            Dictionary<WorldObject.Type, List<Polygon>> polygonDictionary = new();
            
            foreach (var j_object in j_objects)
            {
                ParseJson_Objects_IntoDict(j_object, polygonDictionary);
            }

            return polygonDictionary;
        }

        private void ParseJson_Objects_IntoDict(JToken j_object, Dictionary<WorldObject.Type, List<Polygon>> polygonDictionary)
        {
            WorldObject.Type type = this.typenamesStringDictionaryReverse[j_object["typename"].ToString()];
            
            List<Polygon> polys = new();

            foreach (var j_poly in j_object["polys"])
            {
                Polygon.Type_t polyType = PolygonTypeDict[j_poly["type"].ToString()];
                List<Tuple<int, int>> points = ParseJson_Points(j_poly);
                polys.Add(new Polygon(polyType, points.ToArray()));
            }

            polygonDictionary.Add(type, polys);
        }

        private static List<Tuple<int, int>> ParseJson_Points(JToken j_poly)
        {
            List<Tuple<int, int>> points = new();
        
            foreach (var j_point in j_poly["points"])
            {
                points.Add(ParseJson_Point(j_point));
            }
            
            return points;
        }

        private static Tuple<int, int> ParseJson_Point(JToken j_point)
        {
            return new(int.Parse(j_point[0].ToString()), int.Parse(j_point[1].ToString()));
        }

        
    }
}