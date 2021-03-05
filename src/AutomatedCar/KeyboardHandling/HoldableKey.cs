using System;
using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public class HoldableKey
    {
        public Key Key { get; }

        public Action<double> OnHold { get; }
        public Action<double> OnIdle { get; }

        public bool IsBeingHeld { get; set; }

        public HoldableKey(Key key, Action<double> onHold, Action<double> onIdle)
        {
            this.Key = key;
            this.OnHold = onHold;
            this.OnIdle = onIdle;
        }
    }
}
