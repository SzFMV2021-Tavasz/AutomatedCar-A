using BaseModel.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseModel.JsonHelper;
using Newtonsoft.Json;

namespace BaseModel
{
    public class World
    {
        private int nextId = 0;
        
        public List<WorldObject> objects = new();

        public World() {}

        public World(List<WorldObject> objects)
        {
            this.objects = objects;
            foreach (var worldObject in this.objects)
            {
                assignId(worldObject);
            }
        }

        private Dictionary<WorldObject.Type, List<Polygon>> _polygonDictionary;

        public Dictionary<WorldObject.Type, List<Polygon>> PolygonDictionary
        {
            private get => _polygonDictionary;
            set
            {
                this._polygonDictionary = value;

                foreach (WorldObject worldObject in objects)
                {
                    worldObject.PolygonDictionary = this._polygonDictionary;
                }
            }
        }

        public static World FromJSON(string worldJsonPath, string worldObjectPolygonsJsonPath)
        {
            return FromJSON(worldJsonPath, worldObjectPolygonsJsonPath, JSONWorldSerializer.TypenameStringDictionary);
        }
        
        public static World FromJSON(
            string worldJsonPath,
            string worldObjectPolygonsJsonPath,
            Dictionary<WorldObject.Type, string> typenameStringDictionary)
        {
            Dictionary<WorldObject.Type, List<Polygon>> polydict =
                new JSONPolygonDictionaryDeserializer(typenameStringDictionary)
                    .Load(worldObjectPolygonsJsonPath);
            
            JSONWorldSerializer serializer = new JSONWorldSerializer(JSONWorldSerializer.TypenameStringDictionary);

            World world = serializer.Load(worldJsonPath);
            world.PolygonDictionary = polydict;

            return world;
        }
        
        private void assignId(WorldObject obj)
        {
            obj.ID = nextId;
            nextId++;
        }

        private void assignPolygonDictionary(WorldObject obj)
        {
            obj.PolygonDictionary = _polygonDictionary;
        }

        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// @attention The ID of the supplied worldObject is going to be altered accordingly to this World instance.
        /// </summary>
        public void AddObject(WorldObject worldObject)
        {
            assignId(worldObject);
            assignPolygonDictionary(worldObject);
            objects.Add(worldObject);
        }
        
        public void RemoveObject(int ID)
        {
            objects.Remove( objects.First(o => o.ID == ID));
        }

        public WorldObject GetObjectByID(int ID)
        {
            return objects.First(o => o.ID == ID);
        }

        public WorldObject[] GetObjectsByType(WorldObject.Type type)
        {
            return objects.Where(o => o.ObjectType == type).ToArray();
        }

        public WorldObject[] GetObjectsWithTags(WorldObject.Tag tag)
        {
            return objects.Where(o => (o.Tags & tag) == tag).ToArray();
        }

        public WorldObject[] GetObjectsWithoutTags(WorldObject.Tag tag)
        {
            return objects.Where(o => (o.Tags & tag) == 0).ToArray();
        }

        public WorldObject[] GetObjectsInAreaTriangle(Triangle triangle)
        {
            throw new NotImplementedException();
        }
    }
}
