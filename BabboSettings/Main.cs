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
        public static Dictionary<string, Preset> presets;
        public static string map_name = "Map";
        public static string default_name = "Default";

        static bool Load(UnityModManager.ModEntry modEntry) {
            Logger.Debug("Loading");

            Main.modEntry = modEntry;
            settings = Settings.Load();
            babboSettings = ModMenu.Instance.gameObject.AddComponent<BabboSettings>();
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            // if the replay_presetOrder is empty and the other is not, it means it's moving to the new version
            // therefore, copy presetOrder to replay_presetOrder
            if (settings.presetOrder.Count > 0 && settings.replay_presetOrder.Count == 0) {
                settings.replay_presetOrder.AddRange(settings.presetOrder);
            }

            // load presets from files
            presets = new Dictionary<string, Preset>();
            string[] filePaths = Directory.GetFiles(modEntry.Path, "*.preset.json");
            foreach (var filePath in filePaths) {
                try {
                    var json = File.ReadAllText(filePath);
                    var result = Preset.Load(json);
                    presets.Add(result.name, result);
                    if (!settings.presetOrder.Contains(result.name)) {
                        settings.presetOrder.Add(result.name);
                    }
                    if (!settings.replay_presetOrder.Contains(result.name)) {
                        settings.replay_presetOrder.Add(result.name);
                    }
                    Logger.Log("preset: " + result.name + " loaded");
                }
                catch (Exception e) {
                    Logger.Log($"ex: {e}");
                }
            }

            // if there are no presets, add the default one
            if (presets.Count == 0) {
                var default_preset = new Preset(default_name);
                default_preset.Save();
                presets.Add(default_preset.name, default_preset);
                if (!settings.presetOrder.Contains(default_preset.name)) {
                    settings.presetOrder.Add(default_preset.name);
                }
                if (!settings.replay_presetOrder.Contains(default_preset.name)) {
                    settings.replay_presetOrder.Add(default_preset.name);
                }
            }

            // remove presets that were not found
            foreach (var presetName in settings.presetOrder.ToArray()) {
                if (!presets.ContainsKey(presetName)) {
                    settings.presetOrder.Remove(presetName);
                }
            }
            foreach (var presetName in settings.replay_presetOrder.ToArray()) {
                if (!presets.ContainsKey(presetName)) {
                    settings.replay_presetOrder.Remove(presetName);
                }
            }

            Logger.Debug("Loaded");
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
            // disabled because there might be missing objects (postProcessVolume, ...) (basically it's too late to save)
            //Save();
        }

        public static void Save() {
            if (canSave) {
                try {
                    babboSettings.presetsManager.SaveToSettings();
                    settings.Save();

                    foreach (var preset in presets.Values) {
                        preset.Save();
                    }
                    Logger.Debug("Done saving in main");
                }
                catch (Exception ex) {
                    Logger.Log("Failed saving in main, probably closed game with mod open: " + ex);
                }
            }
            else {
                Logger.Log("Cannot save yet");
            }
        }
    }
}
