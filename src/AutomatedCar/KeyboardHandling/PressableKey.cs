using System;
using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public class PressableKey
    {
        public Key Key { get; }

        public Action OnPress { get; }

        public PressableKey(Key key, Action onPress)
        {
            this.Key = key;
            this.OnPress = onPress;
        }
    }
}
