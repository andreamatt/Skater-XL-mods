using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using XLGraphicsUI;

namespace XLGraphics
{
#if DEBUG
	[EnableReloading]
#endif
	public static class Main
	{
		public static bool enabled;
		public static Harmony harmony;
		public static string modId = "UItest";
		public static XLGraphics xlGraphics;
		public static XLGraphicsMenu menu;
		public static AssetBundle uiBundle;
		public static GameObject presetObjectAsset;
		public static UnityModManager.ModEntry modEntry;
		public static Settings settings;

		static bool Load(UnityModManager.ModEntry modEntry) {

			Main.modEntry = modEntry;
			modEntry.OnToggle = OnToggle;
#if DEBUG
			modEntry.OnUnload = Unload;
#endif
			//modEntry.OnSaveGUI = OnSave;

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

				// create harmony instance
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());

				// load menu asset
				uiBundle = AssetBundle.LoadFromFile(modEntry.Path + "graphicsmenuassetbundle");
				GameObject newMenuObject = GameObject.Instantiate(uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Menu.prefab"));

				presetObjectAsset = uiBundle.LoadAsset<GameObject>("Assets/Prefabs/PresetObject.prefab");

				GameObject.DontDestroyOnLoad(newMenuObject);
				menu = XLGraphicsMenu.Instance;

				xlGraphics = new GameObject().AddComponent<XLGraphics>();
				GameObject.DontDestroyOnLoad(xlGraphics.gameObject);
			}
			else {
				harmony.UnpatchAll(harmony.Id);
				GameObject.DestroyImmediate(menu.gameObject);
				GameObject.DestroyImmediate(xlGraphics.gameObject);
				uiBundle.Unload(true);
				uiBundle = null;
				xlGraphics = null;
			}
			Logger.Log("Loaded");
			return true;
		}

		static void OnSave(UnityModManager.ModEntry modEntry) {
			Save();
		}

		public static void Save() {
			settings.Save();
		}
#if DEBUG
		static bool Unload(UnityModManager.ModEntry modEntry) {
			//harmony.UnpatchAll(harmony.Id);

			return true;
		}
#endif
	}
}
