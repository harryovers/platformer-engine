using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics
{
    public abstract class Force
    {
        public abstract void ApplyForce(Body body);
    }
}
