using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;

namespace Platform.Entities.Sprites.Statics
{
    public class Block : Sprite
    {
        public Color4 Color { get; set; }
        public int Health { get; set; }

        public Block(int x, int y)
            : base(SpriteType.Static, x, y)
        {
            this.IsStatic = true;
            Color = Color4.Cyan;
            this.Width = 10;
            this.Height = 10;
            this.ID = "block-" + x + ":" + y;
            this.Texture = Platform.Core.Textures.Block;
            Health = int.MaxValue;
            this.DrawOrder = Physics.DrawOrder.Scenery;
        }

        public void HandleDamage(Killbox killbox)
        {
            if (killbox.Damageful >= Health)
            {
                this.Dead = true;
            }
        }

        public void HandlePlayer(Player player)
        {
        }

        public override void Draw()
        {
            if (this.Dead || this.Texture == null)
            {
                return;
            }

            SpriteRender.Draw(this.X, this.Y, this.Z, this.Width, this.Height, this.Texture, Color, 1.0, false, false);
        }
    }
}
