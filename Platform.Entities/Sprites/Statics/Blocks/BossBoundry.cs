using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Statics.Blocks
{
    public class BossBoundry : Block
    {
        public BossBoundry(int x, int y)
            : base(x, y)
        {
            this.CollisionGroup = 8055;
            this.DontList = true;
        }

        public override void Draw()
        {
            // don't draw
            // base.Draw();
        }
    }
}
