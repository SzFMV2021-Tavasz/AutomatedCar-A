using System.Collections.Generic;
using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public class KeyboardHandler
    {
        private double tickInterval;

        public KeyboardHandler(double tickInterval)
        {
            this.tickInterval = tickInterval;
        }

        public List<PressableKey> PressableKeys { get; } = new List<PressableKey>();

        public List<HoldableKey> HoldableKeys { get; } = new List<HoldableKey>();

        public void OnKeyDown(Key key)
        {
            PressableKeys.Find(pressableKey => pressableKey.Key == key)?.OnPress();
            HoldableKey holdableKey = HoldableKeys.Find(holdableKey => holdableKey.Key == key);
            if (holdableKey != null)
            {
                holdableKey.IsBeingHeld = true;
            }
        }

        public void OnKeyUp(Key key)
        {
            HoldableKey holdableKey = HoldableKeys.Find(holdableKey => holdableKey.Key == key);
            if (holdableKey != null)
            {
                holdableKey.IsBeingHeld = false;
            }
        }

        public void Tick()
        {
            foreach (HoldableKey holdableKey in HoldableKeys)
            {
                if (holdableKey.IsBeingHeld && holdableKey.OnHold != null) holdableKey.OnHold(tickInterval / 1000);
                else if (holdableKey.OnIdle != null) holdableKey.OnIdle(tickInterval / 1000);
            }
        }
    }
}
