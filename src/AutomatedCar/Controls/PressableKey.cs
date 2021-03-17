using System;
using System.Windows.Input;

namespace AutomatedCar.Controls
{
    public class PressableKey : InputKey
    {
        public PressableKey(Key key, string category, string control, Action onPress) : base(key, category, control)
        {
            this.OnPress = onPress;
        }

        public Action OnPress { get; }
    }
}
