using Harmony12;
using System;
using UnityEngine;

namespace BabboSettings.Patches
{

    [HarmonyPatch(typeof(Respawn), "DoRespawn")]
    static class Respawn_DoRespawn_Patch
    {
        static bool Prefix() {
            PatchData.Instance.SetJustRespawned();
            return true;
        }
    }

    [HarmonyPatch(typeof(Respawn), "SetSpawnPos", new Type[] { })]
    static class Respawn_SetSpawnPos_Patch
    {
        static bool Prefix() {
            PatchData.Instance.SetSpawnPos();
            return true;
        }
    }
}