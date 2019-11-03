using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics.Forces
{
    public class Drag : Force
    {
        public override void ApplyForce(Body body)
        {
            body.VelX -= CalcDragCoef(body.VelX, body.Drag);
            body.VelY -= CalcDragCoef(body.VelY, body.Drag);
        }

        private double CalcDragCoef(double speed, double drag)
        {
            if (drag > 1.0)
            {
                drag = 1.0;
            }

            var multip = speed * drag;

            return multip;
        }
    }
}
