using OpenTK.Input;
using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Bots
{
    public class Spider : Bot
    {
        public Spider(int x, int y)
            : base(x, y)
        {
            this.CollisionGroup = 2;
            this.Width = 10;
            this.Height = 10;
            this.Mass = 10;
            this.Drag = 0.0125;
            this.ID = "enemy-spider-" + x + ":" + y;
            this.Damageful = 2;
            this.Texture = Platform.Core.Textures.Spider;
            this.Tokens = 2;
        }

        public override void Update(TimeSpan delta, KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            if (Target != null)
            {
                if (Math.Abs(Target.Center.X - this.Center.X) < 100 && Math.Abs(Target.Center.Y - this.Center.Y) < 50)
                {
                    if (Math.Abs(this.VelX) < 0.1)
                    {
                        if (Target.Center.X > this.Center.X)
                        {
                            Move(100);
                        }
                        else
                        {
                            Move(-100);
                        }
                    }
                }
            }

            base.Update(delta, keyboardState, previousKeyboardState);
        }

        private void Move(int force)
        {
            var velocity = Math.Abs(this.VelX);

            if (velocity < 16)
            {
                this.ApplyImpulse(force, 0);

                if (!SoundEffects.Enemy_2.Playing)
                {
                    SoundEffects.Enemy_2.Play();
                }
            }
        }
    }
}
