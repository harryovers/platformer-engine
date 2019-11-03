using OpenTK.Graphics;
using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;

namespace Platform.Entities.Sprites.PickUps
{
    public class Token : PickUp
    {
        public bool IsPlayers { get; set; }

        public Token(int x, int y, bool isPlayers = false)
            : base(x, y)
        {
            IsPlayers = isPlayers;
            this.ID = "token-" + x + ":" + y;
            this.Width = 2;
            this.Height = 2;
            this.Mass = 0.2;
            this.Restitution = 1;
            this.CollisionGroup = 777;
            this.Drag = 0.3;

            this.Texture = Textures.Blank;
        }

        public override void Draw()
        {
            if (this.Dead || this.Texture == null)
            {
                return;
            }

            SpriteRender.Draw(
                this.X - this.NegativeTextureOffset.X,
                this.Y - this.NegativeTextureOffset.Y,
                this.Z,
                this.Width + this.PositiveTextureOffset.X,
                this.Height + this.PositiveTextureOffset.Y,
                this.Texture,
                this.IsPlayers ? Color4.Red : Color4.Yellow,
                1.0f,
                false);
        }
    }
}
