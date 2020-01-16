using Harmony12;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SoundMod.Patches
{

    [HarmonyPatch(typeof(DeckSounds), "Start")]
    static class Start_Patch
    {
        static bool Prefix() {
            Logger.Log("Start");
            SoundMod.Start();
            return true;
        }
    }
}
