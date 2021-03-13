using AutomatedCar.Models.Enums;
using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System.Numerics;
using Xunit;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    public class IntegratorTests
    {
        private readonly Mock<IVehicleConstants> constants = new Mock<IVehicleConstants>();

        public IntegratorTests()
        {
            constants.Setup(C => C.CurbWeight).Returns(10);
            constants.Setup(C => C.WheelBase).Returns(2);
        }

        [Fact]
        public void NextVehicleTransformPropertyReturnsInitialTransform()
        {
            var initialTransform = new VehicleTransform(Vector2.Zero, 0, Vector2.Zero, 0);
            var integrator = new Integrator(constants.Object);

            integrator.Reset(initialTransform, 0f, Gear.D);

            var nextTransform = integrator.NextVehicleTransform;

            Assert.Equal(initialTransform, nextTransform);
        }

        [Fact]
        public void ForceAppliedIncreasesVelocity()
        {
            var initialTransform = new VehicleTransform(Vector2.Zero, 0, Vector2.Zero, 0);
            var integrator = new Integrator(constants.Object);
            var deltaTime = 0.1f;

            integrator.Reset(initialTransform, deltaTime, Gear.D);
            integrator.AccumulateForce(WheelKind.Front, Vector2.UnitX);
            integrator.AccumulateForce(WheelKind.Back, Vector2.UnitX);
            var nextTransform = integrator.NextVehicleTransform;

            Assert.True(nextTransform.Velocity.Length() > initialTransform.Velocity.Length());
        }

        [Fact]
        public void InverseForceAppliedDecreasesVelocity()
        {
            var initialTransform = new VehicleTransform(Vector2.Zero, 0, Vector2.UnitX, 0);
            var integrator = new Integrator(constants.Object);
            var deltaTime = 0.1f;

            integrator.Reset(initialTransform, deltaTime, Gear.D);
            integrator.AccumulateForce(WheelKind.Front, -initialTransform.Velocity);
            integrator.AccumulateForce(WheelKind.Back, -initialTransform.Velocity);
            var nextTransform = integrator.NextVehicleTransform;

            Assert.True(nextTransform.Velocity.Length() < initialTransform.Velocity.Length());
        }

        [Fact]
        public void TorqueTest()
        {
            // Apply two forces on each end of the vehicle, such that the front
            // is pushed in the Y+ direction and the back is pushed in the Y-
            // direction.
            // This should make the body (in absence of other forces) rotate
            // around its center of mass.
            //
            //      ↓
            //     +--------+
            //     |        |
            //     +--------+
            //            ↑

            var initialTransform = new VehicleTransform(Vector2.Zero, 0, Vector2.Zero, 0);
            var integrator = new Integrator(constants.Object);
            var deltaTime = 0.1f;

            integrator.Reset(initialTransform, deltaTime, Gear.D);
            integrator.AccumulateForce(WheelKind.Front, +10 * Vector2.UnitY);
            integrator.AccumulateForce(WheelKind.Back, -10 * Vector2.UnitY);
            var nextTransform = integrator.NextVehicleTransform;

            Assert.True(nextTransform.AngularDisplacement > initialTransform.AngularDisplacement);
        }

        [Fact]
        public void TractiveForceRotatesTheVehicle()
        {
            var initialTransform = new VehicleTransform(Vector2.Zero, 0, Vector2.UnitX, 0);
            var integrator = new Integrator(constants.Object);
            var deltaTime = 0.01f;

            integrator.Reset(initialTransform, deltaTime, Gear.D);
            integrator.AccumulateForce(WheelKind.Front, 10 * new Vector2(1, 2));
            var nextTransform = integrator.NextVehicleTransform;

            Assert.True(nextTransform.AngularDisplacement > initialTransform.AngularDisplacement);
        }
    }
}
