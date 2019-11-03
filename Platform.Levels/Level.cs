using Platform.Core;
using Platform.Entities;
using Platform.Entities.Sprites;
using Platform.Entities.Sprites.Bots;
using Platform.Entities.Sprites.Bots.Bosses;
using Platform.Entities.Sprites.Deathboxes;
using Platform.Entities.Sprites.PickUps;
using Platform.Entities.Sprites.PickUps.PowerUps;
using Platform.Entities.Sprites.PickUps.PowerUps.Weapons;
using Platform.Entities.Sprites.Statics;
using Platform.Entities.Sprites.Statics.Blocks;
using Platform.Entities.Sprites.Warps;
using Platform.Entities.Sprites.Warps.Doors;
using Platform.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Levels
{
    public class Level
    {
        private List<Body> _bodies = new List<Body>();

        private static Player _player = null;
        public Player Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
            }
        }

        private Boss _boss = null;
        public Boss Boss
        {
            get { return _boss; }
            set { _boss = value; }
        }
        

        public Level(string levelName, string previousLevel, string checkpointLevel)
        {
            Bitmap bmp = new Bitmap(string.Format("Levels/{0}.bmp", levelName));

            for (int x = 0, nX = bmp.Width; x < nX; x++)
            {
                for (int y = 0, nY = bmp.Height; y < nY; y++)
                {
                    var color = bmp.GetPixel(x, y);

                    switch (color.Name)
                    {
                        case "ffffffff":
                            // no object
                            break;
                        case "ff000000":
                            var block = new Block(x * 10, y * 10);

                            _bodies.Add(block);
                            break;
                        case "ff00ff00":
                            var checkpoint = new Checkpoint(x * 10, y * 10, levelName == checkpointLevel);

                            _bodies.Add(checkpoint);

                            if (previousLevel == null)
                            {
                                AddPlayer(x, y, 1);
                                Player.ResetPlayer();
                            }
                            break;
                        case "ffff0000":
                            var spikes = new Spikes(x * 10, (y * 10) + 1);

                            _bodies.Add(spikes);
                            break;
                        case "ff00a0c0":
                            var jumper = new Jumper(x * 10, y * 10);

                            jumper.Target = Player;

                            _bodies.Add(jumper);
                            break;
                        case "ff0000ff":
                            var enemy = new Spider(x * 10, y * 10);

                            enemy.Target = Player;

                            _bodies.Add(enemy);
                            break;
                        case "ffa6caf0":
                            var something = new Charger(x * 10, y * 10);

                            something.Target = Player;

                            _bodies.Add(something);
                            break;
                        case "ff000040":
                            var stomper = new Stomper((x * 10) - 20, (y * 10) - 11);

                            stomper.Target = Player;

                            var stomperGift = new DoorKey(0, 0);
                            stomperGift.LockID = 4;
                            stomperGift.Mass = 5;
                            stomperGift.IsStatic = false;
                            stomperGift.Drag = 0.01;
                            stomperGift.CollisionGroup = 8055;

                            stomper.Gift = stomperGift;

                            _boss = stomper;
                            _bodies.Add(stomper);
                            break;
                        case "ffffff00":
                            var star = new Star(x * 10, y * 10);

                            _bodies.Add(star);
                            break;
                        case "ffff00ff":
                            var rightDoor = new RightDoor(x * 10, (y * 10) - 6, levelName, Direction.Left);

                            if (previousLevel == (rightDoor as Door).Level)
                            {
                                AddPlayer(x + (int)((rightDoor as Door).Start), y, -1);
                            }

                            if (levelName == "0-4")
                            {
                                rightDoor.IsLocked = true;
                                rightDoor.LockID = 3;
                                rightDoor.SetTexture();
                            }

                            _bodies.Add(rightDoor);
                            break;
                        case "ff800080":
                            var leftDoor = new LeftDoor(x * 10, (y * 10) - 6, levelName, Direction.Right);

                            if (previousLevel == (leftDoor as Door).Level)
                            {
                                AddPlayer(x + (int)((leftDoor as Door).Start), y, 1);
                            }

                            if (levelName == "0-0")
                            {
                                leftDoor.IsLocked = true;
                                leftDoor.LockID = 4;
                                leftDoor.IsExit = true;
                                leftDoor.SetTexture();
                            }

                            if (levelName == "2-2")
                            {
                                leftDoor.IsLocked = true;
                                leftDoor.LockID = 2;
                                leftDoor.SetTexture();
                            }

                            _bodies.Add(leftDoor);
                            break;
                        case "ff8000c0":
                            var downDoor = new DownDoor(x * 10, (y * 10) - 6, levelName, Direction.Left);

                            if (previousLevel == (downDoor as Door).Level)
                            {
                                AddPlayer(x + (int)((downDoor as Door).Start), y, -1);
                            }

                            if (levelName == "2-2")
                            {
                                downDoor.IsLocked = true;
                                downDoor.LockID = 1;
                                downDoor.SetTexture();
                            }

                            if (levelName == "3-2")
                            {
                                downDoor.IsLocked = true;
                                downDoor.LockID = 4;
                                downDoor.SetTexture();
                            }

                            _bodies.Add(downDoor);
                            break;
                        case "ffe00080":
                            var upDoor = new UpDoor(x * 10, (y * 10) - 6, levelName, Direction.Right);

                            if (previousLevel == (upDoor as Door).Level)
                            {
                                AddPlayer(x + (int)((upDoor as Door).Start), y, 1);
                            }

                            if (levelName == "4-2")
                            {
                                upDoor.IsLocked = true;
                                upDoor.LockID = 4;
                                upDoor.SetTexture();
                            }

                            _bodies.Add(upDoor);
                            break;
                        case "ffa0a000":
                            var key = new DoorKey(x * 10, y * 10);

                            switch (levelName)
                            {
                                case "1-1":
                                    key.LockID = 2;
                                    break;
                                case "2-0":
                                    key.LockID = 1;
                                    break;
                                case "1-3":
                                    key.LockID = 3;
                                    break;
                            }

                            _bodies.Add(key);
                            break;
                        case "ffe0c000":
                            if (_player != null && _player.Inventory.HasFlag(InventoryItem.DoubleJump))
                            {
                                break;
                            }

                            var doubleJump = new DoubleJump(x * 10, y * 10);

                            _bodies.Add(doubleJump);
                            break;
                        case "ffc0a000":
                            if (_player != null && _player.Inventory.HasFlag(InventoryItem.Run))
                            {
                                break;
                            }

                            var run = new Run(x * 10, y * 10);

                            _bodies.Add(run);
                            break;
                        case "ffc0e000":
                            if (_player != null && _player.Inventory.HasFlag(InventoryItem.Weapon))
                            {
                                break;
                            }

                            var basicWeapon = new BasicWeapon(x * 10, y * 10);

                            _bodies.Add(basicWeapon);
                            break;
                        case "ffa0c000":
                            if (_player != null && _player.Inventory.HasFlag(InventoryItem.WeaponPower))
                            {
                                break;
                            }

                            var strongWeapon = new StrongWeapon(x * 10, y * 10);

                            _bodies.Add(strongWeapon);
                            break;
                        case "ffe0e040":
                            if (_player != null && _player.Inventory.HasFlag(InventoryItem.WeaponSpeed))
                            {
                                break;
                            }

                            var fastWeapon = new FastWeapon(x * 10, y * 10);

                            _bodies.Add(fastWeapon);
                            break;
                        case "ff608000":
                            if (_player != null && _player.Inventory.HasFlag(InventoryItem.WeaponLength))
                            {
                                break;
                            }

                            var longWeapon = new LongWeapon(x * 10, y * 10);

                            _bodies.Add(longWeapon);
                            break;
                        case "ffc0c0c0":
                            var botBoundry = new BotBoundry(x * 10, y * 10);

                            _bodies.Add(botBoundry);
                            break;
                        case "ff808080":
                            var breakable = new Breakable(x * 10, y * 10);

                            _bodies.Add(breakable);
                            break;
                        case "ffa0a0a4":
                            var bossBoundry = new BossBoundry(x * 10, y * 10);

                            _bodies.Add(bossBoundry);
                            break;
                        default:
                            break;
                    }
                }
            }

            _bodies = _bodies.OrderBy(b => b.DrawOrder).ToList();
        }

        public void LoadLevelBodies(World world)
        {
            foreach (var body in _bodies)
            {
                world.AddBody(body);
            }
        }

        private void AddPlayer(int x, int y, int direction)
        {
            var player = new Player(x * 10, (y * 10) - 2);
            player.PlayerDirection = direction;
            if (Player != null)
            {
                player.Inventory = Player.Inventory;
                player.Health = Player.Health;
                player.Score = Player.Score;
                player.Lives = Player.Lives;
                player.Tokens = Player.Tokens;
            }

            _bodies.Add(player);
            _bodies.Add(player.Killbox);

            Player = player;

            foreach (var enemy in _bodies.Where(b => b is Bot).Select(b => b as Bot))
            {
                enemy.Target = Player;
            }
        }
    }
}
