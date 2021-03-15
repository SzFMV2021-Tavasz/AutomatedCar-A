using BaseModel.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseModel.JsonHelper;
using Newtonsoft.Json;
using System.Windows;

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
        private Dictionary<WorldObject.Type, Tuple<int, int>> _referencePointDictionary;

        [JsonIgnore]
        public Dictionary<WorldObject.Type, List<Polygon>> PolygonDictionary
        {
            private get => _polygonDictionary;
            set
            {
                this._polygonDictionary = value;
                updateAllMappings();
            }
        }

        [JsonIgnore]
        public Dictionary<WorldObject.Type, Tuple<int, int>> ReferencePointDictionary
        {
            get => _referencePointDictionary;
            set
            {
                _referencePointDictionary = value;
                updateAllMappings();
            }
        }

        public static World FromJSON(string worldJsonPath, string worldObjectPolygonsJsonPath, string referencePointsJsonPath)
        {
            return FromJSON(worldJsonPath, worldObjectPolygonsJsonPath, referencePointsJsonPath, JSONWorldSerializer.TypenameStringDictionary);
        }
        
        public static World FromJSON(
            string worldJsonPath,
            string worldObjectPolygonsJsonPath,
            string referencePointsJsonPath,
            Dictionary<WorldObject.Type, string> typenameStringDictionary)
        {
            World world = new JSONWorldSerializer(JSONWorldSerializer.TypenameStringDictionary).Load(worldJsonPath);
            
            world.PolygonDictionary = new JSONPolygonDictionaryDeserializer(typenameStringDictionary)
                .Load(worldObjectPolygonsJsonPath);

            world.ReferencePointDictionary = new JSONReferenceDictionaryDeserializer(typenameStringDictionary)
                .Load(referencePointsJsonPath);

            return world;
        }
        
        private void assignId(WorldObject obj)
        {
            obj.ID = nextId;
            nextId++;
        }

        private void updateAllMappings()
        {
            foreach (WorldObject worldObject in objects)
            {
                assignMappings(worldObject);
            }
        }

        private void assignMappings(WorldObject obj)
        {
            assignPolygonDictionary(obj);
            assignReferenceDictionary(obj);
        }
        
        private void assignPolygonDictionary(WorldObject obj)
        {
            obj.PolygonDictionary = _polygonDictionary;
        }

        private void assignReferenceDictionary(WorldObject obj)
        {
            obj.ReferencePointDictionary = _referencePointDictionary;
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
            assignMappings(worldObject);
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
            List<WorldObject> objectsInTriangle = new List<WorldObject>();

            foreach (WorldObject item in objects)
            {
                foreach (Polygon polygon in item.Polygons)
                {
                    polygon.PPoints = polygon.TupleListToPointList(polygon.Points);
                    int i = 0;
                    Point point = new Point();
                    point.X = polygon.PPoints[i].X + item.X;
                    point.Y = polygon.PPoints[i].Y + item.Y;
                    while (!PointInTriangle(point, triangle.Item1, triangle.Item2, triangle.Item3) && polygon.PPoints.Count > i)
                    {
                        point.X = polygon.PPoints[i].X + item.X;
                        point.Y = polygon.PPoints[i].Y + item.Y;
                        i++;
                    }
                    if (PointInTriangle(point, triangle.Item1, triangle.Item2, triangle.Item3))
                    {
                        objectsInTriangle.Add(item);
                    }
                }             
            }
            return objectsInTriangle.ToArray();
        }

        public static bool PointInTriangle(Point p, Point p1, Point p2, Point p3)
        {
            double a = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) / ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            double b = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) / ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            double c = 1 - a - b;

            if (a >= 0 && a <= 1 && b >= 0 && b <= 1 && c >= 0 && c <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
