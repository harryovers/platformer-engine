using OpenTK.Input;
using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Bots
{
    public class Charger : Bot
    {
        private bool attackMode = false;
        public bool Angry { get; set; }

        public Charger(int x, int y)
            : base (x, y)
        {
            this.CollisionGroup = 2;
            this.Width = 10;
            this.Height = 10;
            this.Mass = 5;
            this.Drag = 0.02;
            this.Restitution = 0.3;
            this.ID = "enemy-unnamed-" + x + ":" + y;
            this.Damageful = 5;
            this.Health = 10;
            this.Tokens = 5;

            SetTexture();
        }

        public override void SetTexture()
        {
            if (this.Health >= 9)
            {
                this.Texture = Textures.Charger;
            }
            else
            {
                 this.Texture = Textures.Charger_Damaged;
            }
        }

        public override void Update(TimeSpan delta, KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            if (Target != null)
            {
                if (Math.Abs(Target.Center.X - this.Center.X) < 120 && Math.Abs(Target.Center.Y - this.Center.Y) < 120)
                {
                    attackMode = true;

                    if (!SoundEffects.Enemy_1.Playing)
                    {
                        SoundEffects.Enemy_1.Play();
                    }
                }

                if (attackMode)
                {
                    var diffVector = (Target.Center - this.Center).Normalize();

                    Move(diffVector.X, diffVector.Y, 1000);
                }
            }

            base.Update(delta, keyboardState, previousKeyboardState);
        }

        private void Move(double x, double y, int force)
        {
            var velocityX = Math.Abs(this.VelX);
            var velocityY = Math.Abs(this.VelY);

            if (velocityX + velocityY < 10)
            {
                this.ApplyImpulse(x * force, y * force);
            }
        }
    }
}
