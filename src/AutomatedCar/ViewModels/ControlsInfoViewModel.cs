using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    public class ControlsInfoViewModel : ViewModelBase
    {
        private string _controlsInfoKey;

        public ControlsInfoViewModel()
        {
        }

        public string ControlsInfoKey
        {
            get => this._controlsInfoKey;
            set => this.RaiseAndSetIfChanged(ref this._controlsInfoKey, value);
        }

        public string Caption => $"Press {_controlsInfoKey} to display controls";

        public void SetControlsInfoKey(string key) => this.ControlsInfoKey = key;
    }
}
