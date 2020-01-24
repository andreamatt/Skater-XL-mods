using Harmony12;
using ReplayEditor;
using UnityEngine;

namespace BabboSettings.Patches
{

    [HarmonyPatch(typeof(ReplayCameraController), "InputCameraFOV")]
    static class ReplayCameraController_InputCameraFOV_Patch
    {
        static bool Prefix(ReplayCameraController __instance) {
            // update the value only if the slider is enabled
            if (PatchData.Instance.cameraController.override_fov == false) {
                float num = -PlayerController.Instance.inputController.player.GetAxis("LeftStickY");
                if (Mathf.Abs(num) > 0.01) {
                    // limit value between 1 and 179
                    var new_fov = PatchData.Instance.cameraController.replay_fov + num * __instance.FOVChangeSpeed * Time.unscaledDeltaTime;
                    new_fov = Mathf.Max(1, Mathf.Min(179, new_fov));

                    PatchData.Instance.UpdateReplayFov(new_fov);
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ReplayCameraController), "InputFreePosition")]
    static class ReplayCameraController_InputFreePosition_Patch
    {
        static bool Prefix(ReplayCameraController __instance) {
            // if not presset, skip
            if (!Input.GetMouseButton(2)) return true;



            return false;
        }
    }

    [HarmonyPatch(typeof(ReplayCameraController), "InputFreeRotation")]
    static class ReplayCameraController_InputFreeRotation_Patch
    {
        static bool Prefix(ReplayCameraController __instance) {
            // if not presset, skip
            if (!Input.GetMouseButton(2)) return true;

            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            var cameraTransform = PatchData.Instance.cameraController.mainCamera.transform;
            cameraTransform.transform.rotation = Quaternion.AngleAxis(y * __instance.RotateSpeed * Time.unscaledDeltaTime, Vector3.ProjectOnPlane(-cameraTransform.right, Vector3.up)) * cameraTransform.rotation;
            cameraTransform.transform.rotation = Quaternion.AngleAxis(x * __instance.RotateSpeed * Time.unscaledDeltaTime, Vector3.up) * cameraTransform.rotation;
            return false;
        }
    }
}