using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Packets
{
    public interface IReadOnlyPowertrainComponentPacket
    {
        int X { get; }
        int Y { get; }
        int Speed { get; }
        int Rpm { get; }
        double SteeringWheelAngleDegrees { get; }
    }
}
