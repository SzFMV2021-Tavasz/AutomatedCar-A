using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System.Numerics;
using Xunit;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    public class VehicleForcesTests
    {
        [Fact]
        public void DragIsProportionalToSpeed()
        {
            var constants = new Mock<IVehicleConstants>();
            constants.Setup(C => C.AirDensity).Returns(1.184f);
            constants.Setup(C => C.DragCoefficient).Returns(0.280f);
            constants.Setup(C => C.CrossSectionalArea).Returns(2.642f);
            var forceCalculator = new VehicleForces(constants.Object);
            var vel0 = Vector2.Zero;
            var vel1 = new Vector2(0, 28);
            var wheelRotation = 0f;

            var force0 = forceCalculator.GetDragForce(wheelRotation, vel0);
            var force1 = forceCalculator.GetDragForce(wheelRotation, vel1);

            Assert.True(force0.Length() < force1.Length());
        }
    }
}
