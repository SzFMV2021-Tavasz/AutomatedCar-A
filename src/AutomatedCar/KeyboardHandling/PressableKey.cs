using System;
using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public class PressableKey : InputKey
    {
        public PressableKey(Key key, string control, Action onPress) : base(key, control)
        {
            this.OnPress = onPress;
        }

        public Action OnPress { get; }
    }
}
