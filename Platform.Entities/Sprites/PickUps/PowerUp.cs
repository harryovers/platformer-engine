using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.PickUps
{
    public abstract class PowerUp : PickUp
    {
        public PowerUp(int x, int y)
            : base(x, y)
        {
            this.Width = 10;
            this.Height = 10;
            this.IsStatic = true;
            this.ID = "powerup-" + x + ":" + y;

            this.Texture = Textures.Star;
        }
    }
}
