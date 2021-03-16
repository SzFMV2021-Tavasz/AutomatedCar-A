using System.Windows.Input;

namespace AutomatedCar.KeyboardHandling
{
    public abstract class InputKey
    {
        public InputKey(Key key, string control)
        {
            this.Key = key;
            this.Control = control;
        }

        public Key Key { get; }

        public string Control { get; }
    }
}
