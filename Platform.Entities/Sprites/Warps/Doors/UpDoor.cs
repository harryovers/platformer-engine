using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Warps.Doors
{
    public class UpDoor : Door
    {
        public UpDoor(int x, int y, string level, Direction start)
            : base(x, y, level, Direction.Up, start)
        {
        }
    }
}
