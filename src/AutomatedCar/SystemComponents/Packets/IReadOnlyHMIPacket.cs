using AutomatedCar.Models.Enums;

namespace AutomatedCar.SystemComponents.Packets
{
    public interface IReadOnlyHMIPacket
    {
        int GasPedal { get; }
        int BreakPedal { get; }
        double SteeringWheelAngle { get; }
        Gear Gear { get; }
    }
}
