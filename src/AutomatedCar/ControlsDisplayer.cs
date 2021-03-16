using AutomatedCar.KeyboardHandling;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AutomatedCar
{
    public class ControlsDisplayer
    {
        private List<PressableKey> pressableKeys;
        private List<HoldableKey> holdableKeys;

        public ControlsDisplayer(List<PressableKey> pressableKeys, List<HoldableKey> holdableKeys)
        {
            this.pressableKeys = pressableKeys;
            this.holdableKeys = holdableKeys;
        }

        public void DisplayControls()
        {
            StringBuilder stringBuilder = new StringBuilder();
            holdableKeys.ForEach(key => stringBuilder.Append($"{key.Control}: {key.Key}\n"));
            pressableKeys.ForEach(key => stringBuilder.Append($"{key.Control}: {key.Key}\n"));
            MessageBox.Show(stringBuilder.ToString());
        }
    }
}
