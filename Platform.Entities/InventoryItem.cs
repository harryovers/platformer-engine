using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities
{
    [Flags]
    public enum InventoryItem
    {
        Empty = 0,
        Key1 = 1,
        Key2 = 2,
        Key3 = 4,
        Key4 = 8,

        DoubleJump = 64,
        Run = 128,

        Weapon = 512,
        WeaponLength = 1024,
        WeaponSpeed = 2048,
        WeaponPower = 4096
    }
}
