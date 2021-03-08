namespace AutomatedCar.SystemComponents.Powertrain
{
    class PriorityChecker : IPriorityChecker
    {
        public IVirtualFunctionBus virtualFunctionBus { get; set; }

        public PacketEnum AccelerationPriorityCheck()
        {
            return PacketEnum.AEB;
        }

        public PacketEnum SteeringPriorityCheck()
        {
            return PacketEnum.ACC;
        }
    }
}
