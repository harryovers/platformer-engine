using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.Audio;

namespace Platform.Core
{
    public static class SoundEffects
    {
        public static SoundEffect Jump { get; set; }
        public static SoundEffect Jump_Second { get; set; }
        public static SoundEffect Attack { get; set; }
        public static SoundEffect Save { get; set; }
        public static SoundEffect Player_Hit { get; set; }
        public static SoundEffect Enemy_Hit { get; set; }
        public static SoundEffect Enemy_1 { get; set; }
        public static SoundEffect Enemy_2 { get; set; }
        public static SoundEffect Key { get; set; }
        public static SoundEffect Pickup { get; set; }
        public static SoundEffect Respawn { get; set; }
    }
}
