namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using ReactiveUI;

    public class DashboardViewModel : ViewModelBase
    {
        private AutomatedCar controlledCar;

        public DashboardViewModel(Models.AutomatedCar controlledCar)
        {
            this.ControlledCar = controlledCar;
        }

        public Models.AutomatedCar ControlledCar
        {
            get => this.controlledCar;
            private set => this.RaiseAndSetIfChanged(ref this.controlledCar, value);
        }
    }
}