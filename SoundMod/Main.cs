using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace SoundMod
{
	static class Main
	{
		public static bool enabled;
		public static Settings settings;
		public static Harmony harmony;
		public static SoundMod soundMod;
		public static string modId = "SoundMod";
		public static UnityModManager.ModEntry modEntry;

		static bool Load(UnityModManager.ModEntry modEntry) {

			Main.modEntry = modEntry;
			settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			modEntry.OnToggle = OnToggle;
			return true;
		}

		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
			Main.modEntry = modEntry;
			if (enabled == value) return true;
			enabled = value;

			if (enabled) {
				// disable if xlshredmenu is detected
				var mod = UnityModManager.FindMod("blendermf.XLShredMenu");
				if (mod != null) {
					modEntry.CustomRequirements = $"Mod {mod.Info.DisplayName} incompatible";
					enabled = false;
					return false;
				}

				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());
				soundMod = new GameObject().AddComponent<SoundMod>();
				GameObject.DontDestroyOnLoad(soundMod.gameObject);
			}
			else {
				harmony.UnpatchAll(harmony.Id);
				GameObject.Destroy(soundMod.gameObject);
				soundMod = null;
			}
			return true;
		}
	}
}
