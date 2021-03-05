namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using ReactiveUI;

    public class DashboardViewModel : ViewModelBase
    {
        private AutomatedCar controlledCar;
        private RpmGaugeViewModel _rpmGaugeViewModel;
        private SpeedGaugeViewModel _speedGaugeViewModel;

        public DashboardViewModel(Models.AutomatedCar controlledCar)
        {
            this.ControlledCar = controlledCar;
        }

        public Models.AutomatedCar ControlledCar
        {
            get => this.controlledCar;
            private set => this.RaiseAndSetIfChanged(ref this.controlledCar, value);
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
    }
}