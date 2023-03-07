using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace XLSoundMod
{
	public class Main
	{
		public static Settings Settings;
		public static UnityModManager.ModEntry modEntry;

		private static GameObject XLSoundModGO;
		public static SoundMod SoundMod;

		public static bool enabled;

		private static bool Load(UnityModManager.ModEntry modEntry)
		{
			Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(OnSaveGUI);
			modEntry.OnToggle = new Func<UnityModManager.ModEntry, bool, bool>(OnToggle);
			Main.modEntry = modEntry;
			return true;
		}

		private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
		{
			Settings.Save(modEntry);
		}

		private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
		{
			if (value == enabled)
			{
				return true;
			}

			enabled = value;

			if (enabled)
			{
				XLSoundModGO = new GameObject("XLSoundMod");
				SoundMod = XLSoundModGO.AddComponent<SoundMod>();

				UnityEngine.Object.DontDestroyOnLoad(XLSoundModGO);
			}
			else
			{
				UnityEngine.Object.Destroy(XLSoundModGO);
			}
			return true;
		}

		public static void Log(object message)
		{
			UnityModManager.Logger.Log(message.ToString());
		}
	}
}