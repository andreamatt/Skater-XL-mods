using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMod.Patches
{
    [HarmonyPatch(typeof(MusicController), "PlayTitleTrack")]
    static class PlayTitleTrack_Patch
    {
        static bool Prefix() {
            // just dont play it
            return false;
        }
    }
}
