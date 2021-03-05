using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseModel;
using BaseModel.JsonHelper;
using BaseModel.WorldObjects;
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
        [TestCase(
            "SerializingDummies\\test_world.json",
            2,
            WorldObject.Type.ROAD_2LANE_STRAIGHT,
            124, 669, 1.0f, 0.0f, -0.0f, 1.0f)]
        [TestCase(
            "SerializingDummies\\test_world.json",
            6,
            WorldObject.Type.ROAD_2LANE_45RIGHT,
            526, 2193, 0.7071068f, -0.7071068f, 0.7071068f, 0.7071068f)]
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

        [TestCase(
            "SerializingDummies\\test_world.json",
            WorldObject.Tag.ROAD_SIGN,
            WorldObject.Type.ROADSIGN_PARKING_RIGHT,
            472, 2007
        )]
        [TestCase(
            "SerializingDummies\\test_world.json",
            WorldObject.Tag.NATURE,
            WorldObject.Type.TREE,
            169, 2454
        )]
        public void Test_Filtering_ByTag_First(
            string fileToLoad,
            WorldObject.Tag tags,
            WorldObject.Type expectedType,
            int expectedX, int expectedY)
        {
            World world = World.FromJSON(fileToLoad, POLYGON_JSON_PATH);
            WorldObject knownObject = world.GetObjectsWithTags(tags).First();
            WorldObject_AssertEqual_XYType(knownObject, expectedX, expectedY, expectedType);
        }

        [TestCase(
            "SerializingDummies\\test_world.json",
            WorldObject.Tag.ROAD,
            WorldObject.Type.PARKING_SPACE_PARALLEL,
            470, 766
        )]
        public void Test_Filtering_ByTagWithout_First(
            string fileToLoad,
            WorldObject.Tag tagsWithout,
            WorldObject.Type expectedType,
            int expectedX, int expectedY)
        {
            World world = World.FromJSON(fileToLoad, POLYGON_JSON_PATH);
            WorldObject knownObject = world.GetObjectsWithoutTags(tagsWithout).First();
            WorldObject_AssertEqual_XYType(knownObject, expectedX, expectedY, expectedType);
        }
        
        private static World MakeWorldWithJsonSerializer(string fileToLoad)
        {
            JSONWorldSerializer serializer = GetSerializer();
            return serializer.Load(fileToLoad);
        }

        private void WorldObject_AssertEqual_XYType(
            WorldObject obj,
            int expectedX,
            int expectedY,
            WorldObject.Type expectedType)
        {
            Assert.AreEqual(obj.ObjectType, expectedType);
            Assert.AreEqual(obj.X, expectedX);
            Assert.AreEqual(obj.Y, expectedY);
        }
        
        [Test]
        public void Test_KnownPolygonOfWorldObjects()
        {
            Dictionary<WorldObject.Type, List<Polygon>> polygonDictionary = GetPolygonDictionary();
            
            Vehicle car = new Vehicle(WorldObject.Type.CAR_1_WHITE);
            car.PolygonDictionary = polygonDictionary;

            Assert.AreEqual(car.Polygons[0].Points[0], new Tuple<int, int>(51, 239));
            Assert.AreEqual(car.Polygons[0].Points[1], new Tuple<int, int>(40, 238));
            Assert.AreEqual(car.Polygons[0].Points[2], new Tuple<int, int>(26, 236));
            Assert.AreEqual(car.Polygons[0].Points[3], new Tuple<int, int>(18, 231));
        }

        [TestCase(
            "SerializingDummies\\test_world.json",
            WorldObject.Type.TREE,
            92, 81,
            91, 88
        )]
        [TestCase(
            "SerializingDummies\\test_world.json",
            WorldObject.Type._2_CROSSROAD_1,
            0, 865,
            350, 865
        )]
        public void Test_SomeKnownPolygonsOfTestWorld(
            string fileToLoad,
            WorldObject.Type type,
            
            int expectedP0X0, int expectedP0Y0, int expectedP0X1, int expectedP0Y1
        )
        {
            World world = World.FromJSON(fileToLoad, POLYGON_JSON_PATH);
            
            WorldObject obj = world.GetObjectsByType(type).First();
            
            Assert.AreEqual(new Tuple<int, int>(expectedP0X1, expectedP0Y1), obj.Polygons[0].Points[1]);
            Assert.AreEqual(new Tuple<int, int>(expectedP0X0, expectedP0Y0), obj.Polygons[0].Points[0]);
        }

        [Test]
        public void Test_MultiPoly()
        {
            World world = World.FromJSON("SerializingDummies\\test_world.json", POLYGON_JSON_PATH);
            WorldObject obj = world.GetObjectsByType(WorldObject.Type._2_CROSSROAD_1).First();
            
            Assert.AreEqual(obj.Polygons[0].Type, Polygon.Type_t.LANE);
            Assert.AreEqual(obj.Polygons[1].Type, Polygon.Type_t.LANE);
            Assert.AreEqual(obj.Polygons[2].Type, Polygon.Type_t.LANE);
            
            Assert.AreEqual(new Tuple<int, int>(0,   865), obj.Polygons[0].Points[0]);
            Assert.AreEqual(new Tuple<int, int>(350, 865), obj.Polygons[0].Points[1]);
            Assert.AreEqual(new Tuple<int, int>(0,   535), obj.Polygons[1].Points[0]);
            Assert.AreEqual(new Tuple<int, int>(350, 535), obj.Polygons[1].Points[1]);
            Assert.AreEqual(new Tuple<int, int>(0,   700), obj.Polygons[2].Points[0]);
            Assert.AreEqual(new Tuple<int, int>(350, 700), obj.Polygons[2].Points[1]);
        }
        

        private static JSONWorldSerializer GetSerializer()
        {
            return new(JSONWorldSerializer.TypenameStringDictionary);
        }

        private Dictionary<WorldObject.Type, List<Polygon>> GetPolygonDictionary()
        {
            return 
                new JSONPolygonDictionaryDeserializer(JSONWorldSerializer.TypenameStringDictionary)
                    .Load(POLYGON_JSON_PATH);
        }
    }
}