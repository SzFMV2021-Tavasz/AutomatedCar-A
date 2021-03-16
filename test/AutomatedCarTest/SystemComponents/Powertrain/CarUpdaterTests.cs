using AutomatedCar.Models;
using AutomatedCar.Models.Enums;
using AutomatedCar.SystemComponents;
using AutomatedCar.SystemComponents.Packets;
using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System;
using System.Numerics;
using Xunit;

namespace Test.SystemComponents.Powertrain
{
    public class CarUpdaterTests
    {
        private IVirtualFunctionBus vfb = new VirtualFunctionBus();
        private Mock<IIntegrator> mockIntegrator = new Mock<IIntegrator>();
        private Mock<IPriorityChecker> mockPrioChecker = new Mock<IPriorityChecker>();
        private Mock<IVehicleForces> mockedVehicleForces = new Mock<IVehicleForces>();
        private ITransmission transmission = new Transmission();

        static void AssertErrorLessThan(double v0, double v1, double error)
        {
            Assert.True(Math.Abs(v1 - v0) < error);
        }

        static void AssertErrorLessThan(Vector2 v0, Vector2 v1, double error)
        {
            Assert.True((v1 - v0).Length() < error);
        }

        [Fact]
        public void VelocityAngularVelocityAndSpeedAreUpdatedInAutomatedCar()
        {
            var desiredVelocity = new Vector2(4, 3);
            var desiredSpeedKmh = desiredVelocity.Length() * 3.6f;
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            var desiredHeading = (float)(Math.PI / 2);
            var desiredAngularVelocity = 15.1f;
            var desiredPositionMeters = new Vector2(192, 192);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(new VehicleTransform(desiredPositionMeters, desiredHeading, desiredVelocity, desiredAngularVelocity));

            CarUpdater carUpdater = new CarUpdater(vfb, null, mockIntegrator.Object, null, new VehicleConstants(), 1.0f);
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();

            AssertErrorLessThan(desiredVelocity, World.Instance.ControlledCar.Velocity, 0.01f);
            AssertErrorLessThan(desiredAngularVelocity, World.Instance.ControlledCar.AngularVelocity, 0.01);
            AssertErrorLessThan(desiredSpeedKmh, World.Instance.ControlledCar.Speed, 1.5);
        }

        [Fact]
        public void CarUpdaterUpdatesPowertrainComponentPacket()
        {
            PowertrainComponentPacket packet = new PowertrainComponentPacket();
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            Vector2 desiredVelocity = new Vector2(4, 3);
            var desiredSpeedKmh = desiredVelocity.Length() * 3.6f;
            var desiredPositionMeters = new Vector2(192, 192);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(new VehicleTransform(desiredPositionMeters, 34.3f, desiredVelocity, 15.1f));
            var desiredSteeringWheel = 0;
            vfb.HMIPacket = new HMIPacket(0, 0, desiredSteeringWheel, Gear.D);

            CarUpdater carUpdater = new CarUpdater(vfb, null, mockIntegrator.Object, packet, new VehicleConstants(), 1.0f);
            var desiredPositionUnits = desiredPositionMeters * carUpdater.UnitsPerMeters;
            carUpdater.SetCurrentTransform();
            carUpdater.UpdatePacket();

            Assert.Equal(desiredSteeringWheel, packet.SteeringWheelAngleDegrees);
            AssertErrorLessThan(desiredSpeedKmh, packet.Speed, 1.5);
            Assert.Equal(desiredPositionUnits.X, packet.X);
            Assert.Equal(desiredPositionUnits.Y, packet.Y);
        }

        [Fact]
        public void CarUpdatesWritesSteeringWheelAngleInDegrees()
        {
            var steeringWheelMaxRotationDegrees = 60;
            var inputSteeringWheel = 30;
            var outputSteeringWheelAngleDegrees = (inputSteeringWheel * steeringWheelMaxRotationDegrees) / 100.0;
            var packet = new PowertrainComponentPacket();
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(new VehicleTransform(Vector2.Zero, 0, Vector2.Zero, 0));
            vfb.HMIPacket = new HMIPacket(0, 0, inputSteeringWheel, Gear.D);

            var vehicleConstants = new VehicleConstants();
            var vehicleForces = new VehicleForces(vehicleConstants);
            CarUpdater carUpdater = new CarUpdater(vfb, vehicleForces, mockIntegrator.Object, packet, vehicleConstants, 1.0f);
            carUpdater.Calculate();
            carUpdater.SetCurrentTransform();
            carUpdater.UpdatePacket();

            Assert.Equal(outputSteeringWheelAngleDegrees, packet.SteeringWheelAngleDegrees);
        }

        [Fact]
        public void CalculateInvokesForceCalculatingMethodsAccordingToPacketPriority()
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            mockPrioChecker.Setup(m => m.AccelerationPriorityCheck()).Returns(PacketEnum.AEB);
            vfb.HMIPacket = new HMIPacket(0, 0, 0, Gear.D);

            Vector2 currentVelocity = new Vector2(2, 3);
            Vector2 currentWheelDirection = new Vector2(1, 0);
            VehicleTransform vehicleTransform = new VehicleTransform(new Vector2(4, 4), 0, currentVelocity, 15.1f);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(vehicleTransform);
            var brakingForce = new Vector2(2, 5);
            mockedVehicleForces.Setup(m => m.GetBrakingForce(1f, It.IsAny<Vector2>())).Returns(brakingForce);

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null, null, 1.0f);
            carUpdater.transmission = transmission;
            carUpdater.priorityChecker = mockPrioChecker.Object;
            carUpdater.Calculate();
            carUpdater.SetCurrentTransform();

            mockIntegrator.Verify(m => m.AccumulateForce(WheelKind.Front, brakingForce), Times.Once);
        }

        [Fact]
        public void CalculateResetsIntegrator()
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            VehicleTransform vehicleTransform = new VehicleTransform(new Vector2(4, 4), 34.3f, Vector2.Zero, 15.1f);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(vehicleTransform);
            vfb.HMIPacket = new HMIPacket(0, 0, 0, Gear.D);

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null, null, 1.0f);
            carUpdater.transmission = transmission;
            carUpdater.SetCurrentTransform();
            carUpdater.Calculate();

            mockIntegrator.Verify(m => m.Reset(vehicleTransform, It.IsAny<float>(), Gear.D), Times.Once);
        }

        [Fact]
        public void CalculateInvokesStaticForceCalculatingMethods()
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            vfb.HMIPacket = new HMIPacket(0, 0, 0, Gear.D);
            Vector2 velocity = new Vector2(2, 3);
            Vector2 wheelDirection = new Vector2(1, 0);
            VehicleTransform vehicleTransform = new VehicleTransform(new Vector2(4, 4), 0, velocity, 15.1f);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(vehicleTransform);
            var brakingForce = new Vector2(2, 5);
            mockedVehicleForces.Setup(m => m.GetDragForce(It.IsAny<Vector2>())).Returns(brakingForce);

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null, null, 1.0f);
            carUpdater.transmission = transmission;
            carUpdater.Calculate();
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();

            mockIntegrator.Verify(mockIntegrator => mockIntegrator.AccumulateForce(WheelKind.Front, brakingForce), Times.Once);
            mockIntegrator.Verify(mockIntegrator => mockIntegrator.AccumulateForce(WheelKind.Back, brakingForce), Times.Once);
        }

        [Fact]
        public void MetersAreConvertedToWorldUnits()
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            var nextPositionMeters = new Vector2(96, 0);
            var nextVelocity = Vector2.Zero;
            var wheelDirection = Vector2.UnitX;
            var nextVehicleTransform = new VehicleTransform(nextPositionMeters, 0, nextVelocity, 0);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(nextVehicleTransform);

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null, null, 1.0f);
            var nextPositionUnits = nextPositionMeters * carUpdater.UnitsPerMeters;
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();

            Assert.Equal(nextPositionUnits.X, World.Instance.ControlledCar.X);
            Assert.Equal(nextPositionUnits.Y, World.Instance.ControlledCar.Y);
        }

        [Theory]
        [InlineData(0f, Math.PI / 2f)]
        [InlineData(Math.PI / 2f, Math.PI)]
        [InlineData(Math.PI, 3f * Math.PI / 2f)]
        [InlineData(2 * Math.PI, Math.PI / 2f)]
        public void CarAngleIsOffsetBy90Degrees(float internalAngle, float expectedAngle)
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            var nextPosition = new Vector2(0, 0);
            var nextVelocity = Vector2.Zero;
            var wheelDirection = Vector2.UnitX;
            var nextVehicleTransform = new VehicleTransform(nextPosition, internalAngle, nextVelocity, 0);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(nextVehicleTransform);

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null, null, 1.0f);
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();

            AssertErrorLessThan(expectedAngle, World.Instance.ControlledCar.CarHeading, 0.005f);
        }
    }
}
