using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Utils;
using XLGraphicsUI;

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
}
