using BaseModel.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BaseModel
{
    public class World
    {
        private int nextId = 0;
        
        public List<WorldObject> objects = new();

        public World() { }

        public World(List<WorldObject> objects)
        {
            this.objects = objects;
            foreach (var worldObject in this.objects) assignId(worldObject);
        }
        
        private void assignId(WorldObject obj)
        {
            obj.ID = nextId;
            nextId++;
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
    }
}
