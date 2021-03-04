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
            foreach (PressableKey pressableKey in PressableKeys)
            {
                if (key == pressableKey.Key)
                {
                    pressableKey.OnPress();
                }
            }

            foreach (HoldableKey holdableKey in HoldableKeys)
            {
                if (key == holdableKey.Key)
                {
                    holdableKey.CurrentStateDuration = 0;
                    holdableKey.IsBeingHeld = true;
                }
            }
        }

        public void OnKeyUp(Key key)
        {
            foreach (HoldableKey holdableKey in HoldableKeys)
            {
                if (key == holdableKey.Key)
                {
                    holdableKey.IsBeingHeld = false;
                    holdableKey.CurrentStateDuration = 0;
                }
            }
        }

        public void Tick()
        {
            foreach (HoldableKey holdableKey in HoldableKeys)
            {
                holdableKey.CurrentStateDuration += tickInterval / 1000;
                if (holdableKey.IsBeingHeld) holdableKey.OnHold(holdableKey.CurrentStateDuration);
                else holdableKey.OnIdle(holdableKey.CurrentStateDuration);
            }
        }
    }
}
