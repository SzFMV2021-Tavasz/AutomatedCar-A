using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BaseModel.JsonHelper
{
    class WorldObjectJsonConverter : JsonConverter
    {
        private readonly Type[] _types;

        public WorldObjectJsonConverter(params Type[] types)
        {
            _types = types;
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            WorldObjects.WorldObject worldObject = (WorldObjects.WorldObject)value;
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue(JSONWorldSerializer.TypenameStringDictionary[worldObject.ObjectType]);

            writer.WritePropertyName("x");
            writer.WriteValue(worldObject.X);
            writer.WritePropertyName("y");
            writer.WriteValue(worldObject.Y);

            writer.WritePropertyName("m11");
            writer.WriteValue(worldObject.Transformation_m11);
            writer.WritePropertyName("m12");
            writer.WriteValue(worldObject.Transformation_m12);
            writer.WritePropertyName("m21");
            writer.WriteValue(worldObject.Transformation_m21);
            writer.WritePropertyName("m22");
            writer.WriteValue(worldObject.Transformation_m22);

            writer.WriteEndObject();
        }
    }
}
