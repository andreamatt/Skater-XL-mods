using HarmonyLib;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Effects.CameraEffects;
using XLGraphics.Utils;
using XLGraphicsUI;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.Patches
{
	[HarmonyPatch(typeof(Input), "GetKeyDown", typeof(KeyCode))]
	static class InputKeyDownPatch
	{
		static void Postfix(ref bool __result) {
			if (XLGraphicsMenu.Instance.isActiveAndEnabled && UI.Instance.IsFocusedInput()) __result = false;
		}
	}

	[HarmonyPatch(typeof(Input), "GetKeyUp", typeof(KeyCode))]
	static class InputKeyUpPatch
	{
		static void Postfix(ref bool __result) {
			if (XLGraphicsMenu.Instance.isActiveAndEnabled && UI.Instance.IsFocusedInput()) __result = false;
		}
	}

	[HarmonyPatch(typeof(ReplayCameraController), "InputCameraFOV")]
	static class ReplayCameraController_InputCameraFOV_Patch
	{
		static bool Prefix(ReplayCameraController __instance) {
			// update the value only if the slider is enabled
			if (!CustomCameraController.Instance.override_fov) {
				float num = -PlayerController.Instance.inputController.player.GetAxis("LeftStickY");
				if (Mathf.Abs(num) > 0.01) {
					// limit value between 1 and 179
					var new_fov = CustomCameraController.Instance.replay_fov + num * __instance.FOVChangeSpeed * Time.unscaledDeltaTime;
					new_fov = Mathf.Max(1, Mathf.Min(179, new_fov));

					CustomCameraController.Instance.replay_fov = new_fov;
					ReplayFovUI.Instance.fov.OverrideValue(new_fov);
				}
			}
			return false;
		}
	}
}
