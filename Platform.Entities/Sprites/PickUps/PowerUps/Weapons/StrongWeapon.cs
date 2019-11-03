using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.PickUps.PowerUps.Weapons
{
    public class StrongWeapon : Weapon
    {
        public StrongWeapon(int x, int y)
            : base(x, y)
        {
            this.Texture = Platform.Core.Textures.Weapon_Strong;
        }
    }
}
