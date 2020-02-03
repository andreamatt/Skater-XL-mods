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

            Main.modEntry = modEntry;
            settings = Settings.Load();
            babboSettings = ModMenu.Instance.gameObject.AddComponent<BabboSettings>();
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            // if the replay_presetOrder is empty and the other is not, it means it's moving to the new version
            // therefore, copy presetOrder to replay_presetOrder
            // also copy the enabled state
            if (settings.presetOrder.Count > 0 && settings.replay_presetOrder.Count == 0) {
                settings.replay_presetOrder.names.AddRange(settings.presetOrder.names);
                settings.replay_presetOrder.enables.AddRange(settings.presetOrder.enables);
                settings.replay_presetOrder.map_enabled = settings.presetOrder.map_enabled;
            }

            // if no Presets folder, create it and move presets there
            if (!Directory.Exists(modEntry.Path + "Presets")) {
                Directory.CreateDirectory(modEntry.Path + "Presets");
            }
            var oldPaths = Directory.GetFiles(modEntry.Path, "*.preset.json");
            foreach (var filePath in oldPaths) {
                var fileName = Path.GetFileName(filePath);
                File.Move(filePath, modEntry.Path + "\\Presets\\" + fileName);
            }

            // load presets from files
            presets = new Dictionary<string, Preset>();
            string[] filePaths = Directory.GetFiles(modEntry.Path + "Presets\\", "*.preset.json");
            foreach (var filePath in filePaths) {
                try {
                    var json = File.ReadAllText(filePath);
                    var preset = Preset.Load(json);
                    if (!preset.isMapPreset) {
                        // avoid adding map preset to presetOrder
                        presets.Add(preset.name, preset);
                        if (!settings.presetOrder.names.Contains(preset.name)) {
                            settings.presetOrder.Add(preset.name, false);
                        }
                        if (!settings.replay_presetOrder.names.Contains(preset.name)) {
                            settings.replay_presetOrder.Add(preset.name, false);
                        }
                        Logger.Log("preset: " + preset.name + " loaded");
                    }
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
                if (!settings.presetOrder.names.Contains(default_preset.name)) {
                    settings.presetOrder.Add(default_preset.name, false);
                }
                if (!settings.replay_presetOrder.names.Contains(default_preset.name)) {
                    settings.replay_presetOrder.Add(default_preset.name, false);
                }
            }

            // remove presets that were not found
            foreach (var name in settings.presetOrder.names) {
                if (!presets.ContainsKey(name)) {
                    settings.presetOrder.Remove(name);
                }
            }
            foreach (var name in settings.replay_presetOrder.names) {
                if (!presets.ContainsKey(name)) {
                    settings.replay_presetOrder.Remove(name);
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
