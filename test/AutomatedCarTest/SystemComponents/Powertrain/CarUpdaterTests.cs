﻿using AutomatedCar.Models;
using AutomatedCar.Models.Enums;
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
        private Mock<IPriorityChecker> mockPrioChecker = new Mock<IPriorityChecker>();
        private Mock<IVehicleForces> mockedVehicleForces = new Mock<IVehicleForces>();
        private ITransmission transmission = new Transmission();

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
            Assert.True(World.Instance.ControlledCar.Speed == desiredVelocity.Length() * 3.6);
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
            Assert.True(packet.Speed == desiredVelocity.Length() * 3.6);
            Assert.True(packet.X == 4);
            Assert.True(packet.Y == 4);
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
            mockedVehicleForces.Setup(m => m.GetBrakingForce(1f, currentVelocity)).Returns(new Vector2(2, 5));

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null);
            carUpdater.transmission = transmission;
            carUpdater.priorityChecker = mockPrioChecker.Object;
            carUpdater.SetCurrentTransform();
            carUpdater.Calculate();

            mockIntegrator.Verify(m => m.AccumulateForce(WheelKind.Front, mockedVehicleForces.Object.GetBrakingForce(1f, currentVelocity)), Times.Once);
            
        }

        [Fact]
        public void CalculateResetsIntegrator()
        {
            World.Instance.ControlledCar = new AutomatedCar.Models.AutomatedCar(0, 0, "");
            VehicleTransform vehicleTransform = new VehicleTransform(new Vector2(4, 4), 34.3f, Vector2.Zero, 15.1f);
            mockIntegrator.Setup(m => m.NextVehicleTransform).Returns(vehicleTransform);
            vfb.HMIPacket = new HMIPacket(0, 0, 0, Gear.D);

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null);
            carUpdater.transmission = transmission;
            carUpdater.SetCurrentTransform();
            carUpdater.Calculate();

            mockIntegrator.Verify(m => m.Reset(vehicleTransform, 0.05f), Times.Once);
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
            mockedVehicleForces.Setup(m => m.GetDragForce(velocity)).Returns(new Vector2(2, 5));
            mockedVehicleForces.Setup(m => m.GetWheelDirectionHackForce(wheelDirection, velocity)).Returns(new Vector2(1, 1));

            CarUpdater carUpdater = new CarUpdater(vfb, mockedVehicleForces.Object, mockIntegrator.Object, null);
            carUpdater.transmission = transmission;
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();
            carUpdater.Calculate();

            mockIntegrator.Verify(mockIntegrator => mockIntegrator.AccumulateForce(WheelKind.Front, mockedVehicleForces.Object.GetDragForce(velocity)), Times.Once);
            mockIntegrator.Verify(mockIntegrator => mockIntegrator.AccumulateForce(WheelKind.Back, mockedVehicleForces.Object.GetDragForce(velocity)), Times.Once);
            mockIntegrator.Verify(mockIntegrator => mockIntegrator.AccumulateForce(WheelKind.Front, mockedVehicleForces.Object.GetWheelDirectionHackForce(wheelDirection, velocity)), Times.Once);
            mockIntegrator.Verify(mockIntegrator => mockIntegrator.AccumulateForce(WheelKind.Back, mockedVehicleForces.Object.GetWheelDirectionHackForce(wheelDirection, velocity)), Times.Once);
        }

    }
}