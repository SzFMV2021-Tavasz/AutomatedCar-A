using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AutomatedCar.Controls
{
    public class ControlsDisplayer
    {
        private List<InputKey> inputKeys;

        public ControlsDisplayer(List<InputKey> inputKeys)
        {
            this.inputKeys = inputKeys;
        }

        public void DisplayControls()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string prevCategory = "";
            foreach (InputKey key in inputKeys)
            {
                if (prevCategory != key.Category)
                {
                    stringBuilder.Append($"\n{key.Category.ToUpper()}\n");
                    prevCategory = key.Category;
                }

                stringBuilder.Append($"{key.Control}: {key.Key}\n");
            }

            MessageBox.Show(stringBuilder.ToString());
        }
    }
}
