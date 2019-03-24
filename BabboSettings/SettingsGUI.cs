using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

namespace BabboSettings {

	internal partial class SettingsGUI : MonoBehaviour {

		private bool reading_main = true;

		// Settings stuff
		private Camera main;
		private PostProcessLayer post_layer;
		private PostProcessVolume post_volume;
		private Settings TO_READ = new Settings();

		private FastApproximateAntialiasing GAME_FXAA;
		private TemporalAntialiasing GAME_TAA; // NOT SERIALIZABLE
		private SubpixelMorphologicalAntialiasing GAME_SMAA;

		private AmbientOcclusion GAME_AO;
		private AutoExposure GAME_EXPO;
		private Bloom GAME_BLOOM;
		private ChromaticAberration GAME_CA;
		private ColorGrading GAME_COLOR; // NOT SERIALIZABLE
		private DepthOfField GAME_DOF;
		private Grain GAME_GRAIN;
		private MotionBlur GAME_BLUR;
		private ScreenSpaceReflections GAME_REFL;
		private Vignette GAME_VIGN;

		private string[] aa_names = { "None", "FXAA", "SMAA", "TAA" };
		private string[] smaa_quality = { "Low", "Medium", "High" };
		private string[] ao_quality = { "Lowest", "Low", "Medium", "High", "Ultra" };
		private string[] ao_mode = { "SAO", "MSVO" };

		private void Update() {
			bool keyUp = Input.GetKeyUp(KeyCode.Backspace);
			if (keyUp) {
				if (showUI == false) {
					Open();
				}
				else {
					Close();
				}
			}
			if (post_volume == null) {
				log("Post volume is null (probably map changed)");
				post_volume = FindObjectOfType<PostProcessVolume>();
				if (post_volume == null) {
					log("Post volume not found => creating");
					GameObject post_vol_go = new GameObject();
					post_vol_go.layer = 8;
					post_volume = post_vol_go.AddComponent<PostProcessVolume>();
					post_volume.profile = new PostProcessProfile();
					post_volume.isGlobal = true;
					log("Now a & e:" + post_volume.isActiveAndEnabled);
					log("Has profile: " + post_volume.HasInstantiatedProfile());
				}
				getSettings();
			}
		}

		void RenderWindow(int windowID) {
			if (Event.current.type == EventType.Repaint) windowRect.height = 0;

			GUI.DragWindow(new Rect(0, 0, 10000, 20));

			GUILayout.BeginVertical();
			{
				if (reading_main) {
					if(Main.settings!=null) TO_READ = DeepClone(Main.settings);
					reading_main = false;
				}
				else {
					TO_READ.ENABLE_POST = post_volume.enabled;
					TO_READ.FOV = main.fieldOfView;

					TO_READ.AA_MODE = post_layer.antialiasingMode;
					TO_READ.TAA_sharpness = post_layer.temporalAntialiasing.sharpness;
					TO_READ.TAA_jitter = post_layer.temporalAntialiasing.jitterSpread;
					TO_READ.TAA_stationary = post_layer.temporalAntialiasing.stationaryBlending;
					TO_READ.TAA_motion = post_layer.temporalAntialiasing.motionBlending;
					TO_READ.SMAA = GAME_SMAA;

					TO_READ.AO = GAME_AO;
					TO_READ.EXPO = GAME_EXPO;
					TO_READ.BLOOM = GAME_BLOOM;
					TO_READ.CA = GAME_CA;
					TO_READ.COLOR_enabled = GAME_COLOR.enabled.value;
					TO_READ.COLOR_saturation = GAME_COLOR.saturation.value;
					TO_READ.DOF = GAME_DOF;
					TO_READ.GRAIN = GAME_GRAIN;
					TO_READ.BLUR = GAME_BLUR;
					TO_READ.REFL = GAME_REFL;
					TO_READ.VIGN = GAME_VIGN;
				}
				
				// Post in general
				{
					post_volume.enabled = GUILayout.Toggle(TO_READ.ENABLE_POST, "Enable post processing");
				}

				// Field Of View
				{
					GUILayout.Label("Field Of View");
					main.fieldOfView = GUILayout.HorizontalSlider(TO_READ.FOV, 30, 110);
				}

				// AntiAliasing
				{
					GUILayout.Space(5);
					GUILayout.Label("AntiAliasing");
					post_layer.antialiasingMode = (Antialiasing)(GUILayout.SelectionGrid((int)TO_READ.AA_MODE, aa_names, aa_names.Length));
					if (post_layer.antialiasingMode == Antialiasing.SubpixelMorphologicalAntialiasing) {
						GAME_SMAA.quality = (SubpixelMorphologicalAntialiasing.Quality)GUILayout.SelectionGrid((int)TO_READ.SMAA.quality, smaa_quality, smaa_quality.Length);
					}
					else if (post_layer.antialiasingMode == Antialiasing.TemporalAntialiasing) {
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("sharpness");
							GAME_TAA.sharpness = GUILayout.HorizontalSlider(TO_READ.TAA_sharpness, 0, 3);
							GUILayout.Label("jitter spread");
							GAME_TAA.jitterSpread = GUILayout.HorizontalSlider(TO_READ.TAA_jitter, 0, 1);
						}
						GUILayout.EndHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("stationary blending");
							GAME_TAA.stationaryBlending = GUILayout.HorizontalSlider(TO_READ.TAA_stationary, 0, 1);
							GUILayout.Label("motion blending");
							GAME_TAA.motionBlending = GUILayout.HorizontalSlider(TO_READ.TAA_motion, 0, 1);
						}
						GUILayout.EndHorizontal();
						if (GUILayout.Button("Default TAA")) {
							var defaultAA = new TemporalAntialiasing();
							GAME_TAA.sharpness = defaultAA.sharpness;
							GAME_TAA.jitterSpread = defaultAA.jitterSpread;
							GAME_TAA.stationaryBlending = defaultAA.stationaryBlending;
							GAME_TAA.motionBlending = defaultAA.motionBlending;
						}
					}
					GUILayout.FlexibleSpace();
				}

				if (TO_READ.ENABLE_POST) {
					// Ambiental Occlusion
					{
						GUILayout.FlexibleSpace();
						GAME_AO.enabled.Override(GUILayout.Toggle(TO_READ.AO.enabled.value, "Ambiental occlusion"));
						if (GAME_AO.enabled.value) {
							GAME_AO.quality.Override((AmbientOcclusionQuality)GUILayout.SelectionGrid((int)TO_READ.AO.quality.value, ao_quality, ao_quality.Length));
							GAME_AO.mode.Override((AmbientOcclusionMode)GUILayout.SelectionGrid((int)TO_READ.AO.mode.value, ao_mode, ao_mode.Length));
						}
						GUILayout.FlexibleSpace();
					}

					// Automatic Exposure
					{
						GAME_EXPO.enabled.Override(GUILayout.Toggle(TO_READ.EXPO.enabled.value, "Automatic Exposure"));
					}

					// Bloom
					{
						GUILayout.FlexibleSpace();
						GAME_BLOOM.enabled.Override(GUILayout.Toggle(TO_READ.BLOOM.enabled.value, "Bloom"));
						GUILayout.FlexibleSpace();
					}

					// Chromatic aberration
					{
						GUILayout.FlexibleSpace();
						GAME_CA.enabled.Override(GUILayout.Toggle(TO_READ.CA.enabled.value, "Chromatic aberration"));
						GUILayout.FlexibleSpace();
					}

					// Color Grading
					{
						GAME_COLOR.enabled.Override(GUILayout.Toggle(TO_READ.COLOR_enabled, "Color Grading"));
						GAME_COLOR.saturation.Override(GUILayout.HorizontalSlider(TO_READ.COLOR_saturation, -100, 100));
					}

					// Depth Of Field
					{
						GAME_DOF.enabled.Override(GUILayout.Toggle(TO_READ.DOF.enabled.value, "Depth Of Field"));
					}

					// Grain
					{
						GAME_GRAIN.enabled.Override(GUILayout.Toggle(TO_READ.GRAIN.enabled.value, "Grain"));
					}

					// Motion Blur
					{
						GUILayout.FlexibleSpace();
						GAME_BLUR.enabled.Override(GUILayout.Toggle(TO_READ.BLUR.enabled.value, "Motion blur"));
						if (TO_READ.BLUR.enabled.value) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Shutter angle");
							GAME_BLUR.shutterAngle.Override((int)Math.Floor(GUILayout.HorizontalSlider(TO_READ.BLUR.shutterAngle, 0, 360)));
							GUILayout.Label("Sample count");
							GAME_BLUR.sampleCount.Override((int)Math.Floor(GUILayout.HorizontalSlider(TO_READ.BLUR.sampleCount, 0, 32)));
							GUILayout.EndHorizontal();
						}
						GUILayout.FlexibleSpace();
					}

					// Screen Space Reflections
					{
						GAME_REFL.enabled.Override(GUILayout.Toggle(TO_READ.REFL.enabled.value, "Reflections"));
					}

					// Vignette
					{
						GAME_VIGN.enabled.Override(GUILayout.Toggle(TO_READ.VIGN.enabled.value, "Vignette"));
					}
				}
				// link to repo?
				if (GUILayout.Button("by Babbo Elu")) {
					GUILayout.FlexibleSpace();
					Application.OpenURL("http://github.com/andreamatt/BabboSettings");
				}

				if (GUILayout.Button("Reload (Map changed?)")) {
					getSettings();
				}

				if (GUILayout.Button("Close")) {
					GUILayout.FlexibleSpace();
					Close();
				}
			}
			GUILayout.EndVertical();
		}

		private void getSettings() {

			main = Camera.main;
			post_layer = main.GetComponent<PostProcessLayer>();

			post_volume = FindObjectOfType<PostProcessVolume>();
			if (post_volume != null) {
				if ((GAME_AO = post_volume.profile.GetSetting<AmbientOcclusion>()) == null) {
					log("Not found ao");
					GAME_AO = post_volume.profile.AddSettings<AmbientOcclusion>();
				}
				if ((GAME_EXPO = post_volume.profile.GetSetting<AutoExposure>()) == null) {
					log("Not found expo");
					GAME_EXPO = post_volume.profile.AddSettings<AutoExposure>();
				}
				if ((GAME_BLOOM = post_volume.profile.GetSetting<Bloom>()) == null) {
					log("Not found bloom");
					GAME_BLOOM = post_volume.profile.AddSettings<Bloom>();
				}
				if ((GAME_CA = post_volume.profile.GetSetting<ChromaticAberration>()) == null) {
					log("Not found ca");
					GAME_CA = post_volume.profile.AddSettings<ChromaticAberration>();
				}
				if ((GAME_COLOR = post_volume.profile.GetSetting<ColorGrading>()) == null) {
					log("Not found color");
					GAME_COLOR = post_volume.profile.AddSettings<ColorGrading>();
				}
				if ((GAME_DOF = post_volume.profile.GetSetting<DepthOfField>()) == null) {
					log("Not found dof");
					GAME_DOF = post_volume.profile.AddSettings<DepthOfField>();
				}
				if ((GAME_GRAIN = post_volume.profile.GetSetting<Grain>()) == null) {
					log("Not found grain");
					GAME_GRAIN = post_volume.profile.AddSettings<Grain>();
				}
				if ((GAME_BLUR = post_volume.profile.GetSetting<MotionBlur>()) == null) {
					log("Not found blur");
					GAME_BLUR = post_volume.profile.AddSettings<MotionBlur>();
				}
				if ((GAME_REFL = post_volume.profile.GetSetting<ScreenSpaceReflections>()) == null) {
					log("Not found refl");
					GAME_REFL = post_volume.profile.AddSettings<ScreenSpaceReflections>();
				}
				if ((GAME_VIGN = post_volume.profile.GetSetting<Vignette>()) == null) {
					log("Not found vign");
					GAME_VIGN = post_volume.profile.AddSettings<Vignette>();
				}
			}
			else {
				log("Post_volume is null in getSettings");
			}
			log("Searched all effects");

			GAME_FXAA = post_layer.fastApproximateAntialiasing;
			GAME_TAA = post_layer.temporalAntialiasing;
			GAME_SMAA = post_layer.subpixelMorphologicalAntialiasing;
			log("Found all AAs");

			reading_main = true;

			log("Done reading post_layer and post_volume settings");
		}
	}
}
