using AutomatedCar.SystemComponents.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents
{
    public interface IVirtualFunctionBus
    {
        void RegisterComponent(SystemComponent component);

        IReadOnlyDummyPacket DummyPacket { get; set; }
        IReadOnlyPowertrainComponentPacket PowertrainPacket { get; set; }
        IReadOnlyACCPacket ACCPacket { get; set; }
        IReadOnlyAEBPacket AEBPacket { get; set; }
        IReadOnlyHMIPacket HMIPacket { get; set; }
        IReadOnlyLKAPacket LKAPacket { get; set; }
        IReadOnlyPPPacket PPPacket { get; set; }
    }
}
