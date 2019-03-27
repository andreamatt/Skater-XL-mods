using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

namespace BabboSettings {

	internal partial class SettingsGUI : MonoBehaviour {
		// Settings stuff
		private PostProcessLayer post_layer;
		private PostProcessVolume post_volume;

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

		private bool sp_AA, sp_AO, sp_EXPO, sp_BLOOM, sp_CA, sp_COLOR, sp_DOF, sp_GRAIN, sp_LENS, sp_BLUR, sp_REFL, sp_VIGN;
		private bool choosing_name, changing_preset;
		private string name_text = "";
		private bool focus_player = true;

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
			if (focus_player) {
				GAME_DOF.focusDistance.Override(Vector3.Distance(PlayerController.Instance.skaterController.skaterTransform.position, Camera.main.transform.position));
			}
		}

		void RenderWindow(int windowID) {
			if (Event.current.type == EventType.Repaint) windowRect.height = 0;

			GUI.DragWindow(new Rect(0, 0, 10000, 20));

			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(750));
			{
				if (choosing_name) {
					name_text = GUILayout.TextField(name_text);
					GUILayout.BeginHorizontal();
					if (GUILayout.Button("Confirm")) {
						choosing_name = false;
						if (Main.presets.ContainsKey(name_text)) {
							log("Preset already exists");
						}
						Preset new_preset = new Preset(name_text);
						SaveTo(new_preset);
						Main.presets[new_preset.name] = new_preset;
						Main.select(name_text);
					}
					if (GUILayout.Button("Cancel")) {
						choosing_name = false;
					}
					GUILayout.EndHorizontal();
				}
				else if (changing_preset) {
					foreach (var preset in Main.presets) {
						if (GUILayout.Button(preset.Key)) {
							SaveTo(Main.selectedPreset);
							Main.select(preset.Key);
							Apply(Main.selectedPreset);
							changing_preset = false;
						}
					}
					GUILayout.Label(separator, separatorStyle);
					if (GUILayout.Button("Cancel")) {
						changing_preset = false;
					}
				}
				else {
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label("Preset: ");
						GUILayout.Label(Main.settings.presetName, usingStyle);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						if (GUILayout.Button("Save as new preset")) {
							choosing_name = true;
							name_text = "";
						}
						if (GUILayout.Button("Change preset")) {
							changing_preset = true;
						}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						if (GUILayout.Button("Save")) {
							Main.Save();
						}
						if (GUILayout.Button("Save and close")) {
							Close();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.Label(separator, separatorStyle);
					// Post in general
					{
						post_volume.enabled = GUILayout.Toggle(post_volume.enabled, "Enable post processing");
					}
					GUILayout.Label(separator, separatorStyle);
					// Field Of View
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label("Field Of View: " + Camera.main.fieldOfView);
						Camera.main.fieldOfView = GUILayout.HorizontalSlider(Camera.main.fieldOfView, 1, 179, sliderStyle, thumbStyle);
						GUILayout.EndHorizontal();
						if (GUILayout.Button("Reset")) {
							Camera.main.fieldOfView = 60;
						}
					}
					GUILayout.Label(separator, separatorStyle);
					// AntiAliasing
					{
						GUILayout.Label("AntiAliasing");
						post_layer.antialiasingMode = (Antialiasing)(GUILayout.SelectionGrid((int)post_layer.antialiasingMode, aa_names, aa_names.Length));
						if (post_layer.antialiasingMode == Antialiasing.SubpixelMorphologicalAntialiasing) {
							sp_AA = GUILayout.Button(sp_AA ? "hide" : "show", spoilerBtnStyle) ? !sp_AA : sp_AA;
							if (sp_AA) {
								GAME_SMAA.quality = (SubpixelMorphologicalAntialiasing.Quality)GUILayout.SelectionGrid((int)GAME_SMAA.quality, smaa_quality, smaa_quality.Length);
							}
						}
						else if (post_layer.antialiasingMode == Antialiasing.TemporalAntialiasing) {
							sp_AA = GUILayout.Button(sp_AA ? "hide" : "show", spoilerBtnStyle) ? !sp_AA : sp_AA;
							if (sp_AA) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("sharpness: " + GAME_TAA.sharpness);
								GAME_TAA.sharpness = GUILayout.HorizontalSlider(GAME_TAA.sharpness, 0, 3, sliderStyle, thumbStyle);
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("jitter spread: " + GAME_TAA.jitter);
								GAME_TAA.jitterSpread = GUILayout.HorizontalSlider(GAME_TAA.jitterSpread, 0, 1, sliderStyle, thumbStyle);
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("stationary blending: " + GAME_TAA.stationaryBlending);
								GAME_TAA.stationaryBlending = GUILayout.HorizontalSlider(GAME_TAA.stationaryBlending, 0, 1, sliderStyle, thumbStyle);
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("motion blending: " + GAME_TAA.motionBlending);
								GAME_TAA.motionBlending = GUILayout.HorizontalSlider(GAME_TAA.motionBlending, 0, 1, sliderStyle, thumbStyle);
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Default TAA")) {
									GAME_TAA = post_layer.temporalAntialiasing = new TemporalAntialiasing();
								}
							}
						}
					}
					GUILayout.Label(separator, separatorStyle);
					if (post_volume.enabled) {
						// Ambient Occlusion
						{
							GUILayout.BeginHorizontal();
							GAME_AO.enabled.Override(GUILayout.Toggle(GAME_AO.enabled.value, "Ambient occlusion"));
							if (GAME_AO.enabled.value) sp_AO = GUILayout.Button(sp_AO ? "hide" : "show", spoilerBtnStyle) ? !sp_AO : sp_AO;
							GUILayout.EndHorizontal();
							if (GAME_AO.enabled.value && sp_AO) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Intensity: " + GAME_AO.intensity.value);
								GAME_AO.intensity.Override(GUILayout.HorizontalSlider(GAME_AO.intensity.value, 0, 4, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GAME_AO.quality.Override((AmbientOcclusionQuality)GUILayout.SelectionGrid((int)GAME_AO.quality.value, ao_quality, ao_quality.Length));
								GAME_AO.mode.Override((AmbientOcclusionMode)GUILayout.SelectionGrid((int)GAME_AO.mode.value, ao_mode, ao_mode.Length));
								if (GUILayout.Button("Reset")) {
									GAME_AO = reset<AmbientOcclusion>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Automatic Exposure
						{
							GUILayout.BeginHorizontal();
							GAME_EXPO.enabled.Override(GUILayout.Toggle(GAME_EXPO.enabled.value, "Automatic Exposure"));
							if (GAME_EXPO.enabled.value) sp_EXPO = GUILayout.Button(sp_EXPO ? "hide" : "show", spoilerBtnStyle) ? !sp_EXPO : sp_EXPO;
							GUILayout.EndHorizontal();
							if (GAME_EXPO.enabled.value && sp_EXPO) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Compensation: " + GAME_EXPO.keyValue.value);
								GAME_EXPO.keyValue.Override(GUILayout.HorizontalSlider(GAME_EXPO.keyValue.value, 0, 4, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Reset")) {
									GAME_EXPO = reset<AutoExposure>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Bloom
						{
							GUILayout.BeginHorizontal();
							GAME_BLOOM.enabled.Override(GUILayout.Toggle(GAME_BLOOM.enabled.value, "Bloom"));
							if (GAME_BLOOM.enabled.value) sp_BLOOM = GUILayout.Button(sp_BLOOM ? "hide" : "show", spoilerBtnStyle) ? !sp_BLOOM : sp_BLOOM;
							GUILayout.EndHorizontal();
							if (GAME_BLOOM.enabled.value && sp_BLOOM) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Intensity: " + GAME_BLOOM.intensity.value);
								GAME_BLOOM.intensity.Override(GUILayout.HorizontalSlider(GAME_BLOOM.intensity.value, 0, 4, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GAME_BLOOM.fastMode.Override(GUILayout.Toggle(GAME_BLOOM.fastMode.value, "Fast mode"));
								if (GUILayout.Button("Reset")) {
									GAME_BLOOM = reset<Bloom>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Chromatic aberration
						{
							GUILayout.BeginHorizontal();
							GAME_CA.enabled.Override(GUILayout.Toggle(GAME_CA.enabled.value, "Chromatic aberration"));
							if (GAME_CA.enabled.value) sp_CA = GUILayout.Button(sp_CA ? "hide" : "show", spoilerBtnStyle) ? !sp_CA : sp_CA;
							GUILayout.EndHorizontal();
							if (GAME_CA.enabled.value && sp_CA) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Intensity: " + GAME_CA.intensity.value);
								GAME_CA.intensity.Override(GUILayout.HorizontalSlider(GAME_CA.intensity.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GAME_CA.fastMode.Override(GUILayout.Toggle(GAME_CA.fastMode.value, "Fast mode"));
								if (GUILayout.Button("Reset")) {
									GAME_CA = reset<ChromaticAberration>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Color Grading
						{
							GUILayout.BeginHorizontal();
							GAME_COLOR.enabled.Override(GUILayout.Toggle(GAME_COLOR.enabled.value, "Color Grading"));
							if (GAME_COLOR.enabled.value) sp_COLOR = GUILayout.Button(sp_COLOR ? "hide" : "show", spoilerBtnStyle) ? !sp_COLOR : sp_COLOR;
							GUILayout.EndHorizontal();
							if (GAME_COLOR.enabled.value && sp_COLOR) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Temperature: " + GAME_COLOR.temperature.value);
								GAME_COLOR.temperature.Override(GUILayout.HorizontalSlider(GAME_COLOR.temperature.value, -100, 100, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Post-exposure: " + GAME_COLOR.postExposure.value);
								GAME_COLOR.postExposure.Override(GUILayout.HorizontalSlider(GAME_COLOR.postExposure.value, 0, 5, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Saturation: " + GAME_COLOR.saturation.value);
								GAME_COLOR.saturation.Override(GUILayout.HorizontalSlider(GAME_COLOR.saturation.value, -100, 100, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Contrast: " + GAME_COLOR.contrast.value);
								GAME_COLOR.contrast.Override(GUILayout.HorizontalSlider(GAME_COLOR.contrast.value, -100, 100, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Reset")) {
									// NEED TO ADD RESET ONE BY ONE
									GAME_COLOR.temperature.Override(0);
									GAME_COLOR.postExposure.Override(0);
									GAME_COLOR.saturation.Override(0);
									GAME_COLOR.contrast.Override(0);
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Depth Of Field
						{
							GUILayout.BeginHorizontal();
							GAME_DOF.enabled.Override(GUILayout.Toggle(GAME_DOF.enabled.value, "Depth Of Field"));
							if (GAME_DOF.enabled.value) sp_DOF = GUILayout.Button(sp_DOF ? "hide" : "show", spoilerBtnStyle) ? !sp_DOF : sp_DOF;
							GUILayout.EndHorizontal();
							if (GAME_DOF.enabled.value && sp_DOF) {
								focus_player = GUILayout.Toggle(focus_player, "Focus player");
								GUILayout.BeginHorizontal();
								GUILayout.Label("focus distance: " + GAME_DOF.focusDistance.value);
								if (!focus_player) {
									GAME_DOF.focusDistance.Override(GUILayout.HorizontalSlider(GAME_DOF.focusDistance.value, 0, 20, sliderStyle, thumbStyle));
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("aperture: " + GAME_DOF.aperture.value);
								GAME_DOF.aperture.Override(GUILayout.HorizontalSlider(GAME_DOF.aperture.value, 0.1f, 32, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("focal length: " + GAME_DOF.focalLength.value);
								GAME_DOF.focalLength.Override(GUILayout.HorizontalSlider(GAME_DOF.focalLength.value, 1, 300, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Reset")) {
									GAME_DOF = reset<DepthOfField>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Grain
						{
							GUILayout.BeginHorizontal();
							GAME_GRAIN.enabled.Override(GUILayout.Toggle(GAME_GRAIN.enabled.value, "Grain"));
							if (GAME_GRAIN.enabled.value) sp_GRAIN = GUILayout.Button(sp_GRAIN ? "hide" : "show", spoilerBtnStyle) ? !sp_GRAIN : sp_GRAIN;
							GUILayout.EndHorizontal();
							if (GAME_GRAIN.enabled.value && sp_GRAIN) {
								GUILayout.BeginHorizontal();
								GAME_GRAIN.colored.Override(GUILayout.Toggle(GAME_GRAIN.colored.value, "colored"));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("intensity: " + GAME_GRAIN.intensity.value);
								GAME_GRAIN.intensity.Override(GUILayout.HorizontalSlider(GAME_GRAIN.intensity.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("size: " + GAME_GRAIN.size.value);
								GAME_GRAIN.size.Override(GUILayout.HorizontalSlider(GAME_GRAIN.size.value, 0.3f, 3, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("luminance contribution: " + GAME_GRAIN.lumContrib.value);
								GAME_GRAIN.lumContrib.Override(GUILayout.HorizontalSlider(GAME_GRAIN.lumContrib.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Reset")) {
									GAME_GRAIN = reset<Grain>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Lens Distortion
						{
							GUILayout.BeginHorizontal();
							GAME_LENS.enabled.Override(GUILayout.Toggle(GAME_LENS.enabled.value, "Lens distortion"));
							if (GAME_LENS.enabled.value) sp_LENS = GUILayout.Button(sp_LENS ? "hide" : "show", spoilerBtnStyle) ? !sp_LENS : sp_LENS;
							GUILayout.EndHorizontal();
							if (GAME_LENS.enabled.value && sp_LENS) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Intensity: " + GAME_LENS.intensity.value);
								GAME_LENS.intensity.Override(GUILayout.HorizontalSlider(GAME_LENS.intensity.value, -100, 100, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("X: " + GAME_LENS.intensityX.value);
								GAME_LENS.intensityX.Override(GUILayout.HorizontalSlider(GAME_LENS.intensityX.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Y: " + GAME_LENS.intensityY.value);
								GAME_LENS.intensityY.Override(GUILayout.HorizontalSlider(GAME_LENS.intensityY.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Scale: " + GAME_LENS.scale.value);
								GAME_LENS.scale.Override(GUILayout.HorizontalSlider(GAME_LENS.scale.value, 0.1f, 5, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Reset")) {
									GAME_LENS = reset<LensDistortion>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Motion Blur
						{
							GUILayout.BeginHorizontal();
							GAME_BLUR.enabled.Override(GUILayout.Toggle(GAME_BLUR.enabled.value, "Motion blur"));
							if (GAME_BLUR.enabled.value) sp_BLUR = GUILayout.Button(sp_BLUR ? "hide" : "show", spoilerBtnStyle) ? !sp_BLUR : sp_BLUR;
							GUILayout.EndHorizontal();
							if (GAME_BLUR.enabled.value && sp_BLUR) {
								GUILayout.BeginHorizontal();
								GUILayout.Label("Shutter angle: " + GAME_BLUR.shutterAngle.value);
								GAME_BLUR.shutterAngle.Override((int)Math.Floor(GUILayout.HorizontalSlider(GAME_BLUR.shutterAngle, 0, 360, sliderStyle, thumbStyle)));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Sample count: " + GAME_BLUR.sampleCount.value);
								GAME_BLUR.sampleCount.Override((int)Math.Floor(GUILayout.HorizontalSlider(GAME_BLUR.sampleCount, 4, 32, sliderStyle, thumbStyle)));
								GUILayout.EndHorizontal();
								if (GUILayout.Button("Reset")) {
									GAME_BLUR = reset<MotionBlur>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Screen Space Reflections
						{
							GUILayout.BeginHorizontal();
							GAME_REFL.enabled.Override(GUILayout.Toggle(GAME_REFL.enabled.value, "Reflections"));
							if (GAME_REFL.enabled.value) sp_REFL = GUILayout.Button(sp_REFL ? "hide" : "show", spoilerBtnStyle) ? !sp_REFL : sp_REFL;
							GUILayout.EndHorizontal();
							if (GAME_REFL.enabled.value && sp_REFL) {
								GAME_REFL.preset.Override((ScreenSpaceReflectionPreset)GUILayout.SelectionGrid((int)GAME_REFL.preset.value, refl_presets, refl_presets.Length));
								if (GUILayout.Button("Reset")) {
									GAME_REFL = reset<ScreenSpaceReflections>();
								}
							}
						}
						GUILayout.Label(separator, separatorStyle);
						// Vignette
						{
							GUILayout.BeginHorizontal();
							GAME_VIGN.enabled.Override(GUILayout.Toggle(GAME_VIGN.enabled.value, "Vignette"));
							if (GAME_VIGN.enabled.value) sp_VIGN = GUILayout.Button(sp_VIGN ? "hide" : "show", spoilerBtnStyle) ? !sp_VIGN : sp_VIGN;
							GUILayout.EndHorizontal();
							if (GAME_VIGN.enabled.value && sp_VIGN) {
								GAME_VIGN.mode.Override(VignetteMode.Classic);
								GUILayout.BeginHorizontal();
								GUILayout.Label("Intensity: " + GAME_VIGN.intensity.value);
								GAME_VIGN.intensity.Override(GUILayout.HorizontalSlider(GAME_VIGN.intensity.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Smoothness: " + GAME_VIGN.smoothness.value);
								GAME_VIGN.smoothness.Override(GUILayout.HorizontalSlider(GAME_VIGN.smoothness.value, 0.1f, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GUILayout.Label("Roundness: " + GAME_VIGN.roundness.value);
								GAME_VIGN.roundness.Override(GUILayout.HorizontalSlider(GAME_VIGN.roundness.value, 0, 1, sliderStyle, thumbStyle));
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
								GAME_VIGN.rounded.Override(GUILayout.Toggle(GAME_VIGN.rounded.value, "Rounded"));
								if (GUILayout.Button("Reset")) {
									GAME_VIGN = reset<Vignette>();
								}
								GUILayout.EndHorizontal();
							}
						}
						GUILayout.Label(separator, separatorStyle);
					}

					// link to repo?
					if (GUILayout.Button("by Babbo Elu")) {
						Application.OpenURL("http://github.com/andreamatt/BabboSettings");
					}
					GUILayout.Label(separator, separatorStyle);
					GUILayout.BeginHorizontal();
					if (GUILayout.Button("Save")) {
						Main.Save();
					}
					if (GUILayout.Button("Save and close")) {
						Close();
					}
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndScrollView();
		}

		private T reset<T>() where T : PostProcessEffectSettings {
			bool enabled = post_volume.profile.GetSetting<T>().enabled.value;
			post_volume.profile.RemoveSettings<T>();
			T eff = post_volume.profile.AddSettings<T>();
			eff.enabled.Override(enabled);
			return eff;
		}

		private void getSettings() {
			post_layer = Camera.main.GetComponent<PostProcessLayer>();
			if (post_layer == null) {
				log("Null post layer");
			}
			if (post_layer.enabled == false) {
				post_layer.enabled = true;
				log("post_layer was disabled,");
			}
			post_volume = FindObjectOfType<PostProcessVolume>();
			if (post_volume != null) {
				string not_found = "";
				if ((GAME_AO = post_volume.profile.GetSetting<AmbientOcclusion>()) == null) {
					not_found += "ao,";
					GAME_AO = post_volume.profile.AddSettings<AmbientOcclusion>();
					GAME_AO.enabled.Override(false);
				}
				if ((GAME_EXPO = post_volume.profile.GetSetting<AutoExposure>()) == null) {
					not_found += "expo,";
					GAME_EXPO = post_volume.profile.AddSettings<AutoExposure>();
					GAME_EXPO.enabled.Override(false);
				}
				if ((GAME_BLOOM = post_volume.profile.GetSetting<Bloom>()) == null) {
					not_found += "bloom,";
					GAME_BLOOM = post_volume.profile.AddSettings<Bloom>();
					GAME_BLOOM.enabled.Override(false);
				}
				if ((GAME_CA = post_volume.profile.GetSetting<ChromaticAberration>()) == null) {
					not_found += "ca,";
					GAME_CA = post_volume.profile.AddSettings<ChromaticAberration>();
					GAME_CA.enabled.Override(false);
				}
				if ((GAME_COLOR = post_volume.profile.GetSetting<ColorGrading>()) == null) {
					not_found += "color,";
					GAME_COLOR = post_volume.profile.AddSettings<ColorGrading>();
					GAME_COLOR.enabled.Override(false);
				}
				if ((GAME_DOF = post_volume.profile.GetSetting<DepthOfField>()) == null) {
					not_found += "dof,";
					GAME_DOF = post_volume.profile.AddSettings<DepthOfField>();
					GAME_DOF.enabled.Override(false);
				}
				if ((GAME_GRAIN = post_volume.profile.GetSetting<Grain>()) == null) {
					not_found += "grain,";
					GAME_GRAIN = post_volume.profile.AddSettings<Grain>();
					GAME_GRAIN.enabled.Override(false);
				}
				if ((GAME_BLUR = post_volume.profile.GetSetting<MotionBlur>()) == null) {
					not_found += "blur,";
					GAME_BLUR = post_volume.profile.AddSettings<MotionBlur>();
					GAME_BLUR.enabled.Override(false);
				}
				if ((GAME_LENS = post_volume.profile.GetSetting<LensDistortion>()) == null) {
					not_found += "lens,";
					GAME_LENS = post_volume.profile.AddSettings<LensDistortion>();
					GAME_LENS.enabled.Override(false);
				}
				if ((GAME_REFL = post_volume.profile.GetSetting<ScreenSpaceReflections>()) == null) {
					not_found += "refl,";
					GAME_REFL = post_volume.profile.AddSettings<ScreenSpaceReflections>();
					GAME_REFL.enabled.Override(false);
				}
				if ((GAME_VIGN = post_volume.profile.GetSetting<Vignette>()) == null) {
					not_found += "vign,";
					GAME_VIGN = post_volume.profile.AddSettings<Vignette>();
					GAME_VIGN.enabled.Override(false);
				}
				if (not_found.Length > 0) {
					log("Not found: " + not_found);
				}
			}
			else {
				log("Post_volume is null in getSettings");
			}

			GAME_FXAA = post_layer.fastApproximateAntialiasing;
			GAME_TAA = post_layer.temporalAntialiasing;
			GAME_SMAA = post_layer.subpixelMorphologicalAntialiasing;

			Preset map_preset = new Preset(SceneManager.GetActiveScene().name + " (Original)");
			SaveTo(map_preset);
			Main.presets[map_preset.name] = map_preset;
			Main.select(Main.settings.presetName);
			Apply(Main.selectedPreset);

			log("Done getSettings");
		}
	}
}
