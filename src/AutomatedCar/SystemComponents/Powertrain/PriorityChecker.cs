namespace AutomatedCar.SystemComponents.Powertrain
{
    public class PriorityChecker : IPriorityChecker
    {
        public IVirtualFunctionBus virtualFunctionBus { get; set; }

        public PacketEnum AccelerationPriorityCheck()
        {
            return PacketEnum.AEB;
        }

        public PacketEnum SteeringPriorityCheck()
        {
            if (virtualFunctionBus.HMIPacket != null)
            {
                return PacketEnum.HMI;
            }
            else
            {
                if (virtualFunctionBus.LKAPacket != null)
                {
                    return PacketEnum.LKA;
                }
                else
                {
                    return PacketEnum.PP;
                }
            }
        }
    }
}
