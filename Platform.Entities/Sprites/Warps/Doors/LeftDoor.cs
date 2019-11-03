using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Warps.Doors
{
    public class LeftDoor : Door
    {
        public LeftDoor(int x, int y, string level, Direction start)
            : base(x, y, level, Direction.Left, start)
        {
        }
    }
}
