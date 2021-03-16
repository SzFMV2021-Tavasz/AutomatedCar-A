using System.Windows.Input;

namespace AutomatedCar.Controls
{
    public abstract class InputKey
    {
        public InputKey(Key key, string category, string control)
        {
            this.Key = key;
            this.Category = category;
            this.Control = control;
        }

        public Key Key { get; }

        public string Category { get; }

        public string Control { get; }
    }
}
