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

        public void SetInsideGear(int speed)
        {
            if (Gear == Gear.D)
            {
                if (speed < 10)
                {
                    InsideGear = 0;
                }
                else if (speed >= 10 && speed < 15)
                {
                    InsideGear = 1;
                }
                else if (speed >= 15 && speed < 20)
                {
                    InsideGear = 2;
                }
                else if (speed >= 20 && speed < 30)
                {
                    InsideGear = 3;
                }
                else if (speed >= 30 && speed < 40)
                {
                    InsideGear = 4;
                }
                else if (speed >= 40 && speed < 52)
                {
                    InsideGear = 5;
                }
                else if (speed >= 52 && speed < 70)
                {
                    InsideGear = 6;
                }
                else if (speed >= 70)
                {
                    InsideGear = 7;
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
