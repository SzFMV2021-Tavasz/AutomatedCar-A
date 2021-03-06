using AutomatedCar.ViewModels;
using ReactiveUI;

namespace AutomatedCar.Views
{
    public class BreakPedalViewModel : ViewModelBase
    {
        private int _value;
        private string _caption;

        public BreakPedalViewModel()
        {
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
