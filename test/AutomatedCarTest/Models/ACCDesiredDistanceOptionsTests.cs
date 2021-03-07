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
    }
}
