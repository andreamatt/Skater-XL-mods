using Harmony12;

namespace BabboSettings.Patches {

    [HarmonyPatch(typeof(PlayerController), "get_IsSwitch")]
    static class PlayerController_IsSwitch_Patch {
        static bool Postfix(bool __result) {
            return Main.settingsGUI.isSwitch();
        }
    }

    [HarmonyPatch(typeof(PlayerController), "get_IsAnimSwitch")]
    static class PlayerController_IsAnimSwitch_Patch {
        static bool Postfix(bool __result) {
            return PlayerController.Instance.IsSwitch;
        }
    }
}