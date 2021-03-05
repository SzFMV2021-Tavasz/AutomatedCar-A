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
        [Fact]
        public  void PowertrainAutomaticallyGetsRegisteredToVFB()
        {
            Mock<IVirtualFunctionBus> mockBus = new Mock<IVirtualFunctionBus>();
            PowertrainComponent ptr = new PowertrainComponent(mockBus.Object);

            mockBus.Verify(m => m.RegisterComponent(ptr), Times.Once);
        }

        [Fact]
        public void VFBGetsPowertrainPacketAfterCreatingPowertrainComponent()
        {
            VirtualFunctionBus virtualFunctionBus = new VirtualFunctionBus();
            PowertrainComponent powertrain = new PowertrainComponent(virtualFunctionBus);

            Assert.True(virtualFunctionBus.PowertrainPacket != null);
        }
    }
}
