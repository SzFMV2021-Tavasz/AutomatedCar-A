using AutomatedCar.ViewModels;
using Xunit;

namespace Test.ViewModels
{
    public class LaneKeepingAndParkingPilotViewModelTests
    {
        private LaneKeepingAndParkingPilotViewModel _laneKeepingAndParkingPilotViewModel;

        public LaneKeepingAndParkingPilotViewModelTests()
        {
            this._laneKeepingAndParkingPilotViewModel = new LaneKeepingAndParkingPilotViewModel();
        }

        [Fact]
        public void ToggleLaneKeeping_ShouldChangeValue_WhenCalled()
        {
            // Arrange
            var expected = !this._laneKeepingAndParkingPilotViewModel.IsLaneKeepingTurnedOn;

            // Act
            this._laneKeepingAndParkingPilotViewModel.ToggleLaneKeeping();

            // Assert
            Assert.Equal(expected, this._laneKeepingAndParkingPilotViewModel.IsLaneKeepingTurnedOn);
        }

        [Fact]
        public void ToggleParkingPilot_ShouldChangeValue_WhenCalled()
        {
            // Arrange
            var expected = !this._laneKeepingAndParkingPilotViewModel.IsParkingPilotTurnedOn;

            // Act
            this._laneKeepingAndParkingPilotViewModel.ToggleParkingPilot();

            // Assert
            Assert.Equal(expected, this._laneKeepingAndParkingPilotViewModel.IsParkingPilotTurnedOn);
        }

        [Fact]
        public void DisplayLaneKeepingWarning_ShouldChangeFieldToTrue_WhenCalled()
        {
            // Arrange
           
            // Act
            this._laneKeepingAndParkingPilotViewModel.DisplayLaneKeepingWarning();

            // Assert
            Assert.True(this._laneKeepingAndParkingPilotViewModel.IsLaneKeepingWarningTurnedOn);
        }

        [Fact]
        public void TurnOffLaneKeepingWarning_ShouldChangeFieldToFalse_WhenCalled()
        {
            // Arrange

            // Act
            this._laneKeepingAndParkingPilotViewModel.TurnOffLaneKeepingWarning();

            // Assert
            Assert.False(this._laneKeepingAndParkingPilotViewModel.IsLaneKeepingWarningTurnedOn);
        }
    }
}
