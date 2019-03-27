using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityModManagerNet;

namespace BabboSettings {

	[Serializable]
	public class Settings : UnityModManager.ModSettings {

		public string presetName = Main.default_name;

		public Settings() {
		}

		public override void Save(UnityModManager.ModEntry modEntry) {
			Save(this, modEntry);
		}

		internal void Save() {
			Save(this, Main.modEntry);
		}
	}

	[Serializable]
	public class Preset {
		public string name = "no_name";

		public bool ENABLE_POST = true;
		public float FOV = 60;
		public bool FOCUS_PLAYER = true;

		public PostProcessLayer.Antialiasing AA_MODE = new PostProcessLayer.Antialiasing();
		public float TAA_sharpness = new TemporalAntialiasing().sharpness;
		public float TAA_jitter = new TemporalAntialiasing().jitterSpread;
		public float TAA_stationary = new TemporalAntialiasing().stationaryBlending;
		public float TAA_motion = new TemporalAntialiasing().motionBlending;
		public SubpixelMorphologicalAntialiasing SMAA = new SubpixelMorphologicalAntialiasing();

		public AmbientOcclusion AO = new AmbientOcclusion();
		public AutoExposure EXPO = new AutoExposure();
		public Bloom BLOOM = new Bloom();
		public ChromaticAberration CA = new ChromaticAberration();
		public bool COLOR_enabled = false;
		public float COLOR_temperature = 0;
		public float COLOR_post_exposure = 0;
		public float COLOR_saturation = 0;
		public float COLOR_contrast = 0;
		public DepthOfField DOF = new DepthOfField();
		public Grain GRAIN = new Grain();
		public LensDistortion LENS = new LensDistortion();
		public MotionBlur BLUR = new MotionBlur();
		public ScreenSpaceReflections REFL = new ScreenSpaceReflections();
		public Vignette VIGN = new Vignette();

		public Preset() {
		}

		public Preset(string name) {
			this.name = name;
		}

		public void Save() {
			var filepath = Main.modEntry.Path;
			try {
				var writer = new StreamWriter($"{filepath}{name}.preset.xml");
				var serializer = new XmlSerializer(typeof(Preset));
				serializer.Serialize(writer, this);
				writer.Close();
			}
			catch (Exception e) {
				Main.log($"Can't save {filepath}. ex: {e.Message}");
			}
		}
	}

	static class Main {
		public static bool enabled;
		public static Settings settings;
		public static HarmonyInstance harmonyInstance;
		public static string modId = "BabboSettings";
		public static SettingsGUI settingsGUI = new GameObject().AddComponent<SettingsGUI>();
		public static UnityModManager.ModEntry modEntry;
		public static Dictionary<string, Preset> presets = new Dictionary<string, Preset>();
		public static Preset selectedPreset;
		public static string map_name = "Map";
		public static string default_name = "Default";

		static bool Load(UnityModManager.ModEntry modEntry) {
			Main.modEntry = modEntry;
			settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			modEntry.OnSaveGUI = OnSaveGUI;
			modEntry.OnToggle = OnToggle;

			// load presets
			var serializer = new XmlSerializer(typeof(Preset));
			string[] filePaths = Directory.GetFiles(modEntry.Path, "*.preset.xml");
			foreach (var filePath in filePaths) {
				try {
					var stream = File.OpenRead(filePath);
					var result = (Preset)serializer.Deserialize(stream);
					presets.Add(result.name, result);
					stream.Close();
					log("preset: " + result.name + " loaded");
				}
				catch (Exception e) {
					log($"ex: {e}");
				}
			}
			try {
				select(settings.presetName);
			}
			catch (Exception e) {
				log("Failed Main.Load: " + e);
			}

			return true;
		}

		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
			Main.modEntry = modEntry;
			if (enabled == value) return true;
			enabled = value;

			if (enabled) {
				harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
				harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
			}
			else {
				Save();
				harmonyInstance.UnpatchAll(harmonyInstance.Id);
			}
			return true;
		}

		internal static void select(string s) {
			if (presets.ContainsKey(s)) {
				selectedPreset = presets[s];
				settings.presetName = s;
				log("Switched to preset " + s);
			}
			else {
				log("Preset " + s + " not found");
				if (presets.ContainsKey(default_name)) {
					selectedPreset = presets[default_name];
					settings.presetName = default_name;
				}
				else {
					try {
						var defaultP = new Preset(default_name);
						var writer = new StreamWriter($"{modEntry.Path}{default_name}.preset.xml");
						var serializer = new XmlSerializer(typeof(Preset));
						serializer.Serialize(writer, defaultP);
						selectedPreset = defaultP;
						settings.presetName = defaultP.name;
						presets.Add(defaultP.name, defaultP);
					}
					catch (Exception e) {
						log("Error creating " + default_name + " preset: " + e);
					}
				}
			}
			settings.Save();
		}

		static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
			Save();
		}

		internal static void Save() {
			try {
				settings.Save();
				settingsGUI.SaveTo(selectedPreset);
				log("Done saving in main");
			}
			catch (Exception ex) {
				log("Failed saving in main, probably closed game with mod open: " + ex.Message);
			}
		}

		internal static void log(string s) {
			UnityModManager.Logger.Log(s);
		}
	}
}
