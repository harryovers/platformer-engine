using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Entities.Sprites.PickUps
{
    public class Checkpoint : PickUp
    {
        public bool IsActive { get; set; }

        public Checkpoint(int x, int y, bool isActive)
            : base(x, y)
        {
            this.ID = "checkpoint";
            this.Width = 10;
            this.Height = 10;
            this.IsStatic = true;
            this.IsActive = isActive;

            SetTexture();
        }

        private void SetTexture()
        {
            this.Texture = IsActive ? Textures.Checkpoint_Active : Textures.Checkpoint_Inactive;
        }

        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                SetTexture();
                SoundEffects.Save.Play();
            }
        }
    }
}
