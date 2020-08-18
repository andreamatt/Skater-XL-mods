using GameManagement;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Presets;

namespace XLGraphics.Patches
{
	[HarmonyPatch(typeof(LevelManager), "LoadLevelRoutine", typeof(LevelInfo))]
	static class LevelManager_LoadLevelRoutine_Patch
	{
		//static bool Prefix() {
		//	Debug.Log("LoadLevelRoutine patch prefix");
		//	//PresetManager.Instance.presets.ForEach(p => p.volume.gameObject.SetActive(false));
		//	return true;
		//}

		//static void Postfix(ref IEnumerator __result) {
		//	Debug.Log("LoadLevelRoutine patch postfix");
		//}
	}


	[HarmonyPatch(typeof(GameStateMachine), "StopLoading")]
	static class GameStateMachine_StopLoading_Patch
	{
		//static bool Prefix() {
		//	Debug.Log("StopLoading patch prefix");
		//	//PresetManager.Instance.SetActives();
		//	return true;
		//}
	}
}
