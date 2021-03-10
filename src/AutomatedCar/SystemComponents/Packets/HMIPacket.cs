using AutomatedCar.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Packets
{
    public class HMIPacket : IReadOnlyHMIPacket
    {
        public HMIPacket(int gasPedal, int brakePedal, int steeringWheelAngle, Gear gear)
        {
            this.GasPedal = gasPedal;
            this.BrakePedal = brakePedal;
            this.SteeringWheelAngle = steeringWheelAngle;
            this.Gear = gear;
        }

        public int GasPedal { get; }

        public int BrakePedal { get; }

        public double SteeringWheelAngle { get; }

        public Gear Gear { get; }
    }
}
