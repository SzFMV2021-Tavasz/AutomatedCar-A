namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IPriorityChecker
    {
        IVirtualFunctionBus virtualFunctionBus { get; set; }

        IVehicleForces vehicleForces { get; set; }

        void AccelerationPriorityCheck();

        void SteeringPriorityCheck();
    }
}
