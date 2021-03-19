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
	[EnableReloading]
	public static class Main
	{
		public static bool enabled;
		public static Harmony harmony;
		public static XLGraphics xlGraphics;
		public static XLGraphicsMenu menu;
		public static AssetBundle uiBundle;
		public static GameObject presetObjectAsset;
		public static GameObject imgNameObjectAsset;
		public static GameObject overlayImageObjectAsset;
		public static GameObject menuObjectAsset;
		public static UnityModManager.ModEntry modEntry;
		public static Settings settings;

		static bool Load(UnityModManager.ModEntry modEntry) {
			Main.modEntry = modEntry;
			Main.Logger.Log("Load");
			modEntry.OnToggle = OnToggle;
			modEntry.OnUnload = Unload;
			//modEntry.OnSaveGUI = OnSave;

			// load assets
			using (var stream = new FileStream(modEntry.Path + "graphicsmenuassetbundle", FileMode.Open, FileAccess.Read)) {
				uiBundle = AssetBundle.LoadFromStream(stream);

				menuObjectAsset = uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Menu.prefab");
				presetObjectAsset = uiBundle.LoadAsset<GameObject>("Assets/Prefabs/PresetObject.prefab");
				imgNameObjectAsset = uiBundle.LoadAsset<GameObject>("Assets/Prefabs/ImgNameToggle.prefab");
				overlayImageObjectAsset = uiBundle.LoadAsset<GameObject>("Assets/Prefabs/OverlayImage.prefab");

				uiBundle.Unload(false); // free the file stream without deleting objectAssets
			}

			return true;
		}

		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
			Main.Logger.Log("OnToggle " + value.ToString());
			if (enabled == value) {
				return true;
			}

			enabled = value;

			if (value) {
				// disable if xlshredmenu is detected
				var mod = UnityModManager.FindMod("blendermf.XLShredMenu");
				if (mod != null) {
					modEntry.CustomRequirements = $"Mod {mod.Info.DisplayName} incompatible";
					enabled = false;
					return false;
				}

				// create harmony instance
				harmony = new Harmony(modEntry.Info.Id);

				// patch
				harmony.PatchAll(Assembly.GetExecutingAssembly());

				// instantiate menu and XLGraphics monobehaviour
				GameObject menuGO = GameObject.Instantiate(menuObjectAsset);
				menu = XLGraphicsMenu.Instance;
				xlGraphics = new GameObject().AddComponent<XLGraphics>();
				GameObject.DontDestroyOnLoad(menuGO);
				GameObject.DontDestroyOnLoad(xlGraphics.gameObject);
			}
			else {
				// unpatch
				harmony.UnpatchAll(harmony.Id);

				// destroy menu and XLGraphics monobehaviour
				GameObject.DestroyImmediate(menu.gameObject);
				GameObject.DestroyImmediate(xlGraphics.gameObject);
			}
			Main.Logger.Log("Loaded");
			return true;
		}

		static void OnSave(UnityModManager.ModEntry modEntry) {
			Save();
		}

		public static void Save() {
			settings.Save();
		}

		static bool Unload(UnityModManager.ModEntry modEntry) {
			Main.Logger.Log("Unload");

			//uiBundle.Unload(true); // no need to unload: assets will simply be replaced on the new load

			return true;
		}

		public static UnityModManager.ModEntry.ModLogger Logger => modEntry.Logger;
	}
}
