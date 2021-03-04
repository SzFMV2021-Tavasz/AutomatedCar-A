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

        }

        public void OnKeyUp(Key key)
        {

        }

        public void Tick()
        {

        }
    }
}
