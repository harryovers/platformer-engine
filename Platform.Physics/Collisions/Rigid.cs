using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics.Collisions
{
    public class Rigid : Collision
    {
        public override bool Detect(Body selfBody, Body otherBody)
        {
            throw new NotImplementedException();
        }

        public override bool React(Body selfBody, Body otherBody)
        {
            throw new NotImplementedException();
        }
    }
}
