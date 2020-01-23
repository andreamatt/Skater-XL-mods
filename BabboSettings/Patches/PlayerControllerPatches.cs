using Harmony12;

namespace BabboSettings.Patches
{

    [HarmonyPatch(typeof(PlayerController), "get_IsSwitch")]
    static class PlayerController_IsSwitch_Patch
    {
        static bool Postfix(bool __result) {
            var cameraMode = PatchData.Instance.cameraMode;
            var isSwitch = PatchData.Instance.isSwitch();
            if (PatchData.Instance.overrideCM || (cameraMode == CameraMode.POV || cameraMode == CameraMode.Skate)) {
                return isSwitch;
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(PlayerController), "get_IsAnimSwitch")]
    static class PlayerController_IsAnimSwitch_Patch
    {
        static bool Postfix(bool __result) {
            var cameraMode = PatchData.Instance.cameraMode;
            var isSwitch = PatchData.Instance.isSwitch();
            if (PatchData.Instance.overrideCM || (cameraMode == CameraMode.POV || cameraMode == CameraMode.Skate)) {
                return isSwitch;
            }
            return __result;
        }
    }
}