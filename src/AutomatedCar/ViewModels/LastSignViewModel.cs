using ReactiveUI;
using System;
namespace AutomatedCar.ViewModels
{
    public class LastSignViewModel : ViewModelBase
    {
        private string _resourcePath;

        public LastSignViewModel()
        {
        }

        public string ResourcePath
        {
            get => this._resourcePath;
            set => this.RaiseAndSetIfChanged(ref this._resourcePath, value);
        }
    }
}
