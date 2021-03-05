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
        private TransmissionViewModel _transmissionViewModel;
        private TurnSignalViewModelBase _leftTurnSignalViewModel;
        private TurnSignalViewModelBase _rightTurnSignalViewModel;

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
            this.TransmissionViewModel.CurrentGear = Gear.P;
            this.TransmissionViewModel.Caption = $"Gear: {this.TransmissionViewModel.CurrentGear}";
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
    }
}