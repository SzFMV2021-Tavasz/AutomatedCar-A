using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public abstract class GaugeViewModelBase : ViewModelBase
    {
        private double _value;       

        public GaugeViewModelBase()
        {
        }

        public double Value
        {
            get => this._value;
            protected set => this.RaiseAndSetIfChanged(ref this._value, value);
        }

        public abstract void SetValue(int value);
    }
}
