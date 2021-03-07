using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class GaugeViewModelBase : ViewModelBase
    {
        private double _value;       

        public GaugeViewModelBase()
        {
        }

        public double Value
        {
            get => this._value;
            set => this.RaiseAndSetIfChanged(ref this._value, value);
        }
    }
}
