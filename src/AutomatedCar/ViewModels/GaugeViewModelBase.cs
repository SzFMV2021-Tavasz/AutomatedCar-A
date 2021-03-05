using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class GaugeViewModelBase : ViewModelBase
    {
        private double _value;
        private string _caption;

        public GaugeViewModelBase()
        {
        }

        public double Value
        {
            get => this._value;
            set => this.RaiseAndSetIfChanged(ref this._value, value);
        }

        public string Caption
        {
            get => this._caption;
            set => this.RaiseAndSetIfChanged(ref this._caption, value);
        }
    }
}
