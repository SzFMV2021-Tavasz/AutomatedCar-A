using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class Vehicle : WorldObject
    {
        public Vehicle(Type type) : base(type) {}

        public override Type[] AllowedTypes => new Type[] {
            Type.CAR_1_BLUE,
            Type.CAR_1_RED,
            Type.CAR_1_WHITE,
            Type.CAR_2_BLUE,
            Type.CAR_2_RED,
            Type.CAR_2_WHITE,
            Type.CAR_3_BLACK,
            Type.BICYCLE
        };

        public override bool IsDynamic => true;
    }
}
