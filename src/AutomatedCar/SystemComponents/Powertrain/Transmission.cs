using AutomatedCar.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class Transmission : ITransmission
    {
        public readonly (int minSpeed, int maxSpeed, int gear)[] DriveSpeedGearMapping = new (int, int, int)[]
        {
            (0, 10, 0),
            (10, 15, 1),
            (15, 20, 2),
            (20, 30, 3),
            (30, 40, 4),
            (40, 52, 5),
            (52, 70, 6),
            (70, int.MaxValue, 7),
        };

        public Gear Gear { get; set; }
        public int InsideGear { get; set; }

        public void SetInsideGear(int speed)
        {
            if (Gear == Gear.D)
            {
                InsideGear = DriveSpeedGearMapping.First(m => m.minSpeed <= speed && speed < m.maxSpeed).gear;
            }
            if (Gear == Gear.P && Gear == Gear.N)
            {
                InsideGear = 0;
            }
            if (Gear == Gear.R)
            {
                InsideGear = 1;
            }
        }
    }
}
