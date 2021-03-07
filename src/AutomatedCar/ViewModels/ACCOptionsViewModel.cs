using AutomatedCar.Models;
using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class ACCOptionsViewModel : ViewModelBase
    {
        private const int MaxDesiredSpeed = 160;
        private const int MinDesiredSpeed = 30;
        private const int DesiredSpeedStep = 10;

        private readonly ACCDesiredDistanceOptions _accDesiredDistanceOptions;
        private double _desiredDistanceInSeconds;
        private int _desiredSpeed;
        private bool _isTurnedOn;

        public ACCOptionsViewModel()
        {
            this._accDesiredDistanceOptions = new ACCDesiredDistanceOptions();
            this.DesiredDistanceInSeconds = this._accDesiredDistanceOptions.GetDefault();
            this.DesiredSpeed = 50;
            this.IsTurnedOn = false;
        }

        public double DesiredDistanceInSeconds
        {
            get => this._desiredDistanceInSeconds;
            private set => this.RaiseAndSetIfChanged(ref this._desiredDistanceInSeconds, value);
        }

        public int DesiredSpeed
        {
            get => this._desiredSpeed;
            private set => this.RaiseAndSetIfChanged(ref this._desiredSpeed, value);
        }

        public bool IsTurnedOn
        {
            get => this._isTurnedOn;
            private set => this.RaiseAndSetIfChanged(ref this._isTurnedOn, value);
        }

        public void Toggle()
        {
            this.IsTurnedOn = !this.IsTurnedOn;
        }

        public void IncreaseDesiredSpeed()
        {
            if (this.DesiredSpeed + DesiredSpeedStep > MaxDesiredSpeed)
            {
                return;
            }

            this.DesiredSpeed += DesiredSpeedStep;
        }

        public void DecreaseDesiredSpeed() 
        {
            if (this.DesiredSpeed - DesiredSpeedStep < MinDesiredSpeed)
            {
                return;
            }

            this.DesiredSpeed -= DesiredSpeedStep;
        }

        public void SetToNextDesiredDistance()
        {
            this.DesiredDistanceInSeconds = this._accDesiredDistanceOptions.GetNext();
        }

        public void SetToPreviousDesiredDistance()
        {
            this.DesiredDistanceInSeconds = this._accDesiredDistanceOptions.GetPrevious();
        }
    }
}
