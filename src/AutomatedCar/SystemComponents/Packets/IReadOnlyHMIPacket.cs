using AutomatedCar.Models.Enums;

namespace AutomatedCar.SystemComponents.Packets
{
    public interface IReadOnlyHMIPacket
    {
        int GasPedal { get; }
        int BrakePedal { get; }
        double SteeringWheelAngle { get; }
        Gear Gear { get; }
    }
}
