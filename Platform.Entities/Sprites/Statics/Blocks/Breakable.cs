using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Statics.Blocks
{
    public class Breakable : Block
    {
        public Breakable(int x, int y)
            : base(x, y)
        {
            this.ID = string.Format("breakable - x:{0}, y:{1}", x, y);
            this.Color = Color4.DarkCyan;
            this.Health = 5;
        }
    }
}
