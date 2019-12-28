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

    [HarmonyPatch(typeof(Respawn), "EndRespawning")]
    static class Respawn_EndRespawning_Patch
    {
        static void Postfix() {
            PatchData.Instance.RotateSpawnCamera();
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

    [HarmonyPatch(typeof(Respawn), "SetSpawnPos", new Type[] { typeof(Vector3), typeof(Quaternion) })]
    static class Respawn_SetSpawnPos_Patch_Vector_Quaternion
    {
        static bool Prefix() {
            PatchData.Instance.SetSpawnPos();
            return true;
        }
    }
}