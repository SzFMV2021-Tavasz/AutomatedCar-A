using AutomatedCar.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface ITransmission
    {
        Gear Gear { get; set; }
        int InsideGear { get; set; }
        void SetInsideGear(int rpm);
    
    }
}
