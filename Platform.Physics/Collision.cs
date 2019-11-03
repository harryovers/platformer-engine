using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics
{
    public abstract class Collision
    {
        public abstract bool Detect(Body selfBody, Body otherBody);
        public abstract bool React(Body selfBody, Body otherBody);
    }
}
