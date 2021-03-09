﻿using AutomatedCar.Models;
using AutomatedCar.SystemComponents;
using AutomatedCar.SystemComponents.Packets;
using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.SystemComponents.Powertrain
{
    public class CarUpdaterTests
    {
        private IVirtualFunctionBus vfb = new VirtualFunctionBus();
        private Mock<IIntegrator> mockIntegrator = new Mock<IIntegrator>();

        [Fact]
        public void CarUpdaterUpdatesTheControlledCarProperties()
        {
            Vector2 desiredVelocity = new Vector2(4, 3);
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(new VehicleTransform(new Vector2(4, 4), 34, desiredVelocity, 15.1f));

            CarUpdater carUpdater = new CarUpdater(vfb, null, mockIntegrator.Object, null);
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();

            Assert.True(World.Instance.ControlledCar.X == 4);
            Assert.True(World.Instance.ControlledCar.Y == 4);
            Assert.True(World.Instance.ControlledCar.Velocity == desiredVelocity);
            Assert.True(World.Instance.ControlledCar.AngularVelocity == 15.1f);
            Assert.True(World.Instance.ControlledCar.CarHeading == 34);
            Assert.True(World.Instance.ControlledCar.Speed == desiredVelocity.Length());
        }

        [Fact]
        public void CarUpdaterUpdatesPowertrainComponentPacket()
        {
            PowertrainComponentPacket packet = new PowertrainComponentPacket();
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            Vector2 desiredVelocity = new Vector2(4, 3);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(new VehicleTransform(new Vector2(4, 4), 34.3f, desiredVelocity, 15.1f));

            CarUpdater carUpdater = new CarUpdater(vfb, null, mockIntegrator.Object, packet);
            carUpdater.SetCurrentTransform();
            carUpdater.UpdatePacket();

            Assert.True(packet.CarHeadingAngle == 34.3f);
            Assert.True(packet.Speed == desiredVelocity.Length());
            Assert.True(packet.X == 4);
            Assert.True(packet.Y == 4);
        }

        [Theory]
        [InlineData(PacketEnum.AEB)]
        [InlineData(PacketEnum.HMI)]
        public void CalculateInvokesForceCalculatingMethodsAccordingToPacketPriority(PacketEnum highestPriorityPacket)
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            Mock<IPriorityChecker> mockPrioChecker = new Mock<IPriorityChecker>();
            mockPrioChecker.Setup(m => m.AccelerationPriorityCheck()).Returns(highestPriorityPacket);
            Mock<IVehicleForces> mockedVehicleForces = new Mock<IVehicleForces>();
            Vector2 currentVelocity = Vector2.Zero;
            Vector2 currentWheelDirection = Vector2.Zero;

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null);
            carUpdater.Calculate();

            if (highestPriorityPacket == PacketEnum.AEB)
            {
                mockIntegrator.Verify(m => m.AccumulateForce(WheelKind.Front, mockedVehicleForces.Object.GetBrakingForce(100, currentVelocity)), Times.Once);
            }
            else if (highestPriorityPacket == PacketEnum.HMI)
            {
                mockIntegrator.Verify(m => m.AccumulateForce(WheelKind.Front, mockedVehicleForces.Object.GetBrakingForce(100, currentVelocity)), Times.Once);
                mockIntegrator.Verify(m => m.AccumulateForce(WheelKind.Front, mockedVehicleForces.Object.GetTractiveForce(100, currentWheelDirection, 1)), Times.Once);
            }
        }
    }
}
