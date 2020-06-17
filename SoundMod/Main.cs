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
		public static float MusicVolume;

		static bool Load(UnityModManager.ModEntry modEntry) {

			Main.modEntry = modEntry;
			settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			modEntry.OnSaveGUI = OnSaveGUI;
			modEntry.OnToggle = OnToggle;
			modEntry.OnGUI = OnSettingsGUI;
			MusicVolume = PlayerPrefs.GetFloat("Volume_Music", 0f);
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

		private static void OnSettingsGUI(UnityModManager.ModEntry obj) {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Titlescreen Volume");
			GUILayout.FlexibleSpace();
			float newVolume = MusicVolume;
			if (float.TryParse(GUILayout.TextField(MusicVolume.ToString("0.00"), GUILayout.Width(50)), out float value)) {
				newVolume = value;
			}
			newVolume = GUILayout.HorizontalSlider(MusicVolume, 0f, 1f, GUILayout.MinWidth(600f));
			if (newVolume != MusicVolume) {
				MusicVolume = newVolume;
				PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
			}
			GUILayout.EndHorizontal();
		}


		static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
			settings.Save(modEntry);
			PlayerPrefs.SetFloat("Volume_Music", MusicVolume);
		}
	}
}
