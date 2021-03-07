using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public abstract class TurnSignalViewModelBase : ViewModelBase
    {
        private bool _isEnabled;

        public TurnSignalViewModelBase()
        {
        }

        public bool IsEnabled
        {
            get => this._isEnabled;
            protected set => this.RaiseAndSetIfChanged(ref this._isEnabled, value);
        }

        public virtual void Toggle() => this.IsEnabled = !this.IsEnabled;
    }
}
