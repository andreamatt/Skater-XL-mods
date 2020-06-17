using GameManagement;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AreYouSure.Patches
{
	[HarmonyPatch(typeof(GameStateMachine), "Update")]
	static class GameStateMachine_Update_Patch
	{
		public static ExitWindow window;
		static bool Prefix(GameStateMachine __instance) {
			__instance.CurrentState.OnUpdate();
			if (Input.GetKeyDown(KeyCode.Escape)) {
				window.Open();
			}
			return false;
		}
	}
}
