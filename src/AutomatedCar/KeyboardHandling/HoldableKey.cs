﻿using System;
using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public class HoldableKey : InputKey
    {
        public HoldableKey(Key key, string control, Action<double> onHold, Action<double> onIdle) : base(key, control)
        {
            this.OnHold = onHold;
            this.OnIdle = onIdle;
        }

        public Action<double> OnHold { get; }

        public Action<double> OnIdle { get; }

        public bool IsBeingHeld { get; set; }
    }
}
