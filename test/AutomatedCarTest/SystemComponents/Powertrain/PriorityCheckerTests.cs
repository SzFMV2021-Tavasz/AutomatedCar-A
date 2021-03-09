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
        Mock<IReadOnlyHMIPacket> mockedHMI = new Mock<IReadOnlyHMIPacket>();
        Mock<IReadOnlyPPPacket> mockedPP = new Mock<IReadOnlyPPPacket>();
        Mock<IReadOnlyLKAPacket> mockedLKA = new Mock<IReadOnlyLKAPacket>();

        [Fact]
        public void SteeringWheelPriorityCheckingHMIPacket()
        {
            virtualFunctionBus.LKAPacket = mockedLKA.Object;
            virtualFunctionBus.HMIPacket = mockedHMI.Object;
            virtualFunctionBus.PPPacket = mockedPP.Object;
            PriorityChecker.virtualFunctionBus = virtualFunctionBus;

            Assert.True(PriorityChecker.SteeringPriorityCheck() == PacketEnum.HMI);
        }

        [Fact]
        public void SteeringWheelPriorityCheckingLKAPacket()
        {
            virtualFunctionBus.LKAPacket = mockedLKA.Object;
            PriorityChecker.virtualFunctionBus = virtualFunctionBus;

            Assert.True(PriorityChecker.SteeringPriorityCheck() == PacketEnum.LKA);
        }

        [Fact]
        public void SteeringWheelPriorityCheckingPPPacket()
        {
            virtualFunctionBus.PPPacket = mockedPP.Object;
            PriorityChecker.virtualFunctionBus = virtualFunctionBus;

            Assert.True(PriorityChecker.SteeringPriorityCheck() == PacketEnum.PP);
        }
    }
}
