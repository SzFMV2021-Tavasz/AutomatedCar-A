using AutomatedCar.Models.Enums;
using AutomatedCar.SystemComponents.Packets;
using NUnit.Framework;

namespace Test.SystemComponents.Packets
{
    public class HMIPacketTest
    {
        private IReadOnlyHMIPacket hmiPacket;

        [SetUp]
        public void SetUp()
        {
            this.hmiPacket = new HMIPacket(75, 50, 15, Gear.R);
        }

        [Test]
        public void GasPedal_Returns75()
        {
            Assert.That(hmiPacket.GasPedal, Is.EqualTo(75));
        }

        [Test]
        public void BrakePedal_Returns50()
        {
            Assert.That(hmiPacket.BrakePedal, Is.EqualTo(50));
        }

        [Test]
        public void SteeringWheelAngle_Returns15()
        {
            Assert.That(hmiPacket.SteeringWheelAngle, Is.EqualTo(15));
        }

        [Test]
        public void Gear_ReturnsR()
        {
            Assert.That(hmiPacket.Gear, Is.EqualTo(Gear.R));
        }
    }
}
