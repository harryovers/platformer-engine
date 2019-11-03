using OpenTK.Graphics;
using Platform.Core;
using Platform.Entities;
using Platform.Entities.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Graphics;

namespace Platform.Gameplay
{
    public class HUD
    {
        float health = 100;
        float healthBarWidth = 150;
        InventoryItem inventory = InventoryItem.Empty;
        int score = 0;
        int lives = 0;
        int tokens = 0;

        public void Update(Player player)
        {
            if (player != null)
            {
                health = (int)((((float)player.Health / (float)player.MaxHealth) * 100) + 0.5);
                inventory = player.Inventory;
                score = player.Score;
                lives = player.Lives;
                tokens = player.Tokens;
            }
        }

        public void Draw()
        {
            SpriteRender.ResetView(800, 600);
            SpriteRender.DrawRect(10, 10, 0.0f, healthBarWidth, 15, Color4.DarkRed);
            SpriteRender.DrawRect(10, 10, 0.0f, (health / 100) * healthBarWidth, 15, Color4.Red);

            if (score != 0)
            {
                TextRender.Draw(350f, 10f, 0.0f, score.ToString(), Fonts.Standard, Color4.White, 2f);
            }

            TextRender.Draw(500f, 10f, 0.0f, lives + "", Fonts.Standard, Color4.Yellow, 2f);
            TextRender.Draw(550f, 10f, 0.0f, tokens + "", Fonts.Standard, Color4.Yellow, 2f);

            if ((int)(inventory & InventoryItem.Key1) > 0)
            {
                SpriteRender.Draw(170, 5, 0, 20, 20, Textures.Key);
            }
            if ((int)(inventory & InventoryItem.Key2) > 0)
            {
                SpriteRender.Draw(200, 5, 0, 20, 20, Textures.Key);
            }
            if ((int)(inventory & InventoryItem.Key3) > 0)
            {
                SpriteRender.Draw(230, 5, 0, 20, 20, Textures.Key);
            }
            if ((int)(inventory & InventoryItem.Key4) > 0)
            {
                SpriteRender.Draw(260, 5, 0, 20, 20, Textures.Key);
            }
        }
    }
}
