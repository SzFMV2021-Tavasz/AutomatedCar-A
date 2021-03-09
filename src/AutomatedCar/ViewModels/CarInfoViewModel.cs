using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.ViewModels
{
    public class CarInfoViewModel : ViewModelBase
    {
        private int _speedLimit;
        private int _steeringWheelAngle;
        private int _x;
        private int _y;

        public CarInfoViewModel()
        {
        }

        public int SpeedLimit
        {
            get => this._speedLimit;
            set => this.RaiseAndSetIfChanged(ref this._speedLimit, value);
        }

        public int SteeringWheelAngle
        {
            get => this._steeringWheelAngle;
            set => this.RaiseAndSetIfChanged(ref this._steeringWheelAngle, value);
        }

        public int X
        {
            get => this._x;
            set => this.RaiseAndSetIfChanged(ref this._x, value);
        }

        public int Y
        {
            get => this._y;
            set => this.RaiseAndSetIfChanged(ref this._y, value);
        }
    }
}
