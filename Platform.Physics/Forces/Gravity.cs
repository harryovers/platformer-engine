using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics.Forces
{
    public class Gravity : Force
    {
        public double GravityX { get; set; }
        public double GravityY { get; set; }

        public Gravity(double x, double y)
        {
            GravityX = x;
            GravityY = y;
        }

        public override void ApplyForce(Body body)
        {
            var dvY = GravityY / body.Mass;
            var dvX = GravityX / body.Mass;

            body.VelY += dvY;
            body.VelX += dvX;
        }
    }
}
