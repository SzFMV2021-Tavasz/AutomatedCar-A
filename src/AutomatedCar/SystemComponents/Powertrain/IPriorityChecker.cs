namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IPriorityChecker
    {
        IVirtualFunctionBus virtualFunctionBus { get; set; }
        PacketEnum AccelerationPriorityCheck();
        PacketEnum SteeringPriorityCheck();
    }
}
