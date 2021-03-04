using System;
using System.Collections.Generic;
using System.IO;
using BaseModel.WorldObjects;
using Newtonsoft.Json;

namespace BaseModel
{
    public class JSONPolygonDictionaryDeserializer
    {
        private Dictionary<WorldObject.Type, string> typenamesStringDictionary;
        
        public JSONPolygonDictionaryDeserializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary)
        {
            this.typenamesStringDictionary = typenamesStringDictionary;
        }
        
        /// <summary>
        /// If a typename cannot be found, either in the supplied enum mapping in the polygons file,
        /// then it is going to be interpreted as the object not being able to collide with others.
        /// </summary>
        public Dictionary<WorldObject.Type, List<Polygon>> Load(string polygonsPath)
        {
            Dictionary<WorldObject.Type, Polygon> polygonDictionary = new Dictionary<WorldObject.Type, Polygon>();
            Dictionary<string, Polygon> helper = new Dictionary<string, Polygon>();

            string jsonFile = File.ReadAllText(polygonsPath);
            
            return null;
            //return polygonDictionary = JsonConvert.DeserializeObject<Dictionary<WorldObject.Type, Polygon>>(jsonFile, new JsonHelper.PoligonDictionaryConverter()); ;
        }
    }
}