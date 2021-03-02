using System;
using System.Collections.Generic;
using System.IO;
using BaseModel;
using BaseModel.WorldObjects;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;

namespace BaseModelTest
{
    public class Tests
    {

        private static readonly string POLYGON_JSON_PATH = "SerializingDummies\\worldobject_polygons.json";
        
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("SerializingDummies\\test_world.json")]
        [TestCase("SerializingDummies\\test_world_1kmx1km.json")]
        public void Test_WhenWorldNotModified_ThenInputOutputIsSame(string worldPath)
        {
            FileUtils fileUtils = new FileUtils();
            string tmpFilePath = Path.GetTempFileName();

            JSONWorldSerializer serializer = GetSerializer();
            
            try
            {
                World world = serializer.Load(worldPath);
                serializer.Save(world, tmpFilePath);
                Assert.True(fileUtils.TextFilesEqual(worldPath, tmpFilePath));
            }
            finally
            {
                File.Delete(tmpFilePath);
            }
        }
        
        [TestCase(
            "SerializingDummies\\test_world.json",
            0,
            WorldObject.Type.ROAD_2LANE_STRAIGHT,
            1700, 144, 0.0f, 1.0f, -1.0f, 0.0f)]
        public void Test_SomeKnownValuesInWorld(
            string fileToLoad, int objectId, WorldObject.Type type, int x, int y, float m11, float m12, float m21, float m22
        )
        {
            JSONWorldSerializer serializer = GetSerializer(); 
            
            World world = serializer.Load(fileToLoad);
            WorldObject knownObject = world.GetObjectByID(objectId);
            
            Assert.AreEqual(knownObject.X, x);
            Assert.AreEqual(knownObject.Y, y);
            Assert.AreEqual(knownObject.ObjectType, type);
            Assert.AreEqual(knownObject.Transformation_m11, m11);
            Assert.AreEqual(knownObject.Transformation_m12, m12);
            Assert.AreEqual(knownObject.Transformation_m21, m21);
            Assert.AreEqual(knownObject.Transformation_m22, m22);
        }
        
        [Test]
        public void Test_KnownPolygonOfWorldObjects()
        {
            Dictionary<WorldObject.Type, Polygon> polygonDictionary = GetPolygonDictionary();
            
            Vehicle car = new Vehicle(WorldObject.Type.CAR_1_WHITE);
            car.SetPolygonNameDictionary(polygonDictionary);

            Assert.Equals(car.Polygon.points[0], new Tuple<int, int>(51, 239));
            Assert.Equals(car.Polygon.points[1], new Tuple<int, int>(40, 238));
            Assert.Equals(car.Polygon.points[2], new Tuple<int, int>(26, 236));
            Assert.Equals(car.Polygon.points[3], new Tuple<int, int>(18, 231));
        }

        private JSONWorldSerializer GetSerializer()
        {
            return new JSONWorldSerializer(JSONWorldSerializer.TypenameStringDictionary);
        }

        private Dictionary<WorldObject.Type, Polygon> GetPolygonDictionary()
        {
            return 
                new JSONPolygonDictionaryDeserializer(JSONWorldSerializer.TypenameStringDictionary)
                    .Load(POLYGON_JSON_PATH);
        }

    }
}