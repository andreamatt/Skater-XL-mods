using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityModManagerNet;
using XLShredLib;

namespace BabboSettings {

	[Serializable]
	public class Settings : UnityModManager.ModSettings {

		public List<string> presetOrder = new List<string>();
		public bool DEBUG = true;

		// Basic
		public bool ENABLE_POST = true;
		public int VSYNC = 1;
		public int SCREEN_MODE = 0;

		// AA
		public PostProcessLayer.Antialiasing AA_MODE = new PostProcessLayer.Antialiasing();
		public float TAA_sharpness = new TemporalAntialiasing().sharpness;
		public float TAA_jitter = new TemporalAntialiasing().jitterSpread;
		public float TAA_stationary = new TemporalAntialiasing().stationaryBlending;
		public float TAA_motion = new TemporalAntialiasing().motionBlending;
		public SubpixelMorphologicalAntialiasing SMAA = new SubpixelMorphologicalAntialiasing();

		// Camera
		public CameraMode CAMERA = CameraMode.Normal;
		public float NORMAL_FOV = 60;
		public float NORMAL_REACT = 0.90f;
		public float NORMAL_REACT_ROT = 0.90f;
		public float NORMAL_CLIP = 0.01f;
		public float FOLLOW_FOV = 60;
		public float FOLLOW_REACT = 0.70f;
		public float FOLLOW_REACT_ROT = 0.70f;
		public float FOLLOW_CLIP = 0.01f;
		public Vector3 FOLLOW_SHIFT = Vector3.zero;
		public float POV_FOV = 60;
		public float POV_REACT = 1;
		public float POV_REACT_ROT = 0.07f;
		public float POV_CLIP = 0.01f;
		public bool HIDE_HEAD = true;
		public Vector3 POV_SHIFT = Vector3.zero;
		public float SKATE_FOV = 60;
		public float SKATE_REACT = 0.90f;
		public float SKATE_REACT_ROT = 0.90f;
		public float SKATE_CLIP = 0.01f;
		public Vector3 SKATE_SHIFT = Vector3.zero;

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
		public bool enabled = true;

		// Effects
		public AmbientOcclusion AO = new AmbientOcclusion();
		public AutoExposure EXPO = new AutoExposure();
		public Bloom BLOOM = new Bloom();
		public ChromaticAberration CA = new ChromaticAberration();
		public bool COLOR_enabled = false;
		public Tonemapper COLOR_tonemapper = Tonemapper.Neutral;
		public float COLOR_temperature = 0;
		public float COLOR_tint = 0;
		public float COLOR_postExposure = 0;
		public float COLOR_hueShift = 0;
		public float COLOR_saturation = 0;
		public float COLOR_contrast = 0;
		public Vector4 COLOR_lift = new Vector4(1, 1, 1, 0);
		public Vector4 COLOR_gamma = new Vector4(1, 1, 1, 0);
		public Vector4 COLOR_gain = new Vector4(1, 1, 1, 0);
		public DepthOfField DOF = new DepthOfField();
		public FocusMode FOCUS_MODE = FocusMode.Custom;
		public Grain GRAIN = new Grain();
		public LensDistortion LENS = new LensDistortion();
		public MotionBlur BLUR = new MotionBlur();
		public ScreenSpaceReflections REFL = new ScreenSpaceReflections();
		public Vignette VIGN = new Vignette();

		public Preset() {
			DisableAll();
		}

		public Preset(string name) {
			DisableAll();
			this.name = name;
		}

		public void DisableAll() {
			AO.enabled.Override(false);
			EXPO.enabled.Override(false);
			BLOOM.enabled.Override(false);
			CA.enabled.Override(false);
			COLOR_enabled = false;
			DOF.enabled.Override(false);
			GRAIN.enabled.Override(false);
			LENS.enabled.Override(false);
			BLUR.enabled.Override(false);
			REFL.enabled.Override(false);
			VIGN.enabled.Override(false);
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
		public static bool canSave = false;
		public static Settings settings;
		public static HarmonyInstance harmonyInstance;
		public static string modId = "BabboSettings";
		public static SettingsGUI settingsGUI;
		public static UnityModManager.ModEntry modEntry;
		public static Dictionary<string, Preset> presets = new Dictionary<string, Preset>();
		public static string map_name = "Map";
		public static string default_name = "Default";

		static bool Load(UnityModManager.ModEntry modEntry) {
			Main.modEntry = modEntry;
			settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			settingsGUI = ModMenu.Instance.gameObject.AddComponent<SettingsGUI>();
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
					if (!settings.presetOrder.Contains(result.name)) {
						settings.presetOrder.Add(result.name);
					}
					stream.Close();
					log("preset: " + result.name + " loaded");
				}
				catch (Exception e) {
					log($"ex: {e}");
				}
			}
			foreach(var presetName in settings.presetOrder.ToArray()) {
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
				if (settingsGUI == null) settingsGUI = ModMenu.Instance.gameObject.AddComponent<SettingsGUI>();
			}
			else {
				harmonyInstance.UnpatchAll(harmonyInstance.Id);
				settingsGUI = null;
				UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<SettingsGUI>());
			}
			return true;
		}

		static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
		}

		internal static void Save() {
			if (canSave) {
				try {
					settingsGUI.SaveToSettings();
					foreach(var preset in presets.Values) {
						preset.Save();
					}
					if (settings.DEBUG) log("Done saving in main");
				}
				catch (Exception ex) {
					log("Failed saving in main, probably closed game with mod open: " + ex.Message);
				}
			}
			else {
				log("Cannot save yet");
			}
		}

		internal static void log(string s) {
			UnityModManager.Logger.Log("[BabboSettings] " + s);
		}
	}
}
