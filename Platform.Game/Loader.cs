using Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Game
{
    public static class Loader
    {
        public static void LoadTextures()
        {
            Textures.Player = TK.ContentPipeline.LoadTexture("man.png");
            Textures.Player_Walking_0 = Textures.Player;
            Textures.Player_Walking_1 = TK.ContentPipeline.LoadTexture("man2.png");
            Textures.Player_Walking_2 = TK.ContentPipeline.LoadTexture("man3.png");
            Textures.Player_Jumping_0 = Textures.Player_Walking_2;
            Textures.Player_Jumping_1 = Textures.Player_Walking_2;
            Textures.Player_Attacking_0 = Textures.Player_Walking_1;
            Textures.Player_Attacking_1 = Textures.Player_Walking_1;
            Textures.Player_Attacking_Jumping_0 = Textures.Player_Walking_2;
            Textures.Player_Attacking_Jumping_1 = Textures.Player_Walking_2;
            Textures.Player = TK.ContentPipeline.LoadTexture("man.png");
            Textures.Spikes = TK.ContentPipeline.LoadTexture("spikes.png");
            Textures.Spider = TK.ContentPipeline.LoadTexture("enemy.png");
            Textures.Jumper = TK.ContentPipeline.LoadTexture("jumper.png");
            Textures.Jumper_Damaged = TK.ContentPipeline.LoadTexture("jumperDamaged.png");
            Textures.Charger = TK.ContentPipeline.LoadTexture("charger.png");
            Textures.Charger_Damaged = TK.ContentPipeline.LoadTexture("chargerDamaged.png");
            Textures.Stomper = TK.ContentPipeline.LoadTexture("stomper.png");
            Textures.Stomper_Angry= TK.ContentPipeline.LoadTexture("stomperAngry.png");
            Textures.Stomper_Floating = TK.ContentPipeline.LoadTexture("stomper.png");
            Textures.Stomper_Retreating = TK.ContentPipeline.LoadTexture("stomperRetreating.png");
            Textures.Stomper_Stomping = TK.ContentPipeline.LoadTexture("stomper.png");
            Textures.Block = TK.ContentPipeline.LoadTexture("block.png");
            Textures.Checkpoint_Active = TK.ContentPipeline.LoadTexture("checkpointActive.png");
            Textures.Checkpoint_Inactive = TK.ContentPipeline.LoadTexture("checkpoint.png");
            Textures.Door = TK.ContentPipeline.LoadTexture("door.png");
            Textures.LockedDoor = TK.ContentPipeline.LoadTexture("lockedDoor.png");
            Textures.ExitDoor = TK.ContentPipeline.LoadTexture("exitDoor.png");
            Textures.Key = TK.ContentPipeline.LoadTexture("key.png");
            Textures.Star = TK.ContentPipeline.LoadTexture("star.png");
            Textures.DoubleJump = TK.ContentPipeline.LoadTexture("doubleJump.png");
            Textures.Run = TK.ContentPipeline.LoadTexture("run.png");
            Textures.Weapon = TK.ContentPipeline.LoadTexture("weapon.png");
            Textures.Weapon_Strong = TK.ContentPipeline.LoadTexture("weaponStrong.png");
            Textures.Killbox = TK.ContentPipeline.LoadTexture("killbox.png");
            Textures.Killbox_Strong = TK.ContentPipeline.LoadTexture("killboxStrong.png");
            Textures.Blank = TK.ContentPipeline.LoadTexture("pixel.png");
        }

        public static void LoadSoundEffects()
        {
            SoundEffects.Attack = TK.ContentPipeline.LoadSound("attack.wav");
            SoundEffects.Enemy_Hit = TK.ContentPipeline.LoadSound("test.wav");
            SoundEffects.Jump = TK.ContentPipeline.LoadSound("jump2.wav");
            SoundEffects.Jump_Second = TK.ContentPipeline.LoadSound("jump.wav");
            SoundEffects.Player_Hit = TK.ContentPipeline.LoadSound("test.wav");
            SoundEffects.Save = TK.ContentPipeline.LoadSound("checkpoint.wav");
            SoundEffects.Key = TK.ContentPipeline.LoadSound("key.wav");
            SoundEffects.Enemy_1 = TK.ContentPipeline.LoadSound("creature.wav");
            SoundEffects.Enemy_2 = TK.ContentPipeline.LoadSound("creature2.wav");
            SoundEffects.Pickup = TK.ContentPipeline.LoadSound("test.wav");
            SoundEffects.Respawn = TK.ContentPipeline.LoadSound("respawn.wav");
        }

        public static void LoadFonts()
        {
            Fonts.Standard = TK.ContentPipeline.LoadFont("font.png");
        }
    }
}
