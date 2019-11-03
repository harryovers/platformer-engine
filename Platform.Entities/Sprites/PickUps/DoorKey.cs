using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.PickUps
{
    public class DoorKey : PickUp
    {
        public int LockID { get; set; }

        public DoorKey(int x, int y)
            : base(x, y)
        {
            this.Width = 10;
            this.Height = 10;
            this.IsStatic = true;
            this.ID = "key-" + x + ":" + y;
            this.LockID = 1;

            this.Texture = Textures.Key;
        }
    }
}
