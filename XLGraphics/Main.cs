using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using XLGraphicsUI;

namespace XLGraphics
{
	public static class Main
	{
		public static bool enabled;
		public static Harmony harmony;
		public static string modId = "UItest";
		public static XLGraphics xlGraphics;
		public static XLGraphicsMenu menu;
		public static AssetBundle uiBundle;
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
				// disable if xlshredmenu is detected
				var mod = UnityModManager.FindMod("blendermf.XLShredMenu");
				if (mod != null) {
					modEntry.CustomRequirements = $"Mod {mod.Info.DisplayName} incompatible";
					enabled = false;
					return false;
				}
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());
				if (xlGraphics == null) {
					xlGraphics = new GameObject().AddComponent<XLGraphics>();
					GameObject.DontDestroyOnLoad(xlGraphics.gameObject);
				}

				if (XLGraphicsMenu.Instance == null) {
					if (uiBundle == null) uiBundle = AssetBundle.LoadFromFile(modEntry.Path + "graphicsmenuassetbundle");

					GameObject newMenuObject = GameObject.Instantiate(uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Menu.prefab"));

					XLGraphicsMenu.Instance.BTNclick = s => {
						Logger.Log("Button clicked: " + s);
					};

					GameObject.DontDestroyOnLoad(newMenuObject);

					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
			}
			else {
				harmony.UnpatchAll(harmony.Id);
				GameObject.Destroy(xlGraphics.gameObject);
				xlGraphics = null;
			}
			return true;
		}
	}
}
