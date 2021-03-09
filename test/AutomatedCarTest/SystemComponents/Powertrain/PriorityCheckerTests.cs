using AutomatedCar.SystemComponents;
using AutomatedCar.SystemComponents.Packets;
using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using Xunit;

namespace Test.SystemComponents.Powertrain
{
    public class PriorityCheckerTests
    {
        IPriorityChecker PriorityChecker = new PriorityChecker();
        IVirtualFunctionBus virtualFunctionBus = new VirtualFunctionBus();

        [Fact]
        public void SteeringWheelPriorityCheckingHMIPacket()
        {
            Mock<IReadOnlyHMIPacket> mockedHMI = new Mock<IReadOnlyHMIPacket>();
            virtualFunctionBus.HMIPacket = mockedHMI.Object;
            PriorityChecker.virtualFunctionBus = virtualFunctionBus;

            Assert.True(PriorityChecker.SteeringPriorityCheck() == PacketEnum.HMI);
        }

        [Fact]
        public void SteeringWheelPriorityCheckingLKAPacket()
        {
            Mock<IReadOnlyLKAPacket> mockedLKA = new Mock<IReadOnlyLKAPacket>();
            virtualFunctionBus.LKAPacket = mockedLKA.Object;
            PriorityChecker.virtualFunctionBus = virtualFunctionBus;

            Assert.True(PriorityChecker.SteeringPriorityCheck() == PacketEnum.LKA);
        }

        [Fact]
        public void SteeringWheelPriorityCheckingPPPacket()
        {
            Mock<IReadOnlyPPPacket> mockedPP = new Mock<IReadOnlyPPPacket>();
            virtualFunctionBus.PPPacket = mockedPP.Object;

            Assert.True(PriorityChecker.SteeringPriorityCheck() == PacketEnum.PP);
        }
    }
}
