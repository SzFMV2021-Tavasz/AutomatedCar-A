using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseModel.WorldObjects;
using Newtonsoft.Json.Linq;

namespace BaseModel.JsonHelper
{
    public class JSONReferenceDictionaryDeserializer: JSONDictionaryDeserializer<Tuple<int, int>, JArray>
    {
        public JSONReferenceDictionaryDeserializer(Dictionary<WorldObject.Type, string> typenamesStringDictionary) : base(typenamesStringDictionary)
        {
        }

        protected override Dictionary<WorldObject.Type, Tuple<int, int>> ParseJson(JArray jFull)
        {
            throw new NotImplementedException();
        }
    }
}