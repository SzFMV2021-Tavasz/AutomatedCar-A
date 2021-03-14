namespace AutomatedCar.SystemComponents.Powertrain
{
    public class PriorityChecker : IPriorityChecker
    {
        public IVirtualFunctionBus virtualFunctionBus { get; set; }

        public PacketEnum AccelerationPriorityCheck()
        {
            if (virtualFunctionBus.AEBPacket != null)
            {
                return PacketEnum.AEB;
            }
            else if (virtualFunctionBus.HMIPacket != null)
            {
                return PacketEnum.HMI;
            }
            else
            {
                if (virtualFunctionBus.ACCPacket != null)
                {
                    return PacketEnum.ACC;
                }
                else
                {
                    return PacketEnum.PP;
                }
            }
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
