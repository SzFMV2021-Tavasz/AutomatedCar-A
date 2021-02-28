using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    public class VehicleForcesTests
    {
        private readonly Mock<IVehicleConstants> constants = new Mock<IVehicleConstants>();

        public VehicleForcesTests()
        {
            constants.Setup(C => C.AirDensity).Returns(1.184f);
            constants.Setup(C => C.DragCoefficient).Returns(0.280f);
            constants.Setup(C => C.CrossSectionalArea).Returns(2.642f);
        }

        [Fact]
        public void DragIsZeroAtZeroVelocity()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var vel = Vector2.Zero;

            var force = forceCalculator.GetDragForce(vel);

            Assert.True(force.Length() < float.Epsilon);
        }

        public static IEnumerable<object[]> dragProportionalTestVectors =>
            new List<object[]>
            {
                new object[] { Vector2.Zero,        new Vector2(0, 28) },
                new object[] { new Vector2(0, 10),  new Vector2(0, 28) },
            };

        [Theory]
        [MemberData(nameof(dragProportionalTestVectors))]
        public void DragIsProportionalToSpeed(Vector2 vel0, Vector2 vel1)
        {
            var forceCalculator = new VehicleForces(constants.Object);

            var force0 = forceCalculator.GetDragForce(vel0);
            var force1 = forceCalculator.GetDragForce(vel1);

            Assert.True(force0.Length() < force1.Length());
        }

        [Fact]
        public void DragPointsInTheOppositeDirectionOfTheVelocityVector()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var vel = new Vector2(0, 28);

            var force = forceCalculator.GetDragForce(vel);

            // NOTE(danielm): dot product of two units vectors is -1 if the
            // angle between them is 180deg.
            var dot = Vector2.Dot(Vector2.Normalize(force), Vector2.Normalize(vel));
            var diff = Math.Abs(-1 - dot);

            Assert.True(diff < float.Epsilon);
        }
    }
}
