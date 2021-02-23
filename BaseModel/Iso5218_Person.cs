using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class Iso5218_Person : WorldObject
    {
        public Iso5218_Person(Type type) : base(type) { }

        public override Type[] AllowedTypes => new Type[] {
            Type.MAN,
            Type.WOMAN
        };

        public override Tag Tags => Tag.DYNAMIC | Tag.PEDESTRIAN;
    }
}
