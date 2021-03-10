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
        public Gear Gear { get; set; }
        public int InsideGear { get; set; }

        public void SetInsideGear(int rpm)
        {
            if (Gear == Gear.D)
            {
                if(rpm > 2500 && InsideGear < 5)
                {
                    InsideGear++;
                }
                else if (rpm < 1600 && InsideGear > 1)
                {
                    InsideGear--;
                }
                else if (InsideGear == 0)
                {
                    InsideGear++;
                }
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
