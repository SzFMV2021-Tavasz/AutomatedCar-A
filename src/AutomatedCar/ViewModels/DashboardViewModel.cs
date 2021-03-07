namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using AutomatedCar.Models.Enums;
    using ReactiveUI;

    public class DashboardViewModel : ViewModelBase
    {
        private AutomatedCar _controlledCar;
        private RpmGaugeViewModel _rpmGaugeViewModel;
        private SpeedGaugeViewModel _speedGaugeViewModel;
        private BreakPedalViewModel _breakPedalViewModel;
        private GasPedalViewModel _gasPedalViewModel;
        private TransmissionViewModel _transmissionViewModel;
        private TurnSignalViewModelBase _leftTurnSignalViewModel;
        private TurnSignalViewModelBase _rightTurnSignalViewModel;
        private ACCOptionsViewModel _accOptionsViewModel;
        private LaneKeepingAndParkingPilotViewModel _laneKeepingAndParkingPilotViewModel;

        public DashboardViewModel(AutomatedCar controlledCar)
        {
            this.ControlledCar = controlledCar;
            this.RpmGaugeViewModel = new RpmGaugeViewModel();
            this.SpeedGaugeViewModel = new SpeedGaugeViewModel();
            this.TransmissionViewModel = new TransmissionViewModel();
            this.LeftTurnSignalViewModel = new TurnSignalViewModelBase();
            this.RightTurnSignalViewModel = new TurnSignalViewModelBase();
            this.RpmGaugeViewModel.Value = 3000;
            this.RpmGaugeViewModel.Caption = $"{this.RpmGaugeViewModel.Value} rpm";
            this.SpeedGaugeViewModel.Value = 50;
            this.SpeedGaugeViewModel.Caption = $"{this.SpeedGaugeViewModel.Value} km/h";
            this.BreakPedalViewModel = new BreakPedalViewModel();
            this.BreakPedalViewModel.Value = 75;
            this.GasPedalViewModel = new GasPedalViewModel();
            this.GasPedalViewModel.Value = 50;
            this.TransmissionViewModel.CurrentGear = Gear.P;
            this.TransmissionViewModel.Caption = $"Gear: {this.TransmissionViewModel.CurrentGear}";

            this.ACCOptionsViewModel = new ACCOptionsViewModel();

            this.LaneKeepingAndParkingPilotViewModel = new LaneKeepingAndParkingPilotViewModel();
        }

        public AutomatedCar ControlledCar
        {
            get => this._controlledCar;
            private set => this.RaiseAndSetIfChanged(ref this._controlledCar, value);
        }

        public RpmGaugeViewModel RpmGaugeViewModel
        {
            get => this._rpmGaugeViewModel;
            set => this.RaiseAndSetIfChanged(ref this._rpmGaugeViewModel, value);
        }

        public SpeedGaugeViewModel SpeedGaugeViewModel
        {
            get => this._speedGaugeViewModel;
            set => this.RaiseAndSetIfChanged(ref this._speedGaugeViewModel, value);
        }

        public BreakPedalViewModel BreakPedalViewModel
        {
            get => this._breakPedalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._breakPedalViewModel, value);
        }

        public GasPedalViewModel GasPedalViewModel
        {
            get => this._gasPedalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._gasPedalViewModel, value);
        }

        public TransmissionViewModel TransmissionViewModel
        {
            get => this._transmissionViewModel;
            set => this.RaiseAndSetIfChanged(ref this._transmissionViewModel, value);
        }

        public TurnSignalViewModelBase LeftTurnSignalViewModel
        {
            get => this._leftTurnSignalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._leftTurnSignalViewModel, value);
        }

        public TurnSignalViewModelBase RightTurnSignalViewModel
        {
            get => this._rightTurnSignalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._rightTurnSignalViewModel, value);
        }

        public ACCOptionsViewModel ACCOptionsViewModel
        {
            get => this._accOptionsViewModel;
            set => this.RaiseAndSetIfChanged(ref this._accOptionsViewModel, value);
        }

        public LaneKeepingAndParkingPilotViewModel LaneKeepingAndParkingPilotViewModel
        {
            get => this._laneKeepingAndParkingPilotViewModel;
            set => this.RaiseAndSetIfChanged(ref this._laneKeepingAndParkingPilotViewModel, value);
        }

        public void ToggleACC() => this.ACCOptionsViewModel.Toggle();

        public void IncreaseACCDesiredSpeed() => this.ACCOptionsViewModel.IncreaseDesiredSpeed();

        public void DecreaseACCDesiredSpeed() => this.ACCOptionsViewModel.DecreaseDesiredSpeed();

        public void SetToNextACCDesiredDistance() => this.ACCOptionsViewModel.SetToNextDesiredDistance();

        public void SetToPreviousACCDesiredDistance() => this.ACCOptionsViewModel.SetToPreviousDesiredDistance();

        public void ToggleParkingPilot() => this.LaneKeepingAndParkingPilotViewModel.ToggleParkingPilot();

        public void ToggleLaneKeeping() => this.LaneKeepingAndParkingPilotViewModel.ToggleLaneKeeping();

        public void DisplayLaneKeepingWarning() => this.LaneKeepingAndParkingPilotViewModel.DisplayLaneKeepingWarning();
    }
}