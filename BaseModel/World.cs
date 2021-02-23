using BaseModel.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class World
    {
        public List<WorldObject> objects = new List<WorldObject>();        

        public World() { }

        public World(List<WorldObject> objects)
        {
            this.objects = objects;
        }

        public WorldObject GetObjectByID(int ID)
        {
            return objects.Where(o => o.ID == ID).First();
        }

        public WorldObject[] GetObjectsWithTags(WorldObject.Tag tag)
        {
            return objects.Where(o => (o.Tags & tag) == tag).ToArray();
        }
    }
}
