using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Statics.Blocks
{
    public class BotBoundry : Block
    {
        public BotBoundry(int x, int y)
            : base(x, y)
        {
            this.CollisionGroup = 2;
            this.DontList = true;
            this.Color = Color4.Aquamarine;
        }

        public override void Draw()
        {
            // don't draw!
            //base.Draw();
        }
    }
}
