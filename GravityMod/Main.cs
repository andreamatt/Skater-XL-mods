using Harmony12;
using System;
using System.Reflection;
using UnityModManagerNet;
using XLShredLib;

namespace GravityMod
{
    static class Main
    {
        public static bool enabled;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;
        public static GravityMod gravityMod;
        public static string modId = "GravityMod";
        public static UnityModManager.ModEntry modEntry;

        static bool Load(UnityModManager.ModEntry modEntry) {

            Main.modEntry = modEntry;
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            gravityMod = ModMenu.Instance.gameObject.AddComponent<GravityMod>();
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Main.modEntry = modEntry;
            if (enabled == value) return true;
            enabled = value;

            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                if (gravityMod == null) gravityMod = ModMenu.Instance.gameObject.AddComponent<GravityMod>();
            }
            else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                gravityMod = null;
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<GravityMod>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
