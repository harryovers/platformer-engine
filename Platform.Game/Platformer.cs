using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.Gameplay;
using TK.Graphics;
using Platform.Core;

namespace Platform.Game
{
    public class Platformer : TK.Game
    {
        Gameplay.Gameplay gameplay;

        public Platformer()
            : base(800, 600)
        {
            this.Title = "Platformer";
            gameplay = new Gameplay.Gameplay();
            this.WindowBorder = OpenTK.WindowBorder.Fixed;
        }

        public override void LoadGame()
        {
            Loader.LoadTextures();
            Loader.LoadSoundEffects();
            Loader.LoadFonts();
            gameplay.Load();
        }

        public override void Update(TimeSpan delta)
        {
            if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.Escape))
            {
                this.Close();
            }

            gameplay.Update(delta);
        }

        public override void Draw(TimeSpan delta)
        {
            SpriteRender.Begin();

            gameplay.Draw(delta);
        }
    }
}
