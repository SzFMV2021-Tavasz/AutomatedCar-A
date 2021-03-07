using System.Collections.Generic;
using System.Linq;

namespace AutomatedCar.Models
{
    public class ACCDesiredDistanceOptions
    {
        private IReadOnlyList<double> DesiredDistanceOptionsInSeconds = new double[] { 0.8, 1.0, 1.2, 1.4 };
        private int _currentIndex;

        public ACCDesiredDistanceOptions(IEnumerable<double> desiredDistanceOptions = null)
        {
            if (desiredDistanceOptions is not null)
            {
                this.DesiredDistanceOptionsInSeconds = desiredDistanceOptions.ToArray();
            }

            this._currentIndex = -1;
        }

        public double GetDefault() => this.DesiredDistanceOptionsInSeconds[0];

        public double GetNext()
        {
            this._currentIndex = (this._currentIndex + 1) % this.DesiredDistanceOptionsInSeconds.Count;
            return this.DesiredDistanceOptionsInSeconds[this._currentIndex];
        }

        public double GetPrevious()
        {
            return 0;
        }
    }
}
