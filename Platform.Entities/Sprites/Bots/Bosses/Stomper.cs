using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Bots.Bosses
{
    public class Stomper : Boss
    {
        private bool floating = false;
        private bool stomping = false;
        private bool retreating = true;
        private bool angry = false;

        public Stomper(int x, int y)
            : base(x, y)
        {
            this.Width = 40;
            this.Height = 17;
            this.Damageful = 3;
            this.PositiveTextureOffset = new OpenTK.Vector2(0, 5);
            this.Mass = 30;
            this.Drag = 0;
            this.ID = "boss-stomper";
            this.Restitution = 0;
            this.Drag = 0.025;
            this.Health = 55;

            SetTexture();
        }

        private void SetTexture()
        {
            if (angry)
            {
                this.Texture = Textures.Stomper_Angry;
            }
            else if (retreating)
            {
                this.Texture = Textures.Stomper_Retreating;
            }
            else if (floating)
            {
                this.Texture = Textures.Stomper_Floating;
            }
            else if (stomping)
            {
                this.Texture = Textures.Stomper_Stomping;
            }
            else
            {
                this.Texture = Textures.Stomper;
            }
        }

        public override void Update(TimeSpan delta, OpenTK.Input.KeyboardState keyboardState, OpenTK.Input.KeyboardState previousKeyboardState)
        {
            var xSpeed = 700;
            var ySpeed = 200;
            var dropSpeed = 150;
            var leftLimit = 25;
            var rightLimit = 285;
            var bottomLimit = 175; // 135
            var topLimit = 60;

            if (this.Y < topLimit)
            {
                this.Y = topLimit;
            }

            if (leftLimit > this.Left)
            {
                this.X = leftLimit;
            }
            if (rightLimit < this.Right)
            {
                this.X = rightLimit - this.Width;
            }
            if (bottomLimit < this.Bottom)
            {
                this.Y = bottomLimit - this.Height;
            }

            if (retreating)
            {
                if (this.Y > 120) // 80
                {
                    ApplyImpulse(this.VelY < 0 ? 0 : ((Target.Center.X - this.Center.X) > 0 ? xSpeed : xSpeed * -1), ySpeed * -1);
                }
                else
                {
                    floating = true;
                    retreating = false;
                    stomping = false;
                    angry = false;
                }
            }
            else if (floating)
            {
                if (Math.Abs((Target.Center - this.Center).X) < 20)
                {
                    stomping = true;
                    floating = false;
                    retreating = false;
                }
                else
                {
                    if (this.Y > 70)
                    {
                        ApplyImpulse((Target.Center.X - this.Center.X) > 0 ? xSpeed : xSpeed * -1, ySpeed * -1);
                    }
                }
            }
            else if (stomping)
            {
                if (this.Y > 157) // 117
                {
                    this.VelY = 0;
                    retreating = true;
                    floating = false;
                    stomping = false;
                }
                else
                {
                    this.VelX = 0;
                    ApplyImpulse(0, dropSpeed);
                }
            }

            if (this.VelY > dropSpeed)
            {
                this.VelY = dropSpeed;
            }

            foreach (var body in this.CollisionList)
            {
                if (body is Killbox)
                {
                    retreating = true;
                    floating = false;
                    stomping = false;
                    angry = true;
                }
            }

            SetTexture();

            base.Update(delta, keyboardState, previousKeyboardState);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
