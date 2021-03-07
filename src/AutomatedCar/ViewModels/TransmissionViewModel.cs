using AutomatedCar.Models.Enums;
using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class TransmissionViewModel : ViewModelBase
    {
        private Gear _currentGear;
        private string _caption;

        public TransmissionViewModel()
        {
        }

        public Gear CurrentGear
        {
            get => this._currentGear;
            set => this.RaiseAndSetIfChanged(ref this._currentGear, value);
        }

        public string Caption
        {
            get => this._caption;
            set => this.RaiseAndSetIfChanged(ref this._caption, value);
        }
    }
}
