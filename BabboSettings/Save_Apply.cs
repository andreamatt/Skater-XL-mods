using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings {
	internal partial class SettingsGUI : MonoBehaviour {
		internal void SaveToSettings() {
			if (Main.settings.DEBUG) log("Saving to settings");
			try {
				if (post_layer == null) throw new Exception("Post_layer is null");
				if (post_volume == null) throw new Exception("Post_volume is null");
				if (Camera.main == null) throw new Exception("maincamera is null");

				Main.settings.ENABLE_POST = post_volume.enabled;
				Main.settings.VSYNC = QualitySettings.vSyncCount;
				Main.settings.SCREEN_MODE = (int)Screen.fullScreenMode;

				Main.settings.AA_MODE = post_layer.antialiasingMode;
				Main.settings.TAA_sharpness = post_layer.temporalAntialiasing.sharpness;
				Main.settings.TAA_jitter = post_layer.temporalAntialiasing.jitterSpread;
				Main.settings.TAA_stationary = post_layer.temporalAntialiasing.stationaryBlending;
				Main.settings.TAA_motion = post_layer.temporalAntialiasing.motionBlending;
				Main.settings.SMAA = DeepClone(GAME_SMAA);

				Main.settings.CAMERA = customCameraController.cameraMode;
				Main.settings.NORMAL_FOV = customCameraController.normal_fov;
				Main.settings.NORMAL_REACT = customCameraController.normal_react;
				Main.settings.NORMAL_REACT_ROT = customCameraController.normal_react_rot;
				Main.settings.NORMAL_CLIP = customCameraController.normal_clip;
				Main.settings.FOLLOW_FOV = customCameraController.follow_fov;
				Main.settings.FOLLOW_REACT = customCameraController.follow_react;
				Main.settings.FOLLOW_REACT_ROT = customCameraController.follow_react_rot;
				Main.settings.FOLLOW_CLIP = customCameraController.follow_clip;
				Main.settings.FOLLOW_SHIFT = customCameraController.follow_shift;
				Main.settings.POV_FOV = customCameraController.pov_fov;
				Main.settings.POV_REACT = customCameraController.pov_react;
				Main.settings.POV_REACT_ROT = customCameraController.pov_react_rot;
				Main.settings.POV_CLIP = customCameraController.pov_clip;
				Main.settings.POV_SHIFT = customCameraController.pov_shift;
				Main.settings.SKATE_FOV = customCameraController.skate_fov;
				Main.settings.SKATE_REACT = customCameraController.skate_react;
				Main.settings.SKATE_REACT_ROT = customCameraController.skate_react_rot;
				Main.settings.SKATE_CLIP = customCameraController.skate_clip;
				Main.settings.SKATE_SHIFT = customCameraController.skate_shift;

				Main.settings.Save();
			}
			catch (Exception e) {
				throw new Exception("Failed saving to settings, ex: " + e);
			}
			if (Main.settings.DEBUG) log("Saved settings");
		}

		private void UpdateMapPreset(Preset preset) {
			if (Main.settings.DEBUG) log("Saving to " + preset.name);
			try {
				if (preset == null) throw new Exception("preset is null");
				if (post_layer == null) throw new Exception("Post_layer is null");
				if (post_volume == null) throw new Exception("Post_volume is null");
				if (Camera.main == null) throw new Exception("maincamera is null");

				preset.AO = DeepClone(GAME_AO);
				preset.EXPO = DeepClone(GAME_EXPO);
				preset.BLOOM = DeepClone(GAME_BLOOM);
				preset.CA = DeepClone(GAME_CA);
				preset.COLOR_enabled = GAME_COLOR.enabled.value;
				preset.COLOR_tonemapper = GAME_COLOR.tonemapper.value;
				preset.COLOR_temperature = GAME_COLOR.temperature.value;
				preset.COLOR_tint = GAME_COLOR.tint.value;
				preset.COLOR_postExposure = GAME_COLOR.postExposure.value;
				preset.COLOR_hueShift = GAME_COLOR.hueShift.value;
				preset.COLOR_saturation = GAME_COLOR.saturation.value;
				preset.COLOR_contrast = GAME_COLOR.contrast.value;
				preset.COLOR_lift = GAME_COLOR.lift.value;
				preset.COLOR_gamma = GAME_COLOR.gamma.value;
				preset.COLOR_gain = GAME_COLOR.gain.value;
				preset.DOF = DeepClone(GAME_DOF);
				preset.FOCUS_MODE = focus_mode;
				preset.GRAIN = DeepClone(GAME_GRAIN);
				preset.LENS = DeepClone(GAME_LENS);
				preset.BLUR = DeepClone(GAME_BLUR);
				preset.REFL = DeepClone(GAME_REFL);
				preset.VIGN = DeepClone(GAME_VIGN);

				preset.Save();
			}
			catch (Exception e) {
				throw new Exception("Failed saving to preset, ex: " + e);
			}
			if (Main.settings.DEBUG) log("Saved to " + preset.name);
		}

		internal void ApplySettings() {
			if (Main.settings.DEBUG) log("Applying settings");
			// Basic
			{
				post_volume.enabled = Main.settings.ENABLE_POST;
				QualitySettings.vSyncCount = Main.settings.VSYNC;
				Screen.fullScreenMode = (FullScreenMode)Main.settings.SCREEN_MODE;

				// AntiAliasing
				{
					post_layer.antialiasingMode = Main.settings.AA_MODE;
					GAME_SMAA.quality = Main.settings.SMAA.quality;
					GAME_TAA.sharpness = Main.settings.TAA_sharpness;
					GAME_TAA.jitterSpread = Main.settings.TAA_jitter;
					GAME_TAA.stationaryBlending = Main.settings.TAA_stationary;
					GAME_TAA.motionBlending = Main.settings.TAA_motion;
				}
			}

			// Camera
			{
				customCameraController.cameraMode = Main.settings.CAMERA;
				customCameraController.normal_fov = Main.settings.NORMAL_FOV;
				customCameraController.normal_react = Main.settings.NORMAL_REACT;
				customCameraController.normal_react_rot = Main.settings.NORMAL_REACT_ROT;
				customCameraController.normal_clip = Main.settings.NORMAL_CLIP;
				customCameraController.follow_fov = Main.settings.FOLLOW_FOV;
				customCameraController.follow_react = Main.settings.FOLLOW_REACT;
				customCameraController.follow_react_rot = Main.settings.FOLLOW_REACT_ROT;
				customCameraController.follow_clip = Main.settings.FOLLOW_CLIP;
				customCameraController.follow_shift = Main.settings.FOLLOW_SHIFT;
				customCameraController.pov_fov = Main.settings.POV_FOV;
				customCameraController.pov_react = Main.settings.POV_REACT;
				customCameraController.pov_react_rot = Main.settings.POV_REACT_ROT;
				customCameraController.pov_clip = Main.settings.POV_CLIP;
				customCameraController.pov_shift = Main.settings.POV_SHIFT;
				customCameraController.skate_fov = Main.settings.SKATE_FOV;
				customCameraController.skate_react = Main.settings.SKATE_REACT;
				customCameraController.skate_react_rot = Main.settings.SKATE_REACT_ROT;
				customCameraController.skate_clip = Main.settings.SKATE_CLIP;
				customCameraController.skate_shift = Main.settings.SKATE_SHIFT;
			}

			if (Main.settings.DEBUG) log("Applied settings");
		}

		internal void ApplyPresets() {
			GAME_AO.enabled.Override(false);
			GAME_EXPO.enabled.Override(false);
			GAME_BLOOM.enabled.Override(false);
			GAME_CA.enabled.Override(false);
			GAME_COLOR.enabled.Override(false);
			GAME_DOF.enabled.Override(false);
			GAME_GRAIN.enabled.Override(false);
			GAME_LENS.enabled.Override(false);
			GAME_BLUR.enabled.Override(false);
			GAME_REFL.enabled.Override(false);
			GAME_VIGN.enabled.Override(false);
			for (int i = Main.settings.presetOrder.Count - 1; i >= 0; i--) {
				var preset = Main.presets[Main.settings.presetOrder[i]];
				if (preset.enabled) ApplyPreset(preset);
			}
		}

		private void ApplyPreset(Preset preset) {
			if (Main.settings.DEBUG) log("Applying " + preset.name);

			// Ambient Occlusion
			if (preset.AO.enabled.value) {
				GAME_AO.enabled.Override(preset.AO.enabled.value);
				GAME_AO.intensity.Override(preset.AO.intensity.value);
				GAME_AO.quality.Override(preset.AO.quality.value);
				GAME_AO.mode.Override(preset.AO.mode.value);
			}

			// Automatic Exposure
			if (preset.EXPO.enabled.value) {
				GAME_EXPO.enabled.Override(preset.EXPO.enabled.value);
				GAME_EXPO.keyValue.Override(preset.EXPO.keyValue.value);
			}

			// Bloom
			if (preset.BLOOM.enabled.value) {
				GAME_BLOOM.enabled.Override(preset.BLOOM.enabled.value);
				GAME_BLOOM.intensity.Override(preset.BLOOM.intensity.value);
				GAME_BLOOM.threshold.Override(preset.BLOOM.threshold.value);
				GAME_BLOOM.diffusion.Override(preset.BLOOM.diffusion.value);
				GAME_BLOOM.fastMode.Override(preset.BLOOM.fastMode.value);
			}

			// Chromatic aberration
			if (preset.CA.enabled.value) {
				GAME_CA.enabled.Override(preset.CA.enabled.value);
				GAME_CA.intensity.Override(preset.CA.intensity.value);
				GAME_CA.fastMode.Override(preset.CA.fastMode.value);
			}

			// Color Grading
			if (preset.COLOR_enabled) {
				GAME_COLOR.gradingMode.Override(GradingMode.HighDefinitionRange);
				GAME_COLOR.enabled.Override(preset.COLOR_enabled);
				GAME_COLOR.tonemapper.Override(preset.COLOR_tonemapper);
				GAME_COLOR.temperature.Override(preset.COLOR_temperature);
				GAME_COLOR.tint.Override(preset.COLOR_tint);
				GAME_COLOR.postExposure.Override(preset.COLOR_postExposure);
				GAME_COLOR.hueShift.Override(preset.COLOR_hueShift);
				GAME_COLOR.saturation.Override(preset.COLOR_saturation);
				GAME_COLOR.contrast.Override(preset.COLOR_contrast);
				GAME_COLOR.lift.Override(preset.COLOR_lift);
				GAME_COLOR.gamma.Override(preset.COLOR_gamma);
				GAME_COLOR.gain.Override(preset.COLOR_gain);
			}

			// Depth Of Field
			if (preset.DOF.enabled.value) {
				GAME_DOF.enabled.Override(preset.DOF.enabled.value);
				GAME_DOF.focusDistance.Override(preset.DOF.focusDistance.value);
				GAME_DOF.aperture.Override(preset.DOF.aperture.value);
				GAME_DOF.focalLength.Override(preset.DOF.focalLength.value);
				GAME_DOF.kernelSize.Override(preset.DOF.kernelSize.value);
				focus_mode = preset.FOCUS_MODE;
			}

			// Grain
			if (preset.GRAIN.enabled.value) {
				GAME_GRAIN.enabled.Override(preset.GRAIN.enabled.value);
				GAME_GRAIN.colored.Override(preset.GRAIN.colored.value);
				GAME_GRAIN.intensity.Override(preset.GRAIN.intensity.value);
				GAME_GRAIN.size.Override(preset.GRAIN.size.value);
				GAME_GRAIN.lumContrib.Override(preset.GRAIN.lumContrib.value);
			}

			// Lens Distortion
			if (preset.LENS.enabled.value) {
				GAME_LENS.enabled.Override(preset.LENS.enabled.value);
				GAME_LENS.intensity.Override(preset.LENS.intensity.value);
				GAME_LENS.intensityX.Override(preset.LENS.intensityX.value);
				GAME_LENS.intensityY.Override(preset.LENS.intensityY.value);
				GAME_LENS.scale.Override(preset.LENS.scale.value);
			}

			// Motion Blur
			if (preset.BLUR.enabled.value) {
				GAME_BLUR.enabled.Override(preset.BLUR.enabled.value);
				GAME_BLUR.shutterAngle.Override(preset.BLUR.shutterAngle.value);
				GAME_BLUR.sampleCount.Override(preset.BLUR.sampleCount.value);
			}

			// Screen Space Reflections
			if (preset.REFL.enabled.value) {
				GAME_REFL.enabled.Override(preset.REFL.enabled.value);
				GAME_REFL.preset.Override(preset.REFL.preset.value);
			}

			// Vignette
			if (preset.VIGN.enabled.value) {
				GAME_VIGN.enabled.Override(preset.VIGN.enabled.value);
				GAME_VIGN.mode.Override(VignetteMode.Classic);
				GAME_VIGN.intensity.Override(preset.VIGN.intensity.value);
				GAME_VIGN.smoothness.Override(preset.VIGN.smoothness.value);
				GAME_VIGN.roundness.Override(preset.VIGN.roundness.value);
				GAME_VIGN.rounded.Override(preset.VIGN.rounded.value);
			}

			if (Main.settings.DEBUG) log("Applied " + preset.name);
		}
	}
}