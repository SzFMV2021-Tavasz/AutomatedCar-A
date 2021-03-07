using AutomatedCar.Models;
using Xunit;

namespace Test.Models
{
    public class ACCDesiredDistanceOptionsTests
    {
        private double[] _options;
        private ACCDesiredDistanceOptions _desiredDistanceOptions;

        public ACCDesiredDistanceOptionsTests()
        {
            this._options = new[] { 1.0, 2.0, 3.0, 4.0 };
            this._desiredDistanceOptions = new ACCDesiredDistanceOptions(this._options);
        }

        [Fact]
        public void GetDefault_ShouldReturn_FirstElementFromOptions()
        {
            // Arrange

            // Act
            var returned = this._desiredDistanceOptions.GetDefault();

            // Assert
            Assert.Equal(1.0, returned, 1);
        }

        [Fact]
        public void GetNext_ShouldReturnFirstElement_WhenFirstCalled()
        {
            // Arrange 

            // Act
            var returnedFromFirstCall = this._desiredDistanceOptions.GetNext();

            // Assert
            Assert.Equal(1.0, returnedFromFirstCall, 1);
        }

        [Fact]
        public void GetNext_ShouldReturnTheFirst_WhenCalledWhenOnLast()
        {
            // Arrange

            // Act
            this._desiredDistanceOptions.GetNext();
            this._desiredDistanceOptions.GetNext();
            this._desiredDistanceOptions.GetNext();
            this._desiredDistanceOptions.GetNext();
            var returnedFromCallOnLastIndex = this._desiredDistanceOptions.GetNext();

            // Assert
            Assert.Equal(1.0, returnedFromCallOnLastIndex, 1);
        }

        [Fact]
        public void GetPrevious_ShouldReturnTheLast_WhenFirstCalled()
        {
            // Arrange 

            // Act
            var returnedFromFirstCall = this._desiredDistanceOptions.GetPrevious();

            // Assert
            Assert.Equal(4.0, returnedFromFirstCall, 1);
        }

        [Fact]
        public void GetPrevious_ShouldReturnTheLast_WhenCalledWhenOnFirst()
        {
            // Arrange

            // Act
            this._desiredDistanceOptions.GetNext();
            var returnedFromCallOnFirstIndex = this._desiredDistanceOptions.GetPrevious();

            // Assert
            Assert.Equal(4.0, returnedFromCallOnFirstIndex, 1);
        }
    }
}
