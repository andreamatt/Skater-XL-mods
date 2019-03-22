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

        public PostProcessLayer.Antialiasing AA_MODE = new PostProcessLayer.Antialiasing();
        public float TAA_sharpness = new TemporalAntialiasing().sharpness;
        public float TAA_jitter = new TemporalAntialiasing().jitterSpread;
        public float TAA_stationary = new TemporalAntialiasing().stationaryBlending;
        public float TAA_motion = new TemporalAntialiasing().motionBlending;
        public SubpixelMorphologicalAntialiasing SMAA = new SubpixelMorphologicalAntialiasing();

        public float FOV = 60;

        public AmbientOcclusion AO = new AmbientOcclusion();
        public AutoExposure EXPO = new AutoExposure();
        public Bloom BLOOM = new Bloom();
        public ChromaticAberration CA = new ChromaticAberration();
        public bool COLOR_enabled = true;
        public float COLOR_saturation = 0;
        public DepthOfField DOF = new DepthOfField();
        public Grain GRAIN = new Grain();
        public MotionBlur BLUR = new MotionBlur();
        public ScreenSpaceReflections REFL = new ScreenSpaceReflections();
        public Vignette VIGN = new Vignette();
        
        public Settings() {
        }

        public override void Save(UnityModManager.ModEntry modEntry) {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }

    static class Main {
        public static bool enabled;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;
        public static string modId = "BabboSettings";

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (enabled == value) return true;
            enabled = value;

            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<SettingsGUI>();
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
    }
}
