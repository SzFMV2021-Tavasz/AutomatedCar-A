using AutomatedCar.ViewModels;
using Xunit;

namespace Test.ViewModels
{
    public class ACCOptionsViewModelTests
    {
        private ACCOptionsViewModel _accOptionsViewModel;
        public ACCOptionsViewModelTests()
        {
            this._accOptionsViewModel = new ACCOptionsViewModel();
        }

        [Fact]
        public void IncreaseDesiredSpeed_ShouldNotGoAbove160()
        {
            // Arrange

            // Act
            for (int i = 0; i < 50; i++)
            {
                this._accOptionsViewModel.IncreaseDesiredSpeed();
            }

            // Assert
            Assert.Equal(160, this._accOptionsViewModel.DesiredSpeed);
        }

        [Fact]
        public void DecreaseDesiredSpeed_ShouldNotGoBelow30()
        {
            // Arrange

            // Act
            for (int i = 0; i < 50; i++)
            {
                this._accOptionsViewModel.DecreaseDesiredSpeed();
            }

            // Assert
            Assert.Equal(30, this._accOptionsViewModel.DesiredSpeed);
        }

        [Fact]
        public void Toggle_ShouldChangeValue_WhenCalled()
        {
            // Arrange
            var expected = !this._accOptionsViewModel.IsTurnedOn;

            // Act
            this._accOptionsViewModel.Toggle();

            // Assert
            Assert.Equal(expected, this._accOptionsViewModel.IsTurnedOn);
        }
    }
}
