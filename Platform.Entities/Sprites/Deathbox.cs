using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites
{
    public abstract class Deathbox : Sprite
    {
        public Deathbox(int x, int y)
            : base(SpriteType.Deathbox, x, y)
        {
            this.CollisionGroup = -2;
            this.DrawOrder = Physics.DrawOrder.Scenery;
        }
    }
}
