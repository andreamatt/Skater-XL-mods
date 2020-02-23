using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using XLShredLib;

namespace MapBrowser
{
    static class Main
    {
        public static bool enabled;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;
        public static string modId = "MapBrowser";
        public static MapBrowser mapBrowser;
        public static UnityModManager.ModEntry modEntry;
        public static string ImageFolder => modEntry.Path + "Images\\";


        static bool Load(UnityModManager.ModEntry modEntry) {

            Main.modEntry = modEntry;
            settings = Settings.Load();
            mapBrowser = ModMenu.Instance.gameObject.AddComponent<MapBrowser>();
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
                if (mapBrowser == null) mapBrowser = ModMenu.Instance.gameObject.AddComponent<MapBrowser>();
            }
            else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                mapBrowser = null;
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<MapBrowser>());
            }
            return true;
        }

        public static void Save() {
            try {
                settings.Save();
            }
            catch (Exception ex) {
                Logger.Log("Failed saving in main, probably closed game with mod open: " + ex);
            }
        }
    }
}
