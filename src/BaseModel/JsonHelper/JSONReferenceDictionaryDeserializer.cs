using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseModel.WorldObjects;
using log4net;
using Newtonsoft.Json.Linq;

namespace BaseModel.JsonHelper
{
    public class JSONReferenceDictionaryDeserializer: JSONDictionaryDeserializer<Tuple<int, int>, JArray>
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(JSONReferenceDictionaryDeserializer));
        
        public JSONReferenceDictionaryDeserializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary) : base(typenamesStringDictionary)
        {
        }

        protected override Dictionary<WorldObject.Type, Tuple<int, int>> ParseJson(JArray jFull, string jsonAbsolutePath)
        {
            Dictionary<WorldObject.Type, Tuple<int, int>> result = new();
            
            foreach (var elem in jFull)
            {
                try
                {
                    result.Add(
                        typenamesStringDictionaryReverse[elem["name"].ToString()],
                        new Tuple<int, int>(int.Parse(elem["x"].ToString()), int.Parse(elem["y"].ToString()))
                    );
                }
                catch (NullReferenceException e)
                {
                    LOGGER.Warn("Failed to parse an element in file " + jsonAbsolutePath, e);
                }
            }

            return result;
        }
    }
}