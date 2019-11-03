using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Platform.Core;
using Platform.Entities.Sprites.PickUps;
using Platform.Entities.Sprites.PickUps.PowerUps;
using Platform.Entities.Sprites.PickUps.PowerUps.Weapons;
using Platform.Entities.Sprites.Statics;
using Platform.Entities.Sprites.Warps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;

namespace Platform.Entities.Sprites
{
    public class PlayerStatus
    {
        public PlayerState State { get; set; }
        public string Data { get; set; }
    }

    public enum PlayerState
    {
        None,
        Dead,
        Warp,
        Checkpoint,
        Kill,
        End
    }

    public class Message
    {
        public string Text { get; set; }
        public TimeSpan Time { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool FollowPlayer { get; set; }
        public bool MoveUp { get; set; }
        public Color4 Color { get; set; }
    }

    public class Player : Sprite
    {
        public Queue<PlayerStatus> Status { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        private List<Message> messages = new List<Message>();
        private Killbox killbox;
        public Killbox Killbox
        {
            get { return killbox; }
        }
        public int PlayerDirection = 1;
        public int Health { get; set; }
        public int MaxHealth
        {
            get { return 1000; }
        }
        private bool attacking = false, attackStarted = false;
        private Vector2 attackMinSize = new Vector2(0, 5);
        //private Vector2 attackMaxSize = new Vector2(7.5f, 0);
        //private Vector2 attackSpeed = new Vector2(0.4f, 0);
        private bool jumping = false, jumpingSecond = false;
        private bool saveMessaged = false;
        public InventoryItem Inventory;
        private Texture2D currentTexture;
        public int Tokens { get; set; }

        public Player(int x, int y)
            : base (SpriteType.Character, x, y)
        {
            this.Lives = 3;
            this.Z = 0.1;
            this.CollisionGroup = 1;
            this.Width = 5;
            this.Height = 9;
            this.NegativeTextureOffset = new OpenTK.Vector2(1f, 1f);
            this.PositiveTextureOffset = new OpenTK.Vector2(2f, 1f);
            this.Drag = 0.01;
            this.Mass = 7;
            this.ID = "player";
            this.Health = MaxHealth;
            this.DrawOrder = Physics.DrawOrder.Player;
            this.Texture = Platform.Core.Textures.Player;
            currentTexture = this.Texture;
            this.Status = new Queue<PlayerStatus>();

            killbox = new Killbox(0, 0);
            killbox.Damageful = 3;
        }

        public void ResetPlayer()
        {
            Health = MaxHealth;
        }

        public override void Update(TimeSpan delta, KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            CheckKeyboardState(keyboardState, previousKeyboardState);
            UpdateKillBox(delta);
            UpdateMessages(delta);
            CheckCollisions();

            // update texture
            if (!jumping)
            {
                if (!attacking)
                {
                    var stepProgress = (int)(this.X % 4);
                    switch (stepProgress)
                    {
                        case 0:
                            currentTexture = Textures.Player_Walking_1;
                            break;
                        case 1:
                            currentTexture = Textures.Player_Walking_0;
                            break;
                        case 2:
                            currentTexture = Textures.Player_Walking_1;
                            break;
                        case 3:
                            currentTexture = Textures.Player_Walking_2;
                            break;
                    }
                }
                else
                {
                    currentTexture = Textures.Player_Attacking_0;
                }
            }
            else
            {
                if (!jumpingSecond)
                {
                    if (attacking)
                    {
                        currentTexture = Textures.Player_Attacking_Jumping_0;
                    }
                    else
                    {
                        currentTexture = Textures.Player_Jumping_0;
                    }
                }
                else
                {
                    if (attacking)
                    {
                        currentTexture = Textures.Player_Attacking_Jumping_1;
                    }
                    else
                    {
                        currentTexture = Textures.Player_Jumping_1;
                    }
                }
            }
        }

        private void UpdateKillBox(TimeSpan delta)
        {
            if (killbox != null)
            {
                if (attacking)
                {
                    if (!attackStarted)
                    {
                        attackStarted = true;
                        killbox.Height = attackMinSize.Y;
                        killbox.Width = attackMinSize.X;
                    }

                    if (Inventory.HasFlag(InventoryItem.WeaponPower))
                    {
                        killbox.Texture = Platform.Core.Textures.Killbox_Strong;
                    }

                    killbox.Damageful = Inventory.HasFlag(InventoryItem.WeaponPower) ? 8 : 3;
                    killbox.Width += GetWeaponSpeed().X; // Inventory.HasFlag(InventoryItem.WeaponSpeed) ? 0.4 : 0.25;
                    killbox.Y = this.Y + ((this.Height - killbox.Height) / 2); // might want to adjust offset by weapon type

                    if (PlayerDirection == -1)
                    {
                        killbox.X = this.X - killbox.Width;
                    }
                    else
                    {
                        killbox.X = this.X + this.Width;
                    }

                    if (killbox.Width > GetWeaponMaxSize().X && killbox.Height > GetWeaponMaxSize().Y)// (Inventory.HasFlag(InventoryItem.WeaponLength) ? attackMaxSize * 1.5 : attackMaxSize))
                    {
                        attacking = false;
                    }

                    killbox.HandleStaticCollisions();
                }
                else
                {
                    killbox.X = 0;
                    killbox.Y = 0;
                    killbox.Width = 0;
                    killbox.Height = 0;
                    attackStarted = false;
                }
            }
        }

        private Vector2 GetWeaponSpeed()
        {
            if (Inventory.HasFlag(InventoryItem.WeaponSpeed))
            {
                return new Vector2(0.4f, 0);
            }
            else
            {
                return new Vector2(0.25f, 0);
            }
        }

        private Vector2 GetWeaponMaxSize()
        {
            if (Inventory.HasFlag(InventoryItem.WeaponLength))
            {
                return new Vector2(7.5f * 1.5f, 0);
            }
            else
            {
                return new Vector2(7.5f, 0);
            }
        }

        private void CheckCollisions()
        {
            var bottomColide = false;
            var collisionList = this.CollisionList.ToArray();

            var containsCheckpoint = false;

            foreach (var body in collisionList)
            {
                if (body is Checkpoint)
                {
                    containsCheckpoint = true;
                }
            }

            if (!containsCheckpoint)
            {
                saveMessaged = false;
            }

            foreach (var body in collisionList)
            {
                var sprite = body as Sprite;

                if (sprite != null)
                {
                    if (sprite.SpriteType == SpriteType.Deathbox)
                    {
                        Dead = true;
                        var status = new PlayerStatus();
                        status.State = PlayerState.Dead;

                        Status.Enqueue(status);

                        return;
                    }

                    if (sprite.SpriteType == SpriteType.Character)
                    {
                        if (!sprite.Dead)
                        {
                            Health -= sprite.Damageful;

                            if (Health <= 0)
                            {
                                Dead = true;
                                var status = new PlayerStatus();
                                status.State = PlayerState.Dead;

                                Status.Enqueue(status);

                                return;
                            }
                        }
                    }

                    if (sprite.SpriteType == SpriteType.PickUp)
                    {
                        if (sprite is Token)
                        {
                            Health += 2;

                            if (Health > MaxHealth)
                            {
                                Health = MaxHealth;
                            }

                            sprite.Dead = true;

                            // TODO: play a sound here

                            Tokens++;

                            if (Tokens >= 100)
                            {
                                Lives++;
                                Tokens -= 100;
                            }

                            Score += 10;
                            AddMessage("+10", (float)sprite.X - 2, (float)sprite.Y, false, Color4.Yellow, 1, true);
                        }
                        else if (sprite is Star)
                        {
                            Health += 100;

                            if (Health > MaxHealth)
                            {
                                Health = MaxHealth;
                            }

                            sprite.Dead = true;

                            SoundEffects.Pickup.Play();

                            Score += 50;
                            AddMessage("+50", (float)sprite.X + 1, (float)sprite.Y, false, Color4.Yellow, 1, true);
                        }
                        else if (sprite is Checkpoint)
                        {
                            var checkpoint = sprite as Checkpoint;
                            checkpoint.Activate();

                            var status = new PlayerStatus();
                            status.State = PlayerState.Checkpoint;

                            Status.Enqueue(status);

                            if (!saveMessaged)
                            {
                                saveMessaged = true;
                                AddMessage("SAVE", (float)sprite.X - 0, (float)sprite.Y, false, Color4.Orange, 1, true);
                            }
                        }
                        else if (sprite is DoorKey)
                        {
                            var key = sprite as DoorKey;

                            switch (key.LockID)
                            {
                                case 1:
                                    Inventory |= InventoryItem.Key1;
                                    break;
                                case 2:
                                    Inventory |= InventoryItem.Key2;
                                    break;
                                case 3:
                                    Inventory |= InventoryItem.Key3;
                                    break;
                                case 4:
                                    Inventory |= InventoryItem.Key4;
                                    break;
                            }

                            sprite.Dead = true;

                            SoundEffects.Key.Play();
                        }
                        else if (sprite is PowerUp)
                        {
                            if (sprite is Weapon)
                            {
                                Inventory |= InventoryItem.Weapon;

                                if (sprite is BasicWeapon)
                                {
                                }
                                else if (sprite is LongWeapon)
                                {
                                    Inventory |= InventoryItem.WeaponLength;
                                }
                                else if (sprite is StrongWeapon)
                                {
                                    Inventory |= InventoryItem.WeaponPower;
                                }
                                else if (sprite is FastWeapon)
                                {
                                    Inventory |= InventoryItem.WeaponSpeed;
                                }
                            }
                            else if (sprite is DoubleJump)
                            {
                                Inventory |= InventoryItem.DoubleJump;
                                AddMessage("DOUBLE JUMP", (float)sprite.X - 8, (float)sprite.Y, false, null, 2, true);
                            }
                            else if (sprite is Run)
                            {
                                Inventory |= InventoryItem.Run;
                                AddMessage("SPEED BOOTS", (float)sprite.X - 8, (float)sprite.Y, false, null, 2, true);
                            }

                            sprite.Dead = true;

                            SoundEffects.Pickup.Play();
                        }
                    }

                    if (sprite.SpriteType == SpriteType.Warp)
                    {
                        if (sprite is Door)
                        {
                            var door = sprite as Door;

                            if (door.IsLocked)
                            {
                                if (CanUnlockDoor(door))
                                {
                                    door.IsLocked = false;
                                }
                                else
                                {
                                    AddMessage("LOCKED", (float)sprite.X - 2.5f, (float)sprite.Y - 5, false, null, 1);
                                }
                            }

                            if (!door.IsLocked)
                            {
                                if (door.IsExit)
                                {
                                    var status = new PlayerStatus();
                                    status.State = PlayerState.End;
                                    status.Data = (sprite as Door).Level;

                                    Status.Enqueue(status);
                                }
                                else
                                {
                                    var status = new PlayerStatus();
                                    status.State = PlayerState.Warp;
                                    status.Data = (sprite as Door).Level;

                                    Status.Enqueue(status);
                                }
                            }
                        }
                    }

                    if (sprite.SpriteType == SpriteType.Static)
                    {
                        if (this.TopBottomColide(body) || this.LeftRightColide(body))
                        {
                            if (this.BottomColide(body))
                            {
                                if (!bottomColide)
                                {
                                    if (this.ImpY >= 0)
                                    {
                                        jumping = false;
                                        jumpingSecond = false;
                                    }

                                    bottomColide = true;
                                }
                            }
                        }

                        //(sprite as Block).Color = Color4.White;
                    }
                }
            }
        }

        private bool CanUnlockDoor(Door door)
        {
            if ((int)(Inventory & InventoryItem.Key1) > 0 && door.LockID == 1)
            {
                return true;
            }

            if ((int)(Inventory & InventoryItem.Key2) > 0 && door.LockID == 2)
            {
                return true;
            }

            if ((int)(Inventory & InventoryItem.Key3) > 0 && door.LockID == 3)
            {
                return true;
            }

            if ((int)(Inventory & InventoryItem.Key4) > 0 && door.LockID == 4)
            {
                return true;
            }

            return false;
        }

        private void CheckKeyboardState(KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            //if (keyboardState == null && previousKeyboardState == null)
            //{
            //    return;
            //}
            if (keyboardState.IsKeyDown(Key.Left))
            {
                Move(/*Inventory.HasFlag(InventoryItem.Run) ? -8 : */-5);
            }
            if (keyboardState.IsKeyDown(Key.Right))
            {
                Move(/*Inventory.HasFlag(InventoryItem.Run) ? 8 : */5);
            }
            if (keyboardState.IsKeyDown(Key.Up) && previousKeyboardState.IsKeyUp(Key.Up))
            {
                Jump(500);
            }
            if (keyboardState.IsKeyDown(Key.Space) && previousKeyboardState.IsKeyUp(Key.Space))
            {
                Attack();
            }

            if (!keyboardState.IsKeyDown(Key.Left) && !keyboardState.IsKeyDown(Key.Right))
            {
                Break();
            }
        }

        private void Jump(int force)
        {
            var jumpAllowed = false;
            var isSecondJump = false;

            if (jumping)
            {
                if (!jumpingSecond && (int)(Inventory & InventoryItem.DoubleJump) > 0)
                {
                    jumpAllowed = true;
                    jumpingSecond = true;
                    isSecondJump = true;
                }
            }

            if (!jumping)
            {
                foreach (var body in this.CollisionList.Where(b => b.IsStatic))
                {
                    if (this.BottomColide(body))
                    {
                        var angleOfColision = this.Center - body.Center;

                        if (angleOfColision.X < 0)
                        {
                            angleOfColision.X *= -1;
                        }
                        if (angleOfColision.Y < 0)
                        {
                            angleOfColision.Y *= -1;
                        }

                        if (angleOfColision.Y >= angleOfColision.X)
                        {
                            jumpAllowed = true;
                            jumping = true;
                            jumpingSecond = false;
                            break;
                        }
                    }
                }
            }

            if (jumpAllowed)
            {
                this.ApplyImpulse(0, (force * -1) * (isSecondJump ? 0.75 : 1));
                if (isSecondJump)
                {
                    SoundEffects.Jump_Second.Play();
                }
                else
                {
                    SoundEffects.Jump.Play();
                }
            }
        }

        private void Move(int force)
        {
            if (force > 0)
            {
                this.PlayerDirection = 1;
            }
            else
            {
                this.PlayerDirection = -1;
            }

            ApplyImpulse(Acceleration() * force, 0);
        }

        private double Acceleration()
        {
            var vel = Math.Abs(this.VelX);

            if (vel > (Inventory.HasFlag(InventoryItem.Run) ? (!jumping ? 40 : 30) : 20))
            {
                return 0;
            }
            if (vel > 35)
            {
                return 0.7;
            }
            if (vel > 25)
            {
                return 0.6;
            }
            if (vel > 15)
            {
                return 0.5;
            }
            return 1;
        }

        private void Break()
        {
            var vel = Math.Abs(this.VelX);
            if (vel > 1)
            {
                if (this.VelX > 0)
                {
                    this.VelX--;
                }
                else
                {
                    this.VelX++;
                }
            }
            else
            {
                this.VelX = 0;
            }
        }

        private void Attack()
        {
            if (Inventory.HasFlag(InventoryItem.Weapon))
            {
                if (killbox != null)
                {
                    if (!attacking && killbox.Width == 0)
                    {
                        attacking = true;
                        killbox.Used = false;
                        SoundEffects.Attack.Play();
                    }
                }
            }
        }

        public void AddMessage(string text, float x, float y, bool followPlayer, Color4? color = null, double seconds = 2, bool moveUp = false)
        {
            if (color == null)
            {
                color = Color4.White;
            }

            messages.Add(new Message()
            {
                Text = text,
                Time = TimeSpan.FromSeconds(seconds),
                X = x,
                Y = y,
                FollowPlayer = followPlayer,
                MoveUp = moveUp,
                Color = color.Value
            });
        }

        private void UpdateMessages(TimeSpan delta)
        {
            foreach (var message in messages)
            {
                message.Time = message.Time.Subtract(delta);

                if (message.MoveUp)
                {
                    message.Y -= (float)delta.TotalMilliseconds / 25;
                }
            }
        }

        private void DrawMessages()
        {
            foreach (var message in messages)
            {
                if (message.Time > TimeSpan.Zero)
                {
                    var color = message.Color;

                    if (message.Time < TimeSpan.FromSeconds(1))
                    {
                        color.A = (float)message.Time.TotalMilliseconds / 1000;
                    }

                    TextRender.Draw(message.X, message.Y, 0.0f, message.Text, Fonts.Standard, color, 0.3f);
                }
            }
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
                currentTexture,
                Color4.White,
                //jumping ? (jumpingSecond ? Color4.Red : Color4.Green) : Color4.White,
                1.0f,
                PlayerDirection == 1);

            DrawMessages();
        }
    }
}
