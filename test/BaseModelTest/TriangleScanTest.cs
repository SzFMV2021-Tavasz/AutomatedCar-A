using System;
using System.Collections.Generic;
using System.Linq;
using BaseModel;
using BaseModel.WorldObjects;
using NUnit.Framework;

namespace BaseModelTest
{
    public class TriangleScanTest
    {
        public World GetWorld()
        {
            return World.FromJSON("SerializingDummies\\test_world.json", "SerializingDummies\\worldobject_polygons.json");
        }
        
        [TestCase(0, 0, 0, 0, 0, 0, 0, new WorldObject.Type[] {}, new int[] {})]
        // [TestCase(0, 0, 0, 0, 0, 0, 1, new[] {WorldObject.Type.MAN}, new[] {1})]
        public void TestTriangleScanWithSomeExpectedResults(
            int p0X, int p0Y, int p1X, int p1Y, int p2X, int p2Y, int totalAmount,
            WorldObject.Type[] types, int[] amounts
        )
        {
            World world = GetWorld();

            WorldObject[] objs = world.GetObjectsInAreaTriangle(new Triangle(p0X, p0Y, p1X, p1Y, p2X, p2Y));
            
            Assert.AreEqual(totalAmount, objs.Length);

            for (int i = 0; i < types.Length; i++)
            {
                Assert.AreEqual(amounts[i], objs.Count(o => o.ObjectType == types[i]));
            }
        }
    }
}