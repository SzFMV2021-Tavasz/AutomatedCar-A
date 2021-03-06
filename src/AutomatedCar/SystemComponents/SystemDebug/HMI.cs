using System;

namespace AutomatedCar.SystemComponents.SystemDebug
{
    public class HMI
    {
        private DebugActionArgs args = new DebugActionArgs();

        public static event EventHandler<DebugActionArgs> DebugActionEventHandler;

        public void OnDebugAction(int debugArgId)
        {

        }
    }
}
