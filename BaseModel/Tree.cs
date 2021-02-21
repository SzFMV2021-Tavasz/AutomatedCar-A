using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel
{
    public class Tree : WorldObject
    {
        public override Type ObjectType => Type.TREE;

        public override bool IsDynamic => false;
    }
}
