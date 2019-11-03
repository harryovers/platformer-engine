using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.PickUps.PowerUps
{
    public class DoubleJump : PowerUp
    {
        public DoubleJump(int x, int y)
            : base(x, y)
        {
            this.Texture = Textures.DoubleJump;
        }
    }
}
