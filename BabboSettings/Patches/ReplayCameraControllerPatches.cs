using Harmony12;
using ReplayEditor;
using UnityEngine;

namespace BabboSettings.Patches
{

    [HarmonyPatch(typeof(ReplayCameraController), "InputCameraFOV")]
    static class ReplayCameraController_InputCameraFOV_Patch
    {
        private static CustomCameraController _CustomCameraController;
        private static CustomCameraController CustomCameraController {
            get {
                if (_CustomCameraController != null) return _CustomCameraController;
                return _CustomCameraController = BabboSettings.Instance.gameObject.GetComponent<CustomCameraController>();
            }
        }

        static bool Prefix(ReplayCameraController __instance) {
            // update the value only if the slider is enabled
            if (CustomCameraController.override_fov == false) {
                float num = -PlayerController.Instance.inputController.player.GetAxis("LeftStickY");
                if (Mathf.Abs(num) > 0.01) {
                    // limit value between 1 and 179
                    var new_fov = CustomCameraController.replay_fov + num * __instance.FOVChangeSpeed * Time.unscaledDeltaTime;
                    CustomCameraController.replay_fov = Mathf.Max(1, Mathf.Min(179, new_fov));
                }
            }
            return false;
        }
    }
}