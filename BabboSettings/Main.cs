using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityModManagerNet;
using XLShredLib;

namespace BabboSettings
{
    static class Main
    {
        public static bool enabled;
        public static bool canSave = false;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;
        public static string modId = "BabboSettings";
        public static BabboSettings babboSettings;
        public static UnityModManager.ModEntry modEntry;
        public static Dictionary<string, Preset> presets = new Dictionary<string, Preset>();
        public static string map_name = "Map";
        public static string default_name = "Default";

        static bool Load(UnityModManager.ModEntry modEntry) {
            Main.modEntry = modEntry;
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            babboSettings = ModMenu.Instance.gameObject.AddComponent<BabboSettings>();
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            // load presets
            string[] filePaths = Directory.GetFiles(modEntry.Path, "*.preset.json");
            foreach (var filePath in filePaths) {
                try {
                    var json = File.ReadAllText(filePath);
                    var result = Preset.Load(json);
                    presets.Add(result.name, result);
                    if (!settings.presetOrder.Contains(result.name)) {
                        settings.presetOrder.Add(result.name);
                    }
                    Logger.Log("preset: " + result.name + " loaded");
                }
                catch (Exception e) {
                    Logger.Log($"ex: {e}");
                }
            }

            if (settings.presetOrder.ToArray().Length == 0) {
                var default_preset = new Preset(default_name);
                default_preset.Save();
                presets.Add(default_preset.name, default_preset);
                if (!settings.presetOrder.Contains(default_preset.name)) {
                    settings.presetOrder.Add(default_preset.name);
                }
            }

            foreach (var presetName in settings.presetOrder.ToArray()) {
                if (!presets.ContainsKey(presetName)) {
                    settings.presetOrder.Remove(presetName);
                }
            }
            // apply them?
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Main.modEntry = modEntry;
            if (enabled == value) return true;
            enabled = value;

            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                if (babboSettings == null) babboSettings = ModMenu.Instance.gameObject.AddComponent<BabboSettings>();
            }
            else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                babboSettings = null;
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<BabboSettings>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
        }

        public static void Save() {
            if (canSave) {
                try {
                    babboSettings.presetsManager.SaveToSettings();
                    foreach (var preset in presets.Values) {
                        preset.Save();
                    }
                    Logger.Debug("Done saving in main");
                }
                catch (Exception ex) {
                    Logger.Log("Failed saving in main, probably closed game with mod open: " + ex.Message);
                }
            }
            else {
                Logger.Log("Cannot save yet");
            }
        }
    }
}
