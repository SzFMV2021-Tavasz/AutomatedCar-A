using System.Collections.Generic;
using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public class KeyboardHandler
    {
        public List<PressableKey> PressableKeys { get; } = new List<PressableKey>();
        public List<HoldableKey> HoldableKeys { get; } = new List<HoldableKey>();

        private double tickInterval;

        public KeyboardHandler(double tickInterval)
        {
            this.tickInterval = tickInterval;
        }

        public void OnKeyDown(Key key)
        {
            PressableKeys.Find(pressableKey => pressableKey.Key == key)?.OnPress();
            HoldableKey holdableKey = HoldableKeys.Find(holdableKey => holdableKey.Key == key);
            if (holdableKey != null)
            {
                holdableKey.CurrentStateDuration = 0;
                holdableKey.IsBeingHeld = true;
            }
        }

        public void OnKeyUp(Key key)
        {
            HoldableKey holdableKey = HoldableKeys.Find(holdableKey => holdableKey.Key == key);
            if (holdableKey != null)
            {
                holdableKey.IsBeingHeld = false;
                holdableKey.CurrentStateDuration = 0;
            }
        }

        public void Tick()
        {
            foreach (HoldableKey holdableKey in HoldableKeys)
            {
                holdableKey.CurrentStateDuration += tickInterval / 1000;
                if (holdableKey.IsBeingHeld && holdableKey.OnHold != null) holdableKey.OnHold(holdableKey.CurrentStateDuration);
                else if (holdableKey.OnIdle != null) holdableKey.OnIdle(holdableKey.CurrentStateDuration);
            }
        }
    }
}
