using Harmony12;
using System;
using System.Reflection;
using UnityEngine.Rendering.PostProcessing;
using UnityModManagerNet;
using XLShredLib;

namespace BabboSettings {

	[Serializable]
	public class Settings : UnityModManager.ModSettings {

		public bool ENABLE_POST = true;
		public float FOV = 60;

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
		public bool COLOR_enabled = true;
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
		
		public Settings() {
		}

		public override void Save(UnityModManager.ModEntry modEntry) {
			Main.settingsGUI.Save();
			Save(this, modEntry);
		}
	}

	static class Main {
		public static bool enabled;
		public static Settings settings;
		public static HarmonyInstance harmonyInstance;
		public static string modId = "BabboSettings";
		public static SettingsGUI settingsGUI;
		private static UnityModManager.ModEntry modEntry;

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
				settingsGUI = ModMenu.Instance.gameObject.AddComponent<SettingsGUI>();
			}
			else {
				harmonyInstance.UnpatchAll(harmonyInstance.Id);
				UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<SettingsGUI>());
			}
			return true;
		}

		static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
			settings.Save(modEntry);
		}

		internal static void Save() {
			settings.Save(modEntry);
		}
	}
}
