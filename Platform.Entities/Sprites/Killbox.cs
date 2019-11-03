using Platform.Core;
using Platform.Entities.Sprites.Statics;
using Platform.Entities.Sprites.Statics.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites
{
    public class Killbox : Sprite
    {
        public bool Used { get; set; }

        public Killbox(int x, int y)
            : base (SpriteType.Killbox, x, y)
        {
            this.ID = "killbox";
            this.CollisionGroup = -3;
            this.IsStatic = true;
            this.Damageful = 3;
            this.Used = false;
            this.CollisionOnlyUpdate = true;
            this.DrawOrder = Physics.DrawOrder.Player;

            this.Texture = Textures.Killbox;
        }

        public void HandleStaticCollisions()
        {
            foreach (var body in this.CollisionList.Where(c => c is Block))
            {
                var sprite = body as Sprite;

                if (sprite is Block)
                {
                    (sprite as Block).HandleDamage(this);
                }
            }
        }
    }
}
