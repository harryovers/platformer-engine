using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites
{
    public abstract class Static : Sprite
    {
        public Static(int x, int y)
            : base (SpriteType.Static, x, y)
        {
            this.IsStatic = true;
            this.ID = "block-" + x + ":" + y;
        }
    }
}
