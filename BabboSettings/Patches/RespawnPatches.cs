using Harmony12;

namespace BabboSettings.Patches {

    [HarmonyPatch(typeof(Respawn), "DoRespawn")]
    static class Respawn_DoRespawn_Patch {
        static bool Prefix() {
            Main.settingsGUI.SetJustRespawned();
            return true;
        }
    }

    [HarmonyPatch(typeof(Respawn), "SetSpawnPos")]
    static class Respawn_SetSpawnPos_Patch {
        static bool Prefix() {
            Main.settingsGUI.SetSpawnSwitch();
            return true;
        }
    }

}