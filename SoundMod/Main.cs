using Harmony12;
using System;
using System.Reflection;
using UnityModManagerNet;
using XLShredLib;

namespace SoundMod
{
    static class Main
    {
        public static bool enabled;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;
        public static SoundMod soundMod;
        public static string modId = "SoundMod";
        public static UnityModManager.ModEntry modEntry;

        static bool Load(UnityModManager.ModEntry modEntry) {

            Main.modEntry = modEntry;
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
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
                if (soundMod == null) soundMod = ModMenu.Instance.gameObject.AddComponent<SoundMod>();
            }
            else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                soundMod = null;
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<SoundMod>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
