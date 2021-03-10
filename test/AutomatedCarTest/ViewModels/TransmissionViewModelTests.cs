using AutomatedCar.Models.Enums;
using AutomatedCar.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace Test.ViewModels
{
    public class TransmissionViewModelTests
    {
        private readonly IReadOnlyList<Gear> GearsInTheRightOrder = new[] { Gear.P, Gear.N, Gear.D, Gear.R };
        private readonly TransmissionViewModel _transmissionViewModel;

        public TransmissionViewModelTests()
        {
            this._transmissionViewModel = new TransmissionViewModel();
        }

        [Fact]
        public void DefaultShouldBeParking_WhenViewModelCreated()
        {
            // Arrange
            var expected = Gear.P;

            // Act

            // Assert
            Assert.Equal(expected, this._transmissionViewModel.CurrentGear);
        }

        [Fact]
        public void ShiftUp_ShouldNotDoAnything_WhenCalledOnReverse()
        {
            // Arrange
            var expected = Gear.R;

            // Act
            for (int i = 0; i < 10; i++) this._transmissionViewModel.ShiftUp();

            // Assert
            Assert.Equal(expected, this._transmissionViewModel.CurrentGear);
        }

        [Fact]
        public void ShiftUp_ShouldChangeGearsSequentially_WhenCalled()
        {
            // Arrange         

            // Act 
            for (int i = 1; i < this.GearsInTheRightOrder.Count; i++)
            {
                this._transmissionViewModel.ShiftUp();
                var gearAfterShifting = this._transmissionViewModel.CurrentGear;
                Assert.Equal(this.GearsInTheRightOrder[i], gearAfterShifting);
            }
        }

        [Fact]
        public void ShiftDown_ShouldNotDoAnything_WhenCalledOnParking()
        {
            // Arrange
            var expected = Gear.P;

            // Act
            for (int i = 0; i < 10; i++) this._transmissionViewModel.ShiftDown();

            // Assert
            Assert.Equal(expected, this._transmissionViewModel.CurrentGear);
        }

        [Fact]
        public void ShiftDown_ShouldChangeGearsSequentially_WhenCalled()
        {
            // Arrange         
            for (int i = 0; i < this.GearsInTheRightOrder.Count; i++) this._transmissionViewModel.ShiftUp();

            // Act 
            for (int i = this.GearsInTheRightOrder.Count - 2; i >= 0; i--)
            {
                this._transmissionViewModel.ShiftDown();
                var gearAfterShifting = this._transmissionViewModel.CurrentGear;
                Assert.Equal(this.GearsInTheRightOrder[i], gearAfterShifting);
            }
        }
    }
}
