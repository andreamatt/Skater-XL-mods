using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
		public string[] serialized_effects = new string[11];

		public FocusMode FOCUS_MODE = FocusMode.Custom;

		// Effects
		[NonSerialized]
		public AmbientOcclusion AO = ScriptableObject.CreateInstance<AmbientOcclusion>();
		[NonSerialized]
		public AutoExposure EXPO = ScriptableObject.CreateInstance<AutoExposure>();
		[NonSerialized]
		public Bloom BLOOM = ScriptableObject.CreateInstance<Bloom>();
		[NonSerialized]
		public ChromaticAberration CA = ScriptableObject.CreateInstance<ChromaticAberration>();
		[NonSerialized]
		public ColorGrading COLOR = ScriptableObject.CreateInstance<ColorGrading>();
		[NonSerialized]
		public DepthOfField DOF = ScriptableObject.CreateInstance<DepthOfField>();
		[NonSerialized]
		public Grain GRAIN = ScriptableObject.CreateInstance<Grain>();
		[NonSerialized]
		public LensDistortion LENS = ScriptableObject.CreateInstance<LensDistortion>();
		[NonSerialized]
		public MotionBlur BLUR = ScriptableObject.CreateInstance<MotionBlur>();
		[NonSerialized]
		public ScreenSpaceReflections REFL = ScriptableObject.CreateInstance<ScreenSpaceReflections>();
		[NonSerialized]
		public Vignette VIGN = ScriptableObject.CreateInstance<Vignette>();

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
			COLOR.enabled.Override(false);
			DOF.enabled.Override(false);
			GRAIN.enabled.Override(false);
			LENS.enabled.Override(false);
			BLUR.enabled.Override(false);
			REFL.enabled.Override(false);
			VIGN.enabled.Override(false);
		}

		internal void Serialize() {
			serialized_effects[0] = JsonUtility.ToJson(AO, true);
			serialized_effects[1] = JsonUtility.ToJson(EXPO, true);
			serialized_effects[2] = JsonUtility.ToJson(BLOOM, true);
			serialized_effects[3] = JsonUtility.ToJson(CA, true);
			serialized_effects[4] = JsonUtility.ToJson(COLOR, true);
			serialized_effects[5] = JsonUtility.ToJson(DOF, true);
			serialized_effects[6] = JsonUtility.ToJson(GRAIN, true);
			serialized_effects[7] = JsonUtility.ToJson(LENS, true);
			serialized_effects[8] = JsonUtility.ToJson(BLUR, true);
			serialized_effects[9] = JsonUtility.ToJson(REFL, true);
			serialized_effects[10] = JsonUtility.ToJson(VIGN, true);
		}

		internal void Deserialize() {
			JsonUtility.FromJsonOverwrite(serialized_effects[0], AO);
			JsonUtility.FromJsonOverwrite(serialized_effects[1], EXPO);
			JsonUtility.FromJsonOverwrite(serialized_effects[2], BLOOM);
			JsonUtility.FromJsonOverwrite(serialized_effects[3], CA);
			JsonUtility.FromJsonOverwrite(serialized_effects[4], COLOR);
			JsonUtility.FromJsonOverwrite(serialized_effects[5], DOF);
			JsonUtility.FromJsonOverwrite(serialized_effects[6], GRAIN);
			JsonUtility.FromJsonOverwrite(serialized_effects[7], LENS);
			JsonUtility.FromJsonOverwrite(serialized_effects[8], BLUR);
			JsonUtility.FromJsonOverwrite(serialized_effects[9], REFL);
			JsonUtility.FromJsonOverwrite(serialized_effects[10], VIGN);
		}

		public void Save() {
			var filepath = Main.modEntry.Path;
			try {
				using (var writer = new StreamWriter($"{filepath}{name}.preset.json")) {
					Serialize();
					writer.WriteLine(JsonUtility.ToJson(this, true));
				}
			}
			catch (Exception e) {
				Main.log($"Can't save {filepath}{name} preset. ex: {e.Message}");
			}
		}

		static public Preset Load(string json) {
			var p = new Preset();
			JsonUtility.FromJsonOverwrite(json, p);
			p.Deserialize();
			return p;
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
			string[] filePaths = Directory.GetFiles(modEntry.Path, "*.preset.json");
			foreach (var filePath in filePaths) {
				try {
					var json = File.ReadAllText(filePath);
					var result = Preset.Load(json);
					presets.Add(result.name, result);
					if (!settings.presetOrder.Contains(result.name)) {
						settings.presetOrder.Add(result.name);
					}
					log("preset: " + result.name + " loaded");
				}
				catch (Exception e) {
					log($"ex: {e}");
				}
			}

			if (settings.presetOrder.ToArray().Length == 0) {
				var def_p = new Preset(default_name);
				def_p.Save();
				presets.Add(def_p.name, def_p);
				if (!settings.presetOrder.Contains(def_p.name)) {
					settings.presetOrder.Add(def_p.name);
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
					foreach (var preset in presets.Values) {
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
