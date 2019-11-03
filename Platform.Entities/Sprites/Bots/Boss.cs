using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.Bots
{
    public abstract class Boss : Bot
    {
        public PickUp Gift { get; set; }
        public bool Gifted { get; set; }

        public Boss(int x, int y)
            : base(x, y)
        {
            this.CollisionGroup = 8055;
            this.Tokens = 13;
            this.DrawOrder = Physics.DrawOrder.Boss;
        }

        public override void Update(TimeSpan delta, OpenTK.Input.KeyboardState keyboardState, OpenTK.Input.KeyboardState previousKeyboardState)
        {
            base.Update(delta, keyboardState, previousKeyboardState);
        }
    }
}
