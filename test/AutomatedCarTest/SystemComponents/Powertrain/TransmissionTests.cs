using AutomatedCar.Models.Enums;
using AutomatedCar.SystemComponents.Powertrain;
using System.Linq;
using Xunit;

namespace Test.SystemComponents.Powertrain
{
    public class TransmissionTests
    {
        [Fact]
        public void TransmissionSwitchesToCorrectGearInDriveMode()
        {
            var transmission = new Transmission();
            transmission.Gear = Gear.D;

            for(int speed = 0; speed < 100; speed++)
            {
                var correctGear = transmission.DriveSpeedGearMapping.First(m => m.minSpeed <= speed && speed < m.maxSpeed).gear;
                transmission.SetInsideGear(speed);

                Assert.Equal(correctGear, transmission.InsideGear);
                Assert.Equal(Gear.D, transmission.Gear);
            }
        }

        [Theory]
        [InlineData(Gear.N)]
        [InlineData(Gear.P)]
        [InlineData(Gear.R)]
        public void NonDriveModesSetInternalGearToZero(Gear gear)
        {
            var transmission = new Transmission();
            transmission.Gear = gear;
            var reallyHighSpeed = 1000;

            transmission.SetInsideGear(reallyHighSpeed);

            Assert.Equal(0, transmission.InsideGear);
            Assert.Equal(gear, transmission.Gear);
        }
    }
}
