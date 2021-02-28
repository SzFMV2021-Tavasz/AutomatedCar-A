using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System;
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
        public void DragIsProportionalToSpeed()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var vel0 = Vector2.Zero;
            var vel1 = new Vector2(0, 28);

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
