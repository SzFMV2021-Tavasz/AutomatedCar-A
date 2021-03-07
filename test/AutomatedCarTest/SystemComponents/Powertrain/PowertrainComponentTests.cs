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
            PowertrainComponent ptr = new PowertrainComponent(mockBus.Object, mockedPriorityChecker.Object, null);
            mockBus.Verify(m => m.RegisterComponent(ptr), Times.Once);
        }

        [Fact]
        public void VFBGetsPowertrainPacketAfterCreatingPowertrainComponent()
        {
            PowertrainComponent powertrain = new PowertrainComponent(virtualFunctionBus, mockedPriorityChecker.Object, null);
            Assert.True(virtualFunctionBus.PowertrainPacket != null);
        }

        [Fact]
        public void PriorityCheckerCheckingMethodsInvokedInPowertrainComponentProcessMethod()
        {
            PowertrainComponent powertrainComponent = new PowertrainComponent(virtualFunctionBus, mockedPriorityChecker.Object, null);
            powertrainComponent.Process();
            mockedPriorityChecker.Verify(c => c.AccelerationPriorityCheck(), Times.Once);
            mockedPriorityChecker.Verify(c => c.SteeringPriorityCheck(), Times.Once);
        }

    }
}
