using HarmonyLib;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.EffectHandlers.CameraEffects;
using XLGraphics.CustomEffects;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.Patches
{
	[HarmonyPatch(typeof(FreeCameraKeyFrame), "Evaluate")]
	static class FreeCameraKeyFrame_Evaluate_Patch
	{
		static CameraCurveResult Postfix(CameraCurveResult __result) {
			CustomCameraController.Instance.replay_fov = __result.fov;
			ReplayFovUI.Instance.fov.OverrideValue(__result.fov);
			return __result;
		}
	}

	[HarmonyPatch(typeof(OrbitCameraKeyFrame), "Evaluate")]
	static class OrbitCameraKeyFrame_Evaluate_Patch
	{
		static CameraCurveResult Postfix(CameraCurveResult __result) {
			CustomCameraController.Instance.replay_fov = __result.fov;
			ReplayFovUI.Instance.fov.OverrideValue(__result.fov);
			return __result;
		}
	}

	[HarmonyPatch(typeof(TripodCameraKeyFrame), "Evaluate")]
	static class TripodCameraKeyFrame_Evaluate_Patch
	{
		static CameraCurveResult Postfix(CameraCurveResult __result) {
			CustomCameraController.Instance.replay_fov = __result.fov;
			ReplayFovUI.Instance.fov.OverrideValue(__result.fov);
			return __result;
		}
	}
}
