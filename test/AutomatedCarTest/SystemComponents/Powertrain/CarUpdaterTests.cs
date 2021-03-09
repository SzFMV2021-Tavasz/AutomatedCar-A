using AutomatedCar.Models;
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

            CarUpdater carUpdater = new CarUpdater(vfb ,null, mockIntegrator.Object, null);
            carUpdater.SetCurrentTransform();
            carUpdater.UpdateWorldObject();

            Assert.True(World.Instance.ControlledCar.X == 4);
            Assert.True(World.Instance.ControlledCar.Y == 4);
            Assert.True(World.Instance.ControlledCar.Velocity == desiredVelocity);
            Assert.True(World.Instance.ControlledCar.AngularVelocity == 15.1f);
            Assert.True(World.Instance.ControlledCar.CarHeading == 34);
            Assert.True(World.Instance.ControlledCar.Speed == desiredVelocity.Length());
        }
    }
}
