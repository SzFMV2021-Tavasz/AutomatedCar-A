namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using ReactiveUI;

    public class DashboardViewModel : ViewModelBase
    {
        private AutomatedCar _controlledCar;
        private RpmGaugeViewModel _rpmGaugeViewModel;
        private SpeedGaugeViewModel _speedGaugeViewModel;
        private GasPedalViewModel _gasPedalViewModel;

        public DashboardViewModel(AutomatedCar controlledCar)
        {
            this.ControlledCar = controlledCar;
            this.RpmGaugeViewModel = new RpmGaugeViewModel();
            this.SpeedGaugeViewModel = new SpeedGaugeViewModel();
            this.RpmGaugeViewModel.Value = 3000;
            this.RpmGaugeViewModel.Caption = $"{this.RpmGaugeViewModel.Value} rpm";
            this.SpeedGaugeViewModel.Value = 50;
            this.SpeedGaugeViewModel.Caption = $"{this.SpeedGaugeViewModel.Value} km/h";

            this.GasPedalViewModel = new GasPedalViewModel();
            this.GasPedalViewModel.Value = 50;
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

        public GasPedalViewModel GasPedalViewModel
        {
            get => this._gasPedalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._gasPedalViewModel, value);
        }
    }
}