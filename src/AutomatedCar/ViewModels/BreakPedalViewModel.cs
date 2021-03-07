using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class BreakPedalViewModel : ViewModelBase
    {
        private int _value;
        private string _caption;

        public BreakPedalViewModel()
        {
            this.Caption = "break pedal";
        }

        public int Value
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
