using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Packets
{
    class PowertrainComponentPacket : ReactiveObject, IReadOnlyPowertrainComponentPacket
    {
        private int x;
        private int y;
        private int speed;
        private int rpm;
        private double carHeadingAngle;

        public int X
        {
            get => this.x;
            set => this.RaiseAndSetIfChanged(ref this.x, value);
        }

        public int Y
        {
            get => this.y;
            set => this.RaiseAndSetIfChanged(ref this.y, value);
        }

        public int Speed
        {
            get => this.speed;
            set => this.RaiseAndSetIfChanged(ref this.speed, value);
        }

        public int Rpm
        {
            get => this.rpm;
            set => this.RaiseAndSetIfChanged(ref this.rpm, value);
        }

        public double CarHeadingAngle
        {
            get => this.carHeadingAngle;
            set => this.RaiseAndSetIfChanged(ref this.carHeadingAngle, value);
        }
    }
}
