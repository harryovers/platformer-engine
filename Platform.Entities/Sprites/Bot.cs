using OpenTK.Input;
using Platform.Entities.Sprites.PickUps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites
{
    public abstract class Bot : Sprite
    {
        protected int Health = 1;
        public Sprite Target { get; set; }
        protected int Tokens { get; set; }

        public Bot(int x, int y)
            : base(SpriteType.Character, x, y)
        {
            Tokens = 5;
            this.DrawOrder = Physics.DrawOrder.Enemy;
        }

        public virtual void SetTexture()
        {
            // do nothing
        }

        public override void Update(TimeSpan delta, KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            CheckKillboxCollision();

            base.Update(delta, keyboardState, previousKeyboardState);
        }

        protected void CheckKillboxCollision()
        {
            if (CollisionList.Any(b => b.ID.StartsWith("k")))
            {
                Debug.WriteLine("dies");
            }

            foreach (var body in CollisionList)
            {
                if (body != null)
                {
                    if (body is Sprite)
                    {
                        var sprite = body as Sprite;

                        switch (sprite.SpriteType)
                        {
                            case SpriteType.Killbox:
                                var killbox = sprite as Killbox;

                                if (!killbox.Used)
                                {
                                    Health -= sprite.Damageful;
                                    SetTexture();
                                    killbox.Used = true;

                                    if (Health <= 0)
                                    {
                                        if (!this.Dead)
                                        {
                                            var status = new PlayerStatus()
                                            {
                                                Data = this.Tokens + ":" + (int)this.Center.X + ":" + (int)this.Center.Y,
                                                State = PlayerState.Kill
                                            };

                                            (Target as Player).Status.Enqueue(status);
                                        }

                                        this.Dead = true;
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }
    }
}
