using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;

namespace Platform.Entities.Sprites.Warps
{
    public class Door : Warp
    {
        public bool IsLocked { get; set; }
        public int LockID { get; set; }
        public string Level { get; set; }
        public Direction Start { get; set; }
        public bool IsExit { get; set; }

        public Door(int x, int y, string level, Direction direction, Direction start, int? lockId = null)
            : base (x, y)
        {
            this.ID = "door-" + level;
            this.Width = 9;
            this.Height = 16;
            Level = GetNextLevel(level, direction);
            Start = start;
            IsExit = false;

            if (lockId.HasValue)
            {
                IsLocked = true;
                LockID = lockId.Value;
            }
            else
            {
                IsLocked = false;
            }

            SetTexture();
        }

        public void SetTexture()
        {
            this.Texture = IsLocked ? Textures.LockedDoor : Textures.Door;
        }

        public string GetNextLevel(string level, Direction direction)
        {
            var split = level.Split('-');

            int x = 0, y = 0;

            int.TryParse(split[0], out x);
            int.TryParse(split[1], out y);
            switch (direction)
            {
                case Direction.Up:
                    x--;
                    break;
                case Direction.Down:
                    x++;
                    break;
                case Direction.Left:
                    y--;
                    break;
                case Direction.Right:
                    y++;
                    break;
            }

            return string.Format("{0}-{1}", Math.Abs(x), Math.Abs(y));
        }

        public override void Draw()
        {
            if (IsExit)
            {
                SpriteRender.Draw(
                    this.X - this.NegativeTextureOffset.X,
                    this.Y - this.NegativeTextureOffset.Y - 10,
                    this.Z,
                    this.Width + this.PositiveTextureOffset.X,
                    (this.Height + this.PositiveTextureOffset.Y) / 2,
                    Textures.ExitDoor);
            }

            base.Draw();
        }
    }
}
