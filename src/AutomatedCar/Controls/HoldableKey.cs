using System;
using System.Windows.Input;

namespace AutomatedCar.Controls
{
    public class HoldableKey : InputKey
    {
        public HoldableKey(Key key, string category, string control, Action<double> onHold, Action<double> onIdle) : base(key, category, control)
        {
            this.OnHold = onHold;
            this.OnIdle = onIdle;
        }

        public Action<double> OnHold { get; }

        public Action<double> OnIdle { get; }

        public bool IsBeingHeld { get; set; }
    }
}
