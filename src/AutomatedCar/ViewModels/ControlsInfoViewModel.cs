using System.Windows.Input;

namespace AutomatedCar.ViewModels
{
    public class ControlsInfoViewModel : ViewModelBase
    {
        private string controlsInfoKey;

        public ControlsInfoViewModel(string controlsInfoKey)
        {
            this.controlsInfoKey = controlsInfoKey;
        }

        public string Caption => $"Press {controlsInfoKey} to display controls";
    }
}
