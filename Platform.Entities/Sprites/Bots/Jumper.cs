using OpenTK.Input;
using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Bots
{
    public class Jumper : Bot
    {
        public bool Angry { get; set; }

        public Jumper(int x, int y)
            : base(x, y)
        {
            this.CollisionGroup = 2;
            this.Width = 10;
            this.Height = 10;
            this.Mass = 10;
            this.Drag = 0.0125;
            this.ID = "enemy-jumper-" + x + ":" + y;
            this.Damageful = 3;
            this.Health = 5;
            this.Angry = false;
            this.Tokens = 3;

            SetTexture();
        }

        public override void SetTexture()
        {
            if (this.Health >= 4)
            {
                this.Texture = Textures.Jumper;
            }
            else
            {
                this.Texture = Textures.Jumper_Damaged;
            }
        }

        public override void Update(TimeSpan delta, KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            if (this.Health <= 4)
            {
                this.Angry = true;
            }
            if (Target != null)
            {
                if (this.Angry || (Math.Abs(Target.Center.X - this.Center.X) < 70 && Math.Abs(Target.Center.Y - this.Center.Y) < 50))
                {
                    if (Math.Abs(this.VelX) < 2)
                    {
                        if (Target.Center.X > this.Center.X)
                        {
                            Move(this.Angry ? 125 : 100);
                        }
                        else
                        {
                            Move(this.Angry ? -125 : -100);
                        }
                    }

                    Jump(this.Angry ? 400 : 500);
                }
            }

            base.Update(delta, keyboardState, previousKeyboardState);
        }

        private void Move(int force)
        {
            var velocity = Math.Abs(this.VelX);

            if (velocity < 20)
            {
                this.ApplyImpulse(force, 0);
            }
        }

        private void Jump(int force)
        {
            var jumpAllowed = false;

            if (Math.Abs(this.VelY) < 0.1)
            {
                jumpAllowed = true;
            }

            if (jumpAllowed)
            {
                this.ApplyImpulse(0, force * -1);

                if (!SoundEffects.Jump_Second.Playing)
                {
                    SoundEffects.Jump_Second.Play();
                }
            }
        }
    }
}
