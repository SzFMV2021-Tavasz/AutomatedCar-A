using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatedCar.SystemComponents.Powertrain;
using AutomatedCar.SystemComponents;
using Moq;
using Xunit;

namespace Test.SystemComponents.Powertrain
{
    public class PowertrainComponentTests
    {
        Mock<IVirtualFunctionBus> mockBus = new Mock<IVirtualFunctionBus>();

        IVirtualFunctionBus virtualFunctionBus = new VirtualFunctionBus();

        [Fact]
        public  void PowertrainAutomaticallyGetsRegisteredToVFB()
        {
            PowertrainComponent ptr = new PowertrainComponent(mockBus.Object);
            mockBus.Verify(m => m.RegisterComponent(ptr), Times.Once);
        }

        [Fact]
        public void VFBGetsPowertrainPacketAfterCreatingPowertrainComponent()
        {
            PowertrainComponent powertrain = new PowertrainComponent(virtualFunctionBus);
            Assert.True(virtualFunctionBus.PowertrainPacket != null);
        }
    }
}