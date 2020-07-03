using HarmonyLib;

namespace BabboSettings.Patches
{

	[HarmonyPatch(typeof(PlayerController), "get_IsSwitch")]
	static class PlayerController_IsSwitch_Patch
	{
		static bool Postfix(bool __result) {
			var cameraMode = PatchData.Instance.cameraMode;
			// isSwitch needs to be calculated all the time, otherwise it loses track of the stance
			var isSwitch = PatchData.Instance.isSwitch();
			if (cameraMode == CameraMode.POV || cameraMode == CameraMode.Skate) {
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
			// isSwitch needs to be calculated all the time, otherwise it loses track of the stance
			var isSwitch = PatchData.Instance.isSwitch();
			if (cameraMode == CameraMode.POV || cameraMode == CameraMode.Skate) {
				return isSwitch;
			}
			return __result;
		}
	}
}
