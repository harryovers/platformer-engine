using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites
{
    public abstract class Warp : Sprite
    {
        public Warp(int x, int y)
            : base (SpriteType.Warp, x, y)
        {
            this.CollisionGroup = -1;
            this.IsStatic = true;
            this.DrawOrder = Physics.DrawOrder.Door;
        }
    }
}
