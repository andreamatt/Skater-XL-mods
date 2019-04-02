using System;
using UnityEngine;

namespace BabboSettings {
	internal partial class SettingsGUI : MonoBehaviour {
		internal void SaveTo(Preset preset) {
			log("Saving to " + preset.name);
			try {
				if (preset == null) throw new Exception("preset is null");
				if (post_layer == null) throw new Exception("Post_layer is null");
				if (post_volume == null) throw new Exception("Post_volume is null");
				if (Camera.main == null) throw new Exception("maincamera is null");
				preset.ENABLE_POST = post_volume.enabled;
				preset.FOV = Camera.main.fieldOfView;
				preset.FOCUS_PLAYER = focus_player;
				preset.VSYNC = QualitySettings.vSyncCount;
				preset.SCREEN_MODE = (int)Screen.fullScreenMode;

				preset.AA_MODE = post_layer.antialiasingMode;
				preset.TAA_sharpness = post_layer.temporalAntialiasing.sharpness;
				preset.TAA_jitter = post_layer.temporalAntialiasing.jitterSpread;
				preset.TAA_stationary = post_layer.temporalAntialiasing.stationaryBlending;
				preset.TAA_motion = post_layer.temporalAntialiasing.motionBlending;
				preset.SMAA = DeepClone(GAME_SMAA);

				preset.AO = DeepClone(GAME_AO);
				preset.EXPO = DeepClone(GAME_EXPO);
				preset.BLOOM = DeepClone(GAME_BLOOM);
				preset.CA = DeepClone(GAME_CA);
				preset.COLOR_enabled = GAME_COLOR.enabled.value;
				preset.COLOR_temperature = GAME_COLOR.temperature.value;
				preset.COLOR_post_exposure = GAME_COLOR.postExposure.value;
				preset.COLOR_saturation = GAME_COLOR.saturation.value;
				preset.COLOR_contrast = GAME_COLOR.contrast.value;
				preset.DOF = DeepClone(GAME_DOF);
				preset.GRAIN = DeepClone(GAME_GRAIN);
				preset.LENS = DeepClone(GAME_LENS);
				preset.BLUR = DeepClone(GAME_BLUR);
				preset.REFL = DeepClone(GAME_REFL);
				preset.VIGN = DeepClone(GAME_VIGN);

				preset.Save();
			}
			catch (Exception e) {
				throw new Exception("Failed saving settingsGUI2, ex: " + e);
			}
			log("Saved to " + preset.name);
		}

		internal void Apply(Preset preset) {
			log("Applying " + preset.name);
			// Post in general
			{
				post_volume.enabled = preset.ENABLE_POST;
			}

			// Field Of View
			{
				Camera.main.fieldOfView = preset.FOV;
			}

			// Vsync
			{
				preset.VSYNC = QualitySettings.vSyncCount;
			}

			// Fullscreen 
			{
				preset.SCREEN_MODE = (int)Screen.fullScreenMode;
			}

			// AntiAliasing
			{
				post_layer.antialiasingMode = preset.AA_MODE;
				GAME_SMAA.quality = preset.SMAA.quality;
				GAME_TAA.sharpness = preset.TAA_sharpness;
				GAME_TAA.jitterSpread = preset.TAA_jitter;
				GAME_TAA.stationaryBlending = preset.TAA_stationary;
				GAME_TAA.motionBlending = preset.TAA_motion;
			}

			// Ambient Occlusion
			{
				GAME_AO.enabled.Override(preset.AO.enabled.value);
				GAME_AO.intensity.Override(preset.AO.intensity.value);
				GAME_AO.quality.Override(preset.AO.quality.value);
				GAME_AO.mode.Override(preset.AO.mode.value);
			}

			// Automatic Exposure
			{
				GAME_EXPO.enabled.Override(preset.EXPO.enabled.value);
				GAME_EXPO.keyValue.Override(preset.EXPO.keyValue.value);
			}

			// Bloom
			{
				GAME_BLOOM.enabled.Override(preset.BLOOM.enabled.value);
				GAME_BLOOM.intensity.Override(preset.BLOOM.intensity.value);
				GAME_BLOOM.fastMode.Override(preset.BLOOM.fastMode.value);
			}

			// Chromatic aberration
			{
				GAME_CA.enabled.Override(preset.CA.enabled.value);
				GAME_CA.intensity.Override(preset.CA.intensity.value);
				GAME_CA.fastMode.Override(preset.CA.fastMode.value);
			}

			// Color Grading
			{
				GAME_COLOR.enabled.Override(preset.COLOR_enabled);
				GAME_COLOR.temperature.Override(preset.COLOR_temperature);
				GAME_COLOR.postExposure.Override(preset.COLOR_post_exposure);
				GAME_COLOR.saturation.Override(preset.COLOR_saturation);
				GAME_COLOR.contrast.Override(preset.COLOR_contrast);
			}

			// Depth Of Field
			{
				GAME_DOF.enabled.Override(preset.DOF.enabled.value);
				GAME_DOF.focusDistance.Override(preset.DOF.focusDistance.value);
				GAME_DOF.aperture.Override(preset.DOF.aperture.value);
				GAME_DOF.focalLength.Override(preset.DOF.focalLength.value);
				focus_player = preset.FOCUS_PLAYER;
			}

			// Grain
			{
				GAME_GRAIN.enabled.Override(preset.GRAIN.enabled.value);
				GAME_GRAIN.colored.Override(preset.GRAIN.colored.value);
				GAME_GRAIN.intensity.Override(preset.GRAIN.intensity.value);
				GAME_GRAIN.size.Override(preset.GRAIN.size.value);
				GAME_GRAIN.lumContrib.Override(preset.GRAIN.lumContrib.value);
			}

			// Lens Distortion
			{
				GAME_LENS.enabled.Override(preset.LENS.enabled.value);
				GAME_LENS.intensity.Override(preset.LENS.intensity.value);
				GAME_LENS.intensityX.Override(preset.LENS.intensityX.value);
				GAME_LENS.intensityY.Override(preset.LENS.intensityY.value);
				GAME_LENS.scale.Override(preset.LENS.scale.value);
			}

			// Motion Blur
			{
				GAME_BLUR.enabled.Override(preset.BLUR.enabled.value);
				GAME_BLUR.shutterAngle.Override(preset.BLUR.shutterAngle.value);
				GAME_BLUR.sampleCount.Override(preset.BLUR.sampleCount.value);
			}

			// Screen Space Reflections
			{
				GAME_REFL.enabled.Override(preset.REFL.enabled.value);
				GAME_REFL.preset.Override(preset.REFL.preset.value);
			}

			// Vignette
			{
				GAME_VIGN.enabled.Override(preset.VIGN.enabled.value);
				GAME_VIGN.mode.Override(preset.VIGN.mode.value);
				GAME_VIGN.intensity.Override(preset.VIGN.intensity.value);
				GAME_VIGN.smoothness.Override(preset.VIGN.smoothness.value);
				GAME_VIGN.roundness.Override(preset.VIGN.roundness.value);
				GAME_VIGN.rounded.Override(preset.VIGN.rounded.value);
			}

			log("Applied " + preset.name);
		}
	}
}