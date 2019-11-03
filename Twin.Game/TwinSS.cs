using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK;

namespace Twin.Game
{
    public class TwinSS : TK.Game
    {
        public static List<Sprite> _sprites = new List<Sprite>();

        //Texture2D texture;

        //float x, y, vX, vY, dX, dY, aX, aY, r, pX, pY, pvX, pvY;

        public TwinSS()
            : base(800, 600)
        {
            this.Title = "Twin";
            //gameplay = new Gameplay.Gameplay();
            this.WindowBorder = OpenTK.WindowBorder.Fixed;
        }

        public override void LoadGame()
        {
            //texture = TK.ContentPipeline.LoadTexture("Textures/char.png");
            //x = 0;
            //y = 0;
            //vX = 0;
            //vY = 0;
            //dX = 0.3f;
            //dY = 0.3f;
            //aX = 0.1f;
            //aY = 0.1f;
            //r = 0;
            //pX = 0;
            //pY = 0;
            //pvX = 0;
            //pvY = 0;
            //Loader.LoadTextures();
            //Loader.LoadSoundEffects();
            //Loader.LoadFonts();
            //gameplay.Load();

            _sprites.Add(new Player("char.png", 1));

            SpriteRender.SetView(0, 0, 800, 600, 1);
        }

        public override void Update(TimeSpan delta)
        {
            var kbState = OpenTK.Input.Keyboard.GetState();

            if (kbState.IsKeyDown(Key.Escape))
            {
                this.Close();
            }

            for (int i = 0, n = _sprites.Count(); i != n; ++i)
            {
                _sprites[i].Update();
            }

            //if (kbState.IsKeyDown(Key.Up))
            //{
            //    vY -= aY;
            //}
            //else if (kbState.IsKeyDown(Key.Down))
            //{
            //    vY += aY;
            //}
            //else
            //{
            //    if (vY < (dY * -1))
            //    {
            //        vY += dY;
            //    }
            //    else if (vY > dY)
            //    {
            //        vY -= dY;
            //    }
            //    else
            //    {
            //        vY = 0;
            //    }
            //}
            //if (kbState.IsKeyDown(Key.Left))
            //{
            //    vX -= aX;
            //}
            //else if (kbState.IsKeyDown(Key.Right))
            //{
            //    vX += aX;
            //}
            //else
            //{
            //    if (vX < (dX * -1))
            //    {
            //        vX += dX;
            //    }
            //    else if (vX > dX)
            //    {
            //        vX -= dX;
            //    }
            //    else
            //    {
            //        vX = 0;
            //    }
            //}

            //if (kbState.IsKeyDown(Key.X))
            //{
            //    r -= 0.1f;
            //}
            //else if (kbState.IsKeyDown(Key.C))
            //{
            //    r += 0.1f;
            //}

            //x += vX;
            //y += vY;

            //if (kbState.IsKeyDown(Key.Space))
            //{
            //    pvX = (float)Math.Cos(r);
            //    pvY = (float)Math.Sin(r);
            //    pX = x;
            //    pY = y;
            //}

            //pX += pvX;
            //pY += pvY;

            //gameplay.Update(delta);
        }

        public override void Draw(TimeSpan delta)
        {
            SpriteRender.Begin();
            SpriteRender.ResetView(800, 600);

            //SpriteRender.Draw(pX, pY, 0, 8, 8, texture, Color4.Red);
            //SpriteRender.Draw(x, y, 0, 32, 32, texture, Color4.Yellow, rotation: r);

            for (int i = 0, n = _sprites.Count(); i != n; ++i)
            {
                _sprites[i].Draw();
            }

            //gameplay.Draw(delta);
        }

        public abstract class Sprite
        {
            Texture2D _texture;
            int _width, _height;
            protected float _r;
            Vector2 _position;

            public Sprite(string texture, int width, int height)
            {
                _texture = TK.ContentPipeline.LoadTexture("Textures/" + texture);
                _width = width;
                _height = height;
            }

            public void SetPosition(float x, float y)
            {
                _position.X = x;
                _position.Y = y;
            }

            public void SetPosition(Vector2 position)
            {
                _position = position;
            }

            public void MovePosition(Vector2 move)
            {
                _position += move;
            }

            public Vector2 GetPosition()
            {
                return _position;
            }

            public void SetRotation(float rotation)
            {
                _r = rotation;
            }

            public abstract void Update();

            public virtual void Draw()
            {
                SpriteRender.Draw(_position.X, _position.Y, 0, _width, _height, _texture, Color4.White, rotation: _r);
            }
        }

        public class Player : Sprite
        {
            int _gamepadIndex;

            public Player(string texture, int gamepadIndex)
                : base(texture, 32, 32)
            {
                _gamepadIndex = gamepadIndex;
            }

            public override void Update()
            {
                var gamepadState = GamePad.GetState(_gamepadIndex);

                var shoot = gamepadState.ThumbSticks.Left;
                var move = gamepadState.ThumbSticks.Right;
                //SetRotation(move.Normalized());

                #region Keyboard Override
                if (_gamepadIndex == 1)
                {
                    var kbState = OpenTK.Input.Keyboard.GetState();
                    if (kbState.IsKeyDown(Key.Up))
                    {
                        move.Y = -1;
                    }
                    else if (kbState.IsKeyDown(Key.Down))
                    {
                        move.Y = 1;
                    }
                    if (kbState.IsKeyDown(Key.Left))
                    {
                        move.X = -1;
                    }
                    else if (kbState.IsKeyDown(Key.Right))
                    {
                        move.X = 1;
                    }

                    if (kbState.IsKeyDown(Key.X))
                    {
                        _r -= 0.1f;
                    }
                    else if (kbState.IsKeyDown(Key.C))
                    {
                        _r += 0.1f;
                    }
                    if (kbState.IsKeyDown(Key.Space))
                    {
                        shoot.X = (float)Math.Cos(_r);
                        shoot.Y = (float)Math.Sin(_r);
                    }
                }
                #endregion

                MovePosition(move);
                if (shoot.LengthFast > 0.25)
                {
                    var projectile = new Projectile(this, 1, 240);
                    _sprites.Add(projectile);
                    projectile.Start(GetPosition(), shoot.Normalized());
                    projectile.SetRotation(_r);
                }
            }
        }

        public class Projectile : Sprite
        {
            Sprite _parent;
            Vector2 _velocity, _startPosition;
            float _speed, _distance;
            bool move = false;

            public Projectile(Sprite parent, float speed, float distance)
                : base("bullet.png", 12, 6)
            {
                _parent = parent;
                _speed = speed;
                _distance = distance;
            }

            public void Start(Vector2 position, Vector2 velocity)
            {
                move = true;
                _velocity = velocity;
                _startPosition = position;
                SetPosition(position);
            }

            public override void Update()
            {
                if (move)
                {
                    MovePosition(_velocity.Normalized() * _speed);
                    if (_distance < (_startPosition - GetPosition()).LengthFast)
                    {
                        move = false;
                    }
                }
            }

            public override void Draw()
            {
                if (move)
                {
                    base.Draw();
                }
            }
        }
    }
}
