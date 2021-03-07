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
        }

        public double DesiredDistanceInSeconds
        {
            get => this._desiredDistanceInSeconds;
            set => this.RaiseAndSetIfChanged(ref this._desiredDistanceInSeconds, value);
        }

        public int DesiredSpeed
        {
            get => this._desiredSpeed;
            set => this.RaiseAndSetIfChanged(ref this._desiredSpeed, value);
        }

        public bool IsTurnedOn
        {
            get => this._isTurnedOn;
            set => this.RaiseAndSetIfChanged(ref this._isTurnedOn, value);
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

        }

        public void SetToPreviousDesiredDistance()
        {

        }
    }
}
