using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using XLShredLib;

namespace AreYouSure
{
    static class Main
    {
        public static bool enabled;
        public static HarmonyInstance harmonyInstance;
        public static string modId = "AreYouSure";
        public static AreYouSure areYouSure;
        public static UnityModManager.ModEntry modEntry;

        static bool Load(UnityModManager.ModEntry modEntry) {

            Main.modEntry = modEntry;
            areYouSure = ModMenu.Instance.gameObject.AddComponent<AreYouSure>();
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
                if (areYouSure == null) areYouSure = ModMenu.Instance.gameObject.AddComponent<AreYouSure>();
            }
            else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                areYouSure = null;
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<AreYouSure>());
            }
            return true;
        }
    }
}
