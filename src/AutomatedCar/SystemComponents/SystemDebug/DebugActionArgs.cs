using System;

namespace AutomatedCar.SystemComponents.SystemDebug
{
    class DebugActionArgs : EventArgs
    {
        public bool DebugRadar { get; set; }

        public bool DebugSonic { get; set; }

        public bool DebugVideo { get; set; }

        public bool DebugPolys { get; set; }
    }
}
