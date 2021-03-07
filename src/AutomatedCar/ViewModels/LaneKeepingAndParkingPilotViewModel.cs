using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class LaneKeepingAndParkingPilotViewModel : ViewModelBase
    {
        private bool _isLaneKeepingTurnedOn;
        private bool _isParkingPilotTurnedOn;
        private bool _isLaneKeepingWarningTurnedOn;

        public LaneKeepingAndParkingPilotViewModel()
        {
        }

        public bool IsLaneKeepingTurnedOn
        {
            get => this._isLaneKeepingTurnedOn;
            private set => this.RaiseAndSetIfChanged(ref this._isLaneKeepingTurnedOn, value);
        }

        public bool IsParkingPilotTurnedOn
        {
            get => this._isParkingPilotTurnedOn;
            private set => this.RaiseAndSetIfChanged(ref this._isParkingPilotTurnedOn, value);
        }

        public bool IsLaneKeepingWarningTurnedOn
        {
            get => this._isLaneKeepingWarningTurnedOn;
            private set => this.RaiseAndSetIfChanged(ref this._isLaneKeepingWarningTurnedOn, value);
        }

        public void ToggleLaneKeeping() => this.IsLaneKeepingTurnedOn = !this.IsLaneKeepingTurnedOn;

        public void ToggleParkingPilot() => this.IsParkingPilotTurnedOn = !this.IsParkingPilotTurnedOn;

        public void DisplayLaneKeepingWarning() => this.IsLaneKeepingWarningTurnedOn = true;

        public void TurnOffLaneKeepingWarning() => this.IsLaneKeepingWarningTurnedOn = false;

    }
}
