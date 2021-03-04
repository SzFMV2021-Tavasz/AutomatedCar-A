using System;
using System.Collections.Generic;
using BaseModel.WorldObjects;

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
        public Dictionary<WorldObject.Type, Polygon> Load(string polygonsPath)
        {
            throw new NotImplementedException();
        }
    }
}