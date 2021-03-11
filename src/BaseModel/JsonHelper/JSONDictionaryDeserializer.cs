using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseModel.WorldObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BaseModel.JsonHelper
{
    public abstract class JSONDictionaryDeserializer<T>
    {
        protected Dictionary<WorldObject.Type, string> typenamesStringDictionary;
        protected readonly Dictionary<string, WorldObject.Type> typenamesStringDictionaryReverse;

        protected JSONDictionaryDeserializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary)
        {
            this.typenamesStringDictionary = typenamesStringDictionary;
            this.typenamesStringDictionaryReverse = typenamesStringDictionary.ToDictionary(e => e.Value, e => e.Key);
        }
        
        /// <summary>
        /// If a typename cannot be found, either in the supplied enum mapping in the polygons file,
        /// then it is going to be interpreted as the object not being able to collide with others.
        /// </summary>
        public Dictionary<WorldObject.Type, T> Load(string polygonsPath)
        {
            try
            {
                string jsonFileContent = File.ReadAllText(polygonsPath);
                JObject j_full = JsonConvert.DeserializeObject<JObject>(jsonFileContent);
                return ParseJson(j_full);
            }
            catch (NullReferenceException e)
            {
                throw new JSONReadingException("Failed to parse JSON.", e);
            }
        }

        protected abstract Dictionary<WorldObject.Type, T> ParseJson(JObject jFull);
    }
}