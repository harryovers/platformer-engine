using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Deathboxes
{
    public class Spikes : Deathbox
    {
        public Spikes(int x, int y)
            : base(x, y)
        {
            this.Width = 10;
            this.Height = 9;
            this.IsStatic = true;
            this.ID = "spikes-" + x + ":" + y;

            this.Texture = Textures.Spikes;
        }
    }
}
