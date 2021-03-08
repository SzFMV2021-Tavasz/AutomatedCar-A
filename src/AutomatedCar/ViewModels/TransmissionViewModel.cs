using AutomatedCar.Models.Enums;
using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class TransmissionViewModel : ViewModelBase
    {
        private Gear _currentGear;

        public TransmissionViewModel()
        {
        }

        public Gear CurrentGear
        {
            get => this._currentGear;
            set => this.RaiseAndSetIfChanged(ref this._currentGear, value);
        }
    }
}
