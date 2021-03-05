using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class TurnSignalViewModelBase : ViewModelBase {

        private bool _isEnabled;
        public TurnSignalViewModelBase()
        {
        }

        public bool IsEnabled
        {
            get => this._isEnabled;
            set => this.RaiseAndSetIfChanged(ref this._isEnabled, value);
        }
    }
}
