using AutomatedCar.SystemComponents.Powertrain;
using Xunit;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    /// <summary>
    /// These tests make sure that the vehicle constants have more or less
    /// sensible values, i.e. having a transmission efficiency of less than 0% or
    /// more than a 100% would be impossible.
    /// </summary>
    public class VehicleConstantsTests
    {
        private IVehicleConstants constants = new VehicleConstants();

        [Fact]
        public void CarHasAtLeastOneGear()
        {
            Assert.True(constants.GearRatios.Length > 1);
        }

        [Fact]
        public void NumberOfGearsAndLengthOfGearRatiosMatches()
        {
            Assert.Equal(constants.NumberOfGears, constants.GearRatios.Length);
        }

        [Fact]
        public void ReverseGearRatioIsGreaterThanZero()
        {
            Assert.True(constants.ReverseGearRatio > 0);
        }

        [Fact]
        public void DifferentialRatioIsGreaterThanZero()
        {
            Assert.True(constants.DifferentialRatio > 0);
        }

        [Fact]
        public void TransmissionEfficiencyIsInRange()
        {
            Assert.True(0 < constants.TransmissionEfficiency && constants.TransmissionEfficiency <= 1);
        }

        [Fact]
        public void OverallWheelRadiusIsGreaterThanZero()
        {
            Assert.True(constants.OverallWheelRadius > 0);
        }

        [Fact]
        public void AirDensityIsPositive()
        {
            Assert.True(constants.AirDensity >= 0);
        }

        [Fact]
        public void DragCoefficientIsPositive()
        {
            Assert.True(constants.DragCoefficient > 0);
        }

        [Fact]
        public void CrossSectionalAreaIsPositive()
        {
            Assert.True(constants.CrossSectionalArea > 0);
        }

        [Fact]
        public void BrakingConstantIsPositive()
        {
            Assert.True(constants.BrakingConstant > 0);
        }

        [Fact]
        public void CurbWeightIsPositive()
        {
            Assert.True(constants.CurbWeight > 0);
        }

        [Fact]
        public void WheelBaseIsPositive()
        {
            Assert.True(constants.WheelBase > 0);
        }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(1.0f)]
        public void CrankshaftSpeedIsPositive(float gasPedal)
        {
            Assert.True(constants.GetCrankshaftSpeed(gasPedal) >= 0);
        }

        [Theory]
        [InlineData(1000.0f)]
        [InlineData(4000.0f)]
        [InlineData(6000.0f)]
        public void EngineTorqueIsPositive(float rpm)
        {
            Assert.True(constants.GetEngineTorque(rpm) > 0);
        }
    }
}
