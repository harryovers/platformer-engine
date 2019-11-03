using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites
{
    public abstract class PickUp : Sprite
    {
        public PickUp(int x, int y)
            : base(SpriteType.PickUp, x, y)
        {
            this.CollisionGroup = -1;
            this.DrawOrder = Physics.DrawOrder.PickUp;
        }
    }
}
