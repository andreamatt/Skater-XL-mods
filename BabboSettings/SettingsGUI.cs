using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

namespace BabboSettings {

	internal partial class SettingsGUI : MonoBehaviour {

		private bool reading_main = true;
		private bool fast_apply = false;

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
		private LensDistortion GAME_LENS;
		private MotionBlur GAME_BLUR;
		private ScreenSpaceReflections GAME_REFL;
		private Vignette GAME_VIGN;

		private string[] aa_names = { "None", "FXAA", "SMAA", "TAA" };
		private string[] smaa_quality = { "Low", "Medium", "High" };
		private string[] ao_quality = { "Lowest", "Low", "Medium", "High", "Ultra" };
		private string[] ao_mode = { "SAO", "MSVO" };
		private string[] refl_presets = { "Low", "Lower", "Medium", "High", "Higher", "Ultra", "Overkill" };

		private bool sp_AO, sp_EXPO, sp_BLOOM, sp_CA, sp_COLOR, sp_DOF, sp_GRAIN, sp_LENS, sp_BLUR, sp_REFL, sp_VIGN;

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
					TO_READ = DeepClone(Main.settings);
					reading_main = false;
					log("Done reading main");
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
					TO_READ.COLOR_temperature = GAME_COLOR.temperature.value;
					TO_READ.COLOR_post_exposure = GAME_COLOR.postExposure.value;
					TO_READ.COLOR_saturation = GAME_COLOR.saturation.value;
					TO_READ.COLOR_contrast = GAME_COLOR.contrast.value;
					TO_READ.DOF = GAME_DOF;
					TO_READ.GRAIN = GAME_GRAIN;
					TO_READ.LENS = GAME_LENS;
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
					GUILayout.BeginHorizontal();
					GUILayout.Label("Field Of View: " + TO_READ.FOV);
					main.fieldOfView = GUILayout.HorizontalSlider(TO_READ.FOV, 30, 110, sliderStyle, thumbStyle);
					GUILayout.EndHorizontal();
					if (GUILayout.Button("Reset")) {
						main.fieldOfView = 60;
					}
				}

				// AntiAliasing
				{
					GUILayout.Label("AntiAliasing");
					post_layer.antialiasingMode = (Antialiasing)(GUILayout.SelectionGrid((int)TO_READ.AA_MODE, aa_names, aa_names.Length));
					if (post_layer.antialiasingMode == Antialiasing.SubpixelMorphologicalAntialiasing) {
						GAME_SMAA.quality = (SubpixelMorphologicalAntialiasing.Quality)GUILayout.SelectionGrid((int)TO_READ.SMAA.quality, smaa_quality, smaa_quality.Length);
					}
					else if (post_layer.antialiasingMode == Antialiasing.TemporalAntialiasing) {
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("sharpness: " + TO_READ.TAA_sharpness);
							GAME_TAA.sharpness = GUILayout.HorizontalSlider(TO_READ.TAA_sharpness, 0, 3, sliderStyle, thumbStyle);
							GUILayout.Label("jitter spread: " + TO_READ.TAA_jitter);
							GAME_TAA.jitterSpread = GUILayout.HorizontalSlider(TO_READ.TAA_jitter, 0, 1, sliderStyle, thumbStyle);
						}
						GUILayout.EndHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("stationary blending: " + TO_READ.TAA_stationary);
							GAME_TAA.stationaryBlending = GUILayout.HorizontalSlider(TO_READ.TAA_stationary, 0, 1);
							GUILayout.Label("motion blending: " + TO_READ.TAA_motion);
							GAME_TAA.motionBlending = GUILayout.HorizontalSlider(TO_READ.TAA_motion, 0, 1);
						}
						GUILayout.EndHorizontal();
						if (GUILayout.Button("Default TAA")) {
							GAME_TAA = new TemporalAntialiasing();
						}
					}
				}

				if (TO_READ.ENABLE_POST) {
					// Ambient Occlusion
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_AO.enabled.Override(GUILayout.Toggle(TO_READ.AO.enabled.value, "Ambient occlusion"));
						if (GAME_AO.enabled.value) sp_AO = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_AO : sp_AO;
						GUILayout.EndHorizontal();
						if (GAME_AO.enabled.value && sp_AO) {
							GAME_AO.quality.Override((AmbientOcclusionQuality)GUILayout.SelectionGrid((int)TO_READ.AO.quality.value, ao_quality, ao_quality.Length));
							GAME_AO.mode.Override((AmbientOcclusionMode)GUILayout.SelectionGrid((int)TO_READ.AO.mode.value, ao_mode, ao_mode.Length));
							if (GUILayout.Button("Reset")) {
								GAME_AO = reset<AmbientOcclusion>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Automatic Exposure
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_EXPO.enabled.Override(GUILayout.Toggle(TO_READ.EXPO.enabled.value, "Automatic Exposure"));
						if (GAME_EXPO.enabled.value) sp_EXPO = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_EXPO : sp_EXPO;
						GUILayout.EndHorizontal();
						if (GAME_EXPO.enabled.value && sp_EXPO) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Compensation: " + TO_READ.EXPO.keyValue.value);
							GAME_EXPO.keyValue.Override(GUILayout.HorizontalSlider(TO_READ.EXPO.keyValue.value, 0, 4, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_EXPO = reset<AutoExposure>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Bloom
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_BLOOM.enabled.Override(GUILayout.Toggle(TO_READ.BLOOM.enabled.value, "Bloom"));
						if (GAME_BLOOM.enabled.value) sp_BLOOM = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_BLOOM : sp_BLOOM;
						GUILayout.EndHorizontal();
						if (GAME_BLOOM.enabled.value && sp_BLOOM) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Intensity: " + TO_READ.BLOOM.intensity.value);
							GAME_BLOOM.intensity.Override(GUILayout.HorizontalSlider(TO_READ.BLOOM.intensity.value, 0, 4, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_BLOOM = reset<Bloom>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Chromatic aberration
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_CA.enabled.Override(GUILayout.Toggle(TO_READ.CA.enabled.value, "Chromatic aberration"));
						if (GAME_CA.enabled.value) sp_CA = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_CA : sp_CA;
						GUILayout.EndHorizontal();
						if (GAME_CA.enabled.value && sp_CA) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Intensity: " + TO_READ.CA.intensity.value);
							GAME_CA.intensity.Override(GUILayout.HorizontalSlider(TO_READ.CA.intensity.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_CA = reset<ChromaticAberration>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Color Grading
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_COLOR.enabled.Override(GUILayout.Toggle(TO_READ.COLOR_enabled, "Color Grading"));
						if (GAME_COLOR.enabled.value) sp_COLOR = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_COLOR : sp_COLOR;
						GUILayout.EndHorizontal();
						if (GAME_COLOR.enabled.value && sp_COLOR) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Temperature: " + TO_READ.COLOR_temperature);
							GAME_COLOR.temperature.Override(GUILayout.HorizontalSlider(TO_READ.COLOR_temperature, -100, 100, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Post-exposure: " + TO_READ.COLOR_post_exposure);
							GAME_COLOR.postExposure.Override(GUILayout.HorizontalSlider(TO_READ.COLOR_post_exposure, 0, 5, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Saturation: " + TO_READ.COLOR_saturation);
							GAME_COLOR.saturation.Override(GUILayout.HorizontalSlider(TO_READ.COLOR_saturation, -100, 100, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Contrast: " + TO_READ.COLOR_contrast);
							GAME_COLOR.contrast.Override(GUILayout.HorizontalSlider(TO_READ.COLOR_contrast, -100, 100, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								// NEED TO ADD RESET ONE BY ONE
								GAME_COLOR.temperature.Override(0);
								GAME_COLOR.postExposure.Override(0);
								GAME_COLOR.saturation.Override(0);
								GAME_COLOR.contrast.Override(0);
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Depth Of Field
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_DOF.enabled.Override(GUILayout.Toggle(TO_READ.DOF.enabled.value, "Depth Of Field"));
						if (GAME_DOF.enabled.value) sp_DOF = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_DOF : sp_DOF;
						GUILayout.EndHorizontal();
						if (GAME_DOF.enabled.value && sp_DOF) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("focus distance: " + TO_READ.DOF.focusDistance.value);
							GAME_DOF.focusDistance.Override(GUILayout.HorizontalSlider(TO_READ.DOF.focusDistance.value, 0, 20, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("aperture: " + TO_READ.DOF.aperture.value);
							GAME_DOF.aperture.Override(GUILayout.HorizontalSlider(TO_READ.DOF.aperture.value, 0.1f, 32, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("focal length: " + TO_READ.DOF.focalLength.value);
							GAME_DOF.focalLength.Override(GUILayout.HorizontalSlider(TO_READ.DOF.focalLength.value, 1, 300, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_DOF = reset<DepthOfField>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Grain
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_GRAIN.enabled.Override(GUILayout.Toggle(TO_READ.GRAIN.enabled.value, "Grain"));
						if (GAME_GRAIN.enabled.value) sp_GRAIN = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_GRAIN : sp_GRAIN;
						GUILayout.EndHorizontal();
						if (GAME_GRAIN.enabled.value && sp_GRAIN) {
							GUILayout.BeginHorizontal();
							GAME_GRAIN.colored.Override(GUILayout.Toggle(TO_READ.GRAIN.colored.value, "colored"));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("intensity: " + TO_READ.GRAIN.intensity.value);
							GAME_GRAIN.intensity.Override(GUILayout.HorizontalSlider(TO_READ.GRAIN.intensity.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("size: " + TO_READ.GRAIN.size.value);
							GAME_GRAIN.size.Override(GUILayout.HorizontalSlider(TO_READ.GRAIN.size.value, 0.3f, 3, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("luminance contribution: " + TO_READ.GRAIN.lumContrib.value);
							GAME_GRAIN.lumContrib.Override(GUILayout.HorizontalSlider(TO_READ.GRAIN.lumContrib.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_GRAIN = reset<Grain>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Lens Distortion
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_LENS.enabled.Override(GUILayout.Toggle(TO_READ.LENS.enabled.value, "Lens distortion"));
						if (GAME_LENS.enabled.value) sp_LENS = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_LENS : sp_LENS;
						GUILayout.EndHorizontal();
						if (GAME_LENS.enabled.value && sp_LENS) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Intensity: " + TO_READ.LENS.intensity.value);
							GAME_LENS.intensity.Override(GUILayout.HorizontalSlider(TO_READ.LENS.intensity.value, -100, 100, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("X: " + TO_READ.LENS.intensityX.value);
							GAME_LENS.intensityX.Override(GUILayout.HorizontalSlider(TO_READ.LENS.intensityX.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Y: " + TO_READ.LENS.intensityY.value);
							GAME_LENS.intensityY.Override(GUILayout.HorizontalSlider(TO_READ.LENS.intensityY.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Scale: " + TO_READ.LENS.scale.value);
							GAME_LENS.scale.Override(GUILayout.HorizontalSlider(TO_READ.LENS.scale.value, 0.1f, 5, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_LENS = reset<LensDistortion>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Motion Blur
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_BLUR.enabled.Override(GUILayout.Toggle(TO_READ.BLUR.enabled.value, "Motion blur"));
						if (GAME_BLUR.enabled.value) sp_BLUR = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_BLUR : sp_BLUR;
						GUILayout.EndHorizontal();
						if (GAME_BLUR.enabled.value && sp_BLUR) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Shutter angle: " + GAME_BLUR.shutterAngle.value);
							GAME_BLUR.shutterAngle.Override((int)Math.Floor(GUILayout.HorizontalSlider(TO_READ.BLUR.shutterAngle, 0, 360, sliderStyle, thumbStyle)));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Sample count: " + GAME_BLUR.sampleCount.value);
							GAME_BLUR.sampleCount.Override((int)Math.Floor(GUILayout.HorizontalSlider(TO_READ.BLUR.sampleCount, 4, 32, sliderStyle, thumbStyle)));
							GUILayout.EndHorizontal();
							if (GUILayout.Button("Reset")) {
								GAME_BLUR = reset<MotionBlur>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Screen Space Reflections
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_REFL.enabled.Override(GUILayout.Toggle(TO_READ.REFL.enabled.value, "Reflections"));
						if (GAME_REFL.enabled.value) sp_REFL = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_REFL : sp_REFL;
						GUILayout.EndHorizontal();
						if (GAME_REFL.enabled.value && sp_REFL) {
							GAME_REFL.preset.Override((ScreenSpaceReflectionPreset)GUILayout.SelectionGrid((int)GAME_REFL.preset.value, refl_presets, refl_presets.Length));
							if (GUILayout.Button("Reset")) {
								GAME_REFL = reset<ScreenSpaceReflections>();
							}
						}
						GUILayout.FlexibleSpace();
					}

					// Vignette
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						GAME_VIGN.enabled.Override(GUILayout.Toggle(TO_READ.VIGN.enabled.value, "Vignette"));
						if (GAME_VIGN.enabled.value) sp_VIGN = GUILayout.Button("show/hide", spoilerBtnStyle) ? !sp_VIGN : sp_VIGN;
						GUILayout.EndHorizontal();
						if ((GAME_VIGN.enabled.value && sp_VIGN) || fast_apply) {
							GAME_VIGN.mode.Override(VignetteMode.Classic);
							GUILayout.BeginHorizontal();
							GUILayout.Label("Intensity: " + GAME_VIGN.intensity.value);
							GAME_VIGN.intensity.Override(GUILayout.HorizontalSlider(TO_READ.VIGN.intensity.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Smoothness: " + GAME_VIGN.smoothness.value);
							GAME_VIGN.smoothness.Override(GUILayout.HorizontalSlider(TO_READ.VIGN.smoothness.value, 0.1f, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Roundness: " + GAME_VIGN.roundness.value);
							GAME_VIGN.roundness.Override(GUILayout.HorizontalSlider(TO_READ.VIGN.roundness.value, 0, 1, sliderStyle, thumbStyle));
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GAME_VIGN.rounded.Override(GUILayout.Toggle(TO_READ.VIGN.rounded.value, "Rounded"));
							if (GUILayout.Button("Reset")) {
								GAME_VIGN = reset<Vignette>();
							}
							GUILayout.EndHorizontal();
						}
						GUILayout.FlexibleSpace();
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

			if (fast_apply) {
				fast_apply = false;
				Close();
			}
		}

		private T reset<T>() where T : PostProcessEffectSettings {
			bool enabled = post_volume.profile.GetSetting<T>().enabled.value;
			post_volume.profile.RemoveSettings<T>();
			T eff = post_volume.profile.AddSettings<T>();
			eff.enabled.Override(enabled);
			return eff;
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
				if ((GAME_LENS = post_volume.profile.GetSetting<LensDistortion>()) == null) {
					log("Not foudn lens");
					GAME_LENS = post_volume.profile.AddSettings<LensDistortion>();
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
			if (!showUI) {
				fast_apply = true;
				Open();
			}

			log("Done reading post_layer and post_volume settings");
		}
	}
}
