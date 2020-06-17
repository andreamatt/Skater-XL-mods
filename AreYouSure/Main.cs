using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace AreYouSure
{
	static class Main
	{
		public static bool enabled;
		public static Harmony harmony;
		public static string modId = "AreYouSure";
		public static AreYouSure areYouSure;
		public static UnityModManager.ModEntry modEntry;

		static bool Load(UnityModManager.ModEntry modEntry) {

			Main.modEntry = modEntry;
			modEntry.OnToggle = OnToggle;

			return true;
		}

		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
			Main.modEntry = modEntry;
			if (enabled == value) return true;
			enabled = value;

			if (enabled) {
				//harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());
				if (areYouSure == null) {
					areYouSure = new GameObject().AddComponent<AreYouSure>();
					GameObject.DontDestroyOnLoad(areYouSure.gameObject);
				}
			}
			else {
				harmony.UnpatchAll(harmony.Id);
				GameObject.Destroy(areYouSure.gameObject);
				areYouSure = null;
			}
			return true;
		}
	}
}
