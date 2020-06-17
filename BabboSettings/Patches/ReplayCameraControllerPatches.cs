using HarmonyLib;
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

	[HarmonyPatch(typeof(ReplayCameraController), "InputFreeRotation")]
	static class ReplayCameraController_InputFreeRotation_Patch
	{
		static bool Prefix(ReplayCameraController __instance) {
			// if not presset, skip
			PatchData.Instance.FreeInput(__instance);

			return true;
		}
	}
}