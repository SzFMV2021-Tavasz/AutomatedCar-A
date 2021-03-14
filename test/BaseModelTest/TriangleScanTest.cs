using System;
using System.Collections.Generic;
using System.Linq;
using BaseModel;
using BaseModel.WorldObjects;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace BaseModelTest
{
    public class TriangleScanTest
    {
        public World GetPresetWorld(string worldJsonBasename = "FilteringDummy_1.json")
        {
            return World.FromJSON(
                "TriangleFilteringDummies\\" + worldJsonBasename,
                "SerializingDummies\\worldobject_polygons.json",
                "SerializingDummies\\reference_points.json"
            );
        }

        [TestCase(  0,   0,   0,   0,   0,   0,     0, new WorldObject.Type[] {}, new int[] {})]
        [TestCase(  0,   0,   0,  80, 200,   0,     1, new[] {WorldObject.Type.MAN}, new[]{1})]
        
        [TestCase( 40,  80,  40, 400, 620, 400,     3,
            new[] {WorldObject.Type.TREE, WorldObject.Type.BOUNDARY, WorldObject.Type.GARAGE},
            new[] {1, 1, 1})]
        
        [TestCase(220,   0, 340,   0, 340, 160,     1, new[] {WorldObject.Type.ROADSIGN_SPEED_60}, new[]{1})]
        [TestCase(700,   0,  50, 700, 700, 700,     3, 
            new[]{WorldObject.Type.CAR_1_BLUE, WorldObject.Type.BICYCLE, WorldObject.Type.GARAGE}, new[]{1, 1, 1})]
        public void TestTriangleScanWithSomeExpectedResults(
            int p0X, int p0Y, int p1X, int p1Y, int p2X, int p2Y, int totalAmount,
            WorldObject.Type[] types, int[] amounts
        )
        {
            World world = GetPresetWorld();

            WorldObject[] objs = world.GetObjectsInAreaTriangle(new Triangle(p0X, p0Y, p1X, p1Y, p2X, p2Y));
            
            Assert.AreEqual(totalAmount, objs.Length);

            for (int i = 0; i < types.Length; i++)
            {
                Assert.AreEqual(amounts[i], objs.Count(o => o.ObjectType == types[i]));
            }
        }
    }
}