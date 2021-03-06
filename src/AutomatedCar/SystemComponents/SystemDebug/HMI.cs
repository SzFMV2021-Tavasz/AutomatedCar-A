using System;

namespace AutomatedCar.SystemComponents.SystemDebug
{
    public class HMI
    {
        private DebugActionArgs args = new DebugActionArgs();

        public static event EventHandler<DebugActionArgs> DebugActionEventHandler;

        public void OnDebugAction(int debugArgId)
        {
            switch (debugArgId)
            {
                case 1:
                    args.DebugRadar = !args.DebugRadar;
                    break;
                case 2:
                    args.DebugSonic = !args.DebugSonic;
                    break;
                case 3:
                    args.DebugVideo = !args.DebugVideo;
                    break;
                case 4:
                    args.DebugPolys = !args.DebugPolys;
                    break;
                default:
                    break;
            }
            DebugActionEventHandler?.Invoke(this, args);
        }
    }
}
