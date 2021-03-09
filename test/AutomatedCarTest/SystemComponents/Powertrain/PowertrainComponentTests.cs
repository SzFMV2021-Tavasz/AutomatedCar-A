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
        Mock<IPriorityChecker> mockedPriorityChecker = new Mock<IPriorityChecker>();

        IVirtualFunctionBus virtualFunctionBus = new VirtualFunctionBus();

        [Fact]
        public  void PowertrainAutomaticallyGetsRegisteredToVFB()
        {
            PowertrainComponent ptr = new PowertrainComponent(mockBus.Object, null, null, null);
            mockBus.Verify(m => m.RegisterComponent(ptr), Times.Once);
        }

        [Fact]
        public void VFBGetsPowertrainPacketAfterCreatingPowertrainComponent()
        {
            PowertrainComponent powertrain = new PowertrainComponent(virtualFunctionBus, null, null, null);
            Assert.True(virtualFunctionBus.PowertrainPacket != null);
        }
    }
}