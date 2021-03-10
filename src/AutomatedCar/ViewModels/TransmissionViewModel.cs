using AutomatedCar.Models.Enums;
using ReactiveUI;
using System.Collections.Generic;

namespace AutomatedCar.ViewModels
{
    public class TransmissionViewModel : ViewModelBase
    {
        private readonly IReadOnlyList<Gear> Gears = new[] { Gear.P, Gear.N, Gear.D, Gear.R };
        private int _currentGearIndex;
        private Gear _currentGear;

        public TransmissionViewModel()
        {
            this._currentGearIndex = 0;
            this.CurrentGear = Gear.P;
        }

        public Gear CurrentGear
        {
            get => this._currentGear;
            private set => this.RaiseAndSetIfChanged(ref this._currentGear, value);
        }

        public void ShiftUp()
        {
            if (this._currentGearIndex == this.Gears.Count - 1)
            {
                return;
            }

            this.CurrentGear = this.Gears[++this._currentGearIndex];
        }

        public void ShiftDown()
        {
            if (this._currentGearIndex == 0)
            {
                return;
            }

            this.CurrentGear = this.Gears[--this._currentGearIndex];
        }
    }
}
