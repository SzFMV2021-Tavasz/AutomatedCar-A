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
            transmission.Gear = AutomatedCar.Models.Enums.Gear.D;

            for(int speed = 0; speed < 100; speed++)
            {
                var correctGear = transmission.DriveSpeedGearMapping.First(m => m.minSpeed <= speed && speed < m.maxSpeed).gear;
                transmission.SetInsideGear(speed);

                Assert.Equal(correctGear, transmission.InsideGear);
                Assert.Equal(AutomatedCar.Models.Enums.Gear.D, transmission.Gear);
            }
        }
    }
}
