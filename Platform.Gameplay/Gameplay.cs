using OpenTK.Graphics;
using OpenTK.Input;
using Platform.Core;
using Platform.Entities;
using Platform.Entities.Sprites;
using Platform.Entities.Sprites.PickUps;
using Platform.Levels;
using Platform.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;

namespace Platform.Gameplay
{
    public enum GameState
    {
        Menu,
        Game,
        Dead,
        End,
        Credits,
    }

    public class Gameplay
    {
        private string currentLevel = null;
        private string checkpointLevel = "0-0";
        private KeyboardState KeyboardState = new KeyboardState();
        private KeyboardState PreviousKeyboardState = new KeyboardState();
        private World world = new World(0, 9.80);
        private Level level;
        private HUD hud;
        private GameState GameState { get; set; }
        private int highscore = 0;
        private int lastscore = 0;
        private DateTime startTime = DateTime.UtcNow;
        private DateTime endTime = DateTime.UtcNow;
        private int savedScore = 0;
        private int savedTokens = 0;
        private InventoryItem savedInventory = (InventoryItem)0;

        public void Load()
        {
            GameState = GameState.Game;
            LoadLevel(checkpointLevel, null);
            hud = new HUD();
            startTime = DateTime.UtcNow;
        }

        public void LoadLevel(string nextLevel, string previousLevel)
        {
            world._bodies.Clear();
            level = new Level(nextLevel, previousLevel, checkpointLevel);
            currentLevel = nextLevel;
            level.LoadLevelBodies(world);

            //level.Player.Inventory |=
            //    InventoryItem.DoubleJump |//
            //    InventoryItem.Run |
            //    InventoryItem.Weapon |
            //    InventoryItem.WeaponLength |
            //    InventoryItem.WeaponPower | // ;//
            //    InventoryItem.WeaponSpeed |
            //    InventoryItem.Key1 |
            //    InventoryItem.Key2 |
            //    InventoryItem.Key3 |
            //    InventoryItem.Key4;
        }

        private void Save()
        {
            var player = level.Player;

            checkpointLevel = currentLevel;

            savedTokens = player.Tokens;
            savedScore = player.Score;
            savedInventory = player.Inventory;

        }

        private void CheckPlayerStatus()
        {
            if (level != null && level.Player != null && level.Player.Status != null && level.Player.Status.Count > 0)
            {
                var status = level.Player.Status.Dequeue();

                switch (status.State)
                {
                    case PlayerState.Dead:
                        level.Player.Lives--;

                        if (level.Player.Lives <= 0)
                        {
                            GameOver();
                            break;
                        }

                        level.Player.Score = savedScore;
                        level.Player.Inventory = savedInventory;
                        level.Player.Tokens = savedTokens;
                        LoadLevel(checkpointLevel, null);
                        SoundEffects.Respawn.Play();
                        break;
                    case PlayerState.End:
                        End();
                        break;
                    case PlayerState.Warp:
                        var newLevel = status.Data;
                        LoadLevel(newLevel, currentLevel);
                        break;
                    case PlayerState.Checkpoint:
                        Save();
                        break;
                    case PlayerState.Kill:
                        var data = status.Data.Split(':');

                        var tokens = int.Parse(data[0]);
                        var x = int.Parse(data[1]);
                        var y = int.Parse(data[2]);

                        for (int i = 1, n = tokens; i <= n; ++i)
                        {
                            var _m = (i % 2 == 1 ? -1 : 1);
                            var _x = (_m * 4 * ((int)(i / 2))) + x;
                            var _y = y - 2;

                            var token = new Token(_x, _y);
                            token.ApplyImpulse((_m * 40) * ((int)(i / 2) * 1.5), -150);

                            world.AddBody(token);
                        }

                        level.Player.AddMessage("100", x, y, false, Color4.Red, 1d, true);
                        level.Player.Score += 100;

                        break;
                }

                //if (level != null && level.Player != null && level.Player.Status != null)
                //{
                //    status.State = PlayerState.None;
                //}
            }
        }

        private void UpdateKeyboardState()
        {
            PreviousKeyboardState = KeyboardState;

            KeyboardState = Keyboard.GetState();
        }

        private void GameOver()
        {
            lastscore = level.Player.Score;
            endTime = DateTime.UtcNow;

            highscore = Math.Max(lastscore, highscore);

            level.Player = null;
            checkpointLevel = "0-0";

            GameState = GameState.Dead;
        }

        private void End()
        {
            Save();

            endTime = DateTime.UtcNow;
            highscore = Math.Max(savedScore, highscore);
            level.Player = null;

            GameState = GameState.End;
        }

        public void Update(TimeSpan delta)
        {
            switch (GameState)
            {
                case GameState.Game:
                    UpdateGame(delta);
                    break;
                case GameState.Dead:
                case GameState.End:
                    UpdateDead();
                    break;
                default:
                    break;
            }
        }

        public void UpdateGame(TimeSpan delta)
        {
            world.Step(delta);
            UpdateKeyboardState();

            foreach (var body in world._bodies)
            {
                var sprite = body as Sprite;

                sprite.Update(delta, KeyboardState, PreviousKeyboardState);
            }

            CheckPlayerStatus();
            hud.Update(level.Player);

            if (level.Boss != null)
            {
                if (level.Boss.Dead && !level.Boss.Gifted)
                {
                    level.Boss.Gifted = true;
                    level.Boss.Gift.X = level.Boss.X + (level.Boss.Width / 2);
                    level.Boss.Gift.Y = level.Boss.Y;

                    world.RemoveBody(level.Boss);
                    world.AddBody(level.Boss.Gift);
                }
            }
        }

        public void UpdateDead()
        {
            UpdateKeyboardState();

            if (KeyboardState.IsKeyDown(Key.Enter))
            {
                Load();
            }
        }

        public void Draw(TimeSpan delta)
        {
            switch (GameState)
            {
                case Platform.Gameplay.GameState.Game:
                    DrawGame(delta);
                    break;
                case Platform.Gameplay.GameState.Dead:
                    DrawDead();
                    break;
                case Platform.Gameplay.GameState.End:
                    DrawEnd();
                    break;
                default:
                    break;
            }
        }

        public void DrawGame(TimeSpan delta)
        {
            SpriteRender.SetView((float)level.Player.X, (float)level.Player.Y, (float)800, (float)600, 5.0f);

            foreach (var body in world._bodies)
            {
                var sprite = body as Sprite;

                sprite.Draw();
            }

            hud.Draw();
        }

        public void DrawDead()
        {
            var totalTime = endTime.Subtract(startTime);
            SpriteRender.ResetView(800, 600);
            TextRender.Draw(100f, 100f, 0f, "GAME OVER", Fonts.Standard, Color4.Red, 5f);
            TextRender.Draw(100f, 460f, 0f, "TIME: " + (int)totalTime.TotalMinutes + ":" + totalTime.Seconds.ToString("00"), Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 480f, 0f, "SCORE: " + lastscore, Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 500f, 0f, "HIGHSCORE: " + highscore, Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 520f, 0f, "PRESS ENTER TO PLAY AGAIN", Fonts.Standard, Color4.White, 2f);
        }

        public void DrawEnd()
        {
            var totalTime = endTime.Subtract(startTime);
            SpriteRender.ResetView(800, 600);
            TextRender.Draw(100f, 100f, 0f, "CONGRATULATIONS", Fonts.Standard, Color4.Green, 5f);
            TextRender.Draw(100f, 150f, 0f, "", Fonts.Standard, Color4.White, 1.75f);
            TextRender.Draw(100f, 440f, 0f, "TIME: " + (int)totalTime.TotalMinutes + ":" + totalTime.Seconds.ToString("00"), Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 460f, 0f, "TOKENS: " + savedTokens, Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 480f, 0f, "SCORE: " + savedScore, Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 500f, 0f, "HIGHSCORE: " + highscore, Fonts.Standard, Color4.White, 2f);
            TextRender.Draw(100f, 520f, 0f, "PRESS ENTER TO PLAY AGAIN", Fonts.Standard, Color4.White, 2f);
        }
    }
}
