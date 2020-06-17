using HarmonyLib;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabboSettings.Patches
{
	[HarmonyPatch(typeof(FreeCameraKeyFrame), "Evaluate")]
	static class FreeCameraKeyFrame_Evaluate_Patch
	{
		static CameraCurveResult Postfix(CameraCurveResult __result) {
			PatchData.Instance.UpdateReplayFov(__result.fov);
			return __result;
		}
	}

	[HarmonyPatch(typeof(OrbitCameraKeyFrame), "Evaluate")]
	static class OrbitCameraKeyFrame_Evaluate_Patch
	{
		static CameraCurveResult Postfix(CameraCurveResult __result) {
			PatchData.Instance.UpdateReplayFov(__result.fov);
			return __result;
		}
	}

	[HarmonyPatch(typeof(TripodCameraKeyFrame), "Evaluate")]
	static class TripodCameraKeyFrame_Evaluate_Patch
	{
		static CameraCurveResult Postfix(CameraCurveResult __result) {
			PatchData.Instance.UpdateReplayFov(__result.fov);
			return __result;
		}
	}
}
