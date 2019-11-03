using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics.Forces
{
    public class Impulse : Force
    {
        public override void ApplyForce(Body body)
        {
            body.VelX += body.ImpX / body.Mass;
            body.ImpX = 0;
            body.VelY += body.ImpY / body.Mass;
            body.ImpY = 0;
        }
    }
}
