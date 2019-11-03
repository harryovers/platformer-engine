using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.PickUps.PowerUps
{
    public class Run : PowerUp
    {
        public Run(int x, int y)
            : base(x, y)
        {
            this.Texture = Platform.Core.Textures.Run;
        }
    }
}
