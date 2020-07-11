using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
	public class PresetsManager : Module
	{
		public void ApplyPresets() {
			// disable all of them, because only enabled ones are read and applied
			// so if one gets unchecked it needs to be disabled even if is not read
			foreach (var suite in new EffectSuite[] { gameEffects.effectSuite, gameEffects.map_effectSuite }) {
				suite.AO.enabled.Override(false);
				suite.EXPO.enabled.Override(false);
				suite.BLOOM.enabled.Override(false);
				suite.CA.enabled.Override(false);
				suite.COLOR.enabled.Override(false);
				suite.DOF.enabled.Override(false);
				suite.GRAIN.enabled.Override(false);
				suite.LENS.enabled.Override(false);
				suite.BLUR.enabled.Override(false);
				suite.REFL.enabled.Override(false);
				suite.VIGN.enabled.Override(false);
			}

			// disable FOV override
			cameraController.override_fov = false;
			// disable camera light
			lightController.LIGHT_ENABLED = false;

			var replayOrder = BabboSettings.Instance.IsReplayActive() ? Main.settings.replay_presetOrder : Main.settings.presetOrder;
			if (replayOrder.map_enabled) ApplyPreset(Main.presets[Main.map_name]);
			for (int i = replayOrder.Count - 1; i >= 0; i--) {
				var name = replayOrder.names[i];
				var enabled = replayOrder.enables[i];
				var preset = Main.presets[name];
				if (enabled) ApplyPreset(preset);
			}
		}

		private void ApplyPreset(Preset preset) {
			var suite = preset.isMapPreset ? gameEffects.map_effectSuite : gameEffects.effectSuite;

			// FOV Override
			if (preset.OVERRIDE_FOV) {
				cameraController.override_fov = true;
				cameraController.override_fov_value = preset.OVERRIDE_FOV_VALUE;
			}

			// Light on camera
			if (preset.LIGHT_ENABLED) {
				lightController.LIGHT_ENABLED = true;
				lightController.LIGHT_RANGE = preset.LIGHT_RANGE;
				lightController.LIGHT_ANGLE = preset.LIGHT_ANGLE;
				lightController.LIGHT_INTENSITY = preset.LIGHT_INTENSITY;
				lightController.LIGHT_COLOR = preset.LIGHT_COLOR;
				lightController.LIGHT_POSITION = preset.LIGHT_POSITION;
				lightController.LIGHT_COOKIE = preset.LIGHT_COOKIE;
			}

			// Ambient Occlusion
			if (preset.AO.enabled.value) {
				suite.AO.enabled.Override(preset.AO.enabled.value);
				suite.AO.intensity.Override(preset.AO.intensity.value);
				suite.AO.quality.Override(preset.AO.quality.value);
				suite.AO.mode.Override(preset.AO.mode.value);
			}

			// Automatic Exposure
			if (preset.EXPO.enabled.value) {
				suite.EXPO.enabled.Override(preset.EXPO.enabled.value);
				suite.EXPO.keyValue.Override(preset.EXPO.keyValue.value);
			}

			// Bloom
			if (preset.BLOOM.enabled.value) {
				suite.BLOOM.enabled.Override(preset.BLOOM.enabled.value);
				suite.BLOOM.intensity.Override(preset.BLOOM.intensity.value);
				suite.BLOOM.threshold.Override(preset.BLOOM.threshold.value);
				suite.BLOOM.diffusion.Override(preset.BLOOM.diffusion.value);
				suite.BLOOM.fastMode.Override(preset.BLOOM.fastMode.value);
			}

			// Chromatic aberration
			if (preset.CA.enabled.value) {
				suite.CA.enabled.Override(preset.CA.enabled.value);
				suite.CA.intensity.Override(preset.CA.intensity.value);
				suite.CA.fastMode.Override(preset.CA.fastMode.value);
			}

			// Color Grading
			if (preset.COLOR.enabled) {
				suite.COLOR.gradingMode.Override(GradingMode.HighDefinitionRange);
				suite.COLOR.enabled.Override(preset.COLOR.enabled.value);
				suite.COLOR.tonemapper.Override(preset.COLOR.tonemapper.value);
				suite.COLOR.temperature.Override(preset.COLOR.temperature.value);
				suite.COLOR.tint.Override(preset.COLOR.tint.value);
				suite.COLOR.postExposure.Override(preset.COLOR.postExposure.value);
				suite.COLOR.hueShift.Override(preset.COLOR.hueShift.value);
				suite.COLOR.saturation.Override(preset.COLOR.saturation.value);
				suite.COLOR.contrast.Override(preset.COLOR.contrast.value);
				suite.COLOR.lift.Override(preset.COLOR.lift.value);
				suite.COLOR.gamma.Override(preset.COLOR.gamma.value);
				suite.COLOR.gain.Override(preset.COLOR.gain.value);
			}

			// Depth Of Field
			if (preset.DOF.enabled.value) {
				suite.DOF.enabled.Override(preset.DOF.enabled.value);
				gameEffects.focus_mode = preset.FOCUS_MODE;
				if (preset.FOCUS_MODE == FocusMode.Custom) {// if it is player/skate it gets set during lateupdate (instead of onGUI)
					suite.DOF.focusDistance.Override(preset.DOF.focusDistance.value);
				}
				suite.DOF.aperture.Override(preset.DOF.aperture.value);
				suite.DOF.focalLength.Override(preset.DOF.focalLength.value);
				suite.DOF.kernelSize.Override(preset.DOF.kernelSize.value);
			}

			// Grain
			if (preset.GRAIN.enabled.value) {
				suite.GRAIN.enabled.Override(preset.GRAIN.enabled.value);
				suite.GRAIN.colored.Override(preset.GRAIN.colored.value);
				suite.GRAIN.intensity.Override(preset.GRAIN.intensity.value);
				suite.GRAIN.size.Override(preset.GRAIN.size.value);
				suite.GRAIN.lumContrib.Override(preset.GRAIN.lumContrib.value);
			}

			// Lens Distortion
			if (preset.LENS.enabled.value) {
				suite.LENS.enabled.Override(preset.LENS.enabled.value);
				suite.LENS.intensity.Override(preset.LENS.intensity.value);
				suite.LENS.intensityX.Override(preset.LENS.intensityX.value);
				suite.LENS.intensityY.Override(preset.LENS.intensityY.value);
				suite.LENS.scale.Override(preset.LENS.scale.value);
			}

			// Motion Blur
			if (preset.BLUR.enabled.value) {
				suite.BLUR.enabled.Override(preset.BLUR.enabled.value);
				suite.BLUR.shutterAngle.Override(preset.BLUR.shutterAngle.value);
				suite.BLUR.sampleCount.Override(preset.BLUR.sampleCount.value);
			}

			// Screen Space Reflections
			if (preset.REFL.enabled.value) {
				suite.REFL.enabled.Override(preset.REFL.enabled.value);
				suite.REFL.preset.Override(preset.REFL.preset.value);
			}

			// Vignette
			if (preset.VIGN.enabled.value) {
				suite.VIGN.enabled.Override(preset.VIGN.enabled.value);
				suite.VIGN.mode.Override(VignetteMode.Classic);
				suite.VIGN.intensity.Override(preset.VIGN.intensity.value);
				suite.VIGN.smoothness.Override(preset.VIGN.smoothness.value);
				suite.VIGN.roundness.Override(preset.VIGN.roundness.value);
				suite.VIGN.rounded.Override(preset.VIGN.rounded.value);
			}

		}

		public void SaveToSettings() {
			Logger.Debug("Saving to settings");
			try {
				if (gameEffects.post_layer == null) throw new Exception("Post_layer is null");
				if (gameEffects.post_volume == null) throw new Exception("Post_volume is null");
				if (Camera.main == null) throw new Exception("maincamera is null");

				Main.settings.ENABLE_POST = gameEffects.post_volume.enabled;
				Main.settings.VSYNC = QualitySettings.vSyncCount;
				Main.settings.SCREEN_MODE = (int)Screen.fullScreenMode;
				Main.settings.SCREEN_RESOLUTION = Screen.currentResolution;

				Main.settings.AA_MODE = gameEffects.post_layer.antialiasingMode;
				Main.settings.TAA_sharpness = gameEffects.post_layer.temporalAntialiasing.sharpness;
				Main.settings.TAA_jitter = gameEffects.post_layer.temporalAntialiasing.jitterSpread;
				Main.settings.TAA_stationary = gameEffects.post_layer.temporalAntialiasing.stationaryBlending;
				Main.settings.TAA_motion = gameEffects.post_layer.temporalAntialiasing.motionBlending;
				JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(gameEffects.SMAA), Main.settings.SMAA);

				Main.settings.CAMERA = cameraController.cameraMode;
				Main.settings.REPLAY_FOV = cameraController.replay_fov;
				Main.settings.NORMAL_FOV = cameraController.normal_fov;
				Main.settings.NORMAL_REACT = cameraController.normal_react;
				Main.settings.NORMAL_REACT_ROT = cameraController.normal_react_rot;
				Main.settings.NORMAL_CLIP = cameraController.normal_clip;
				Main.settings.FOLLOW_FOV = cameraController.follow_fov;
				Main.settings.FOLLOW_REACT = cameraController.follow_react;
				Main.settings.FOLLOW_REACT_ROT = cameraController.follow_react_rot;
				Main.settings.FOLLOW_CLIP = cameraController.follow_clip;
				Main.settings.FOLLOW_SHIFT = cameraController.follow_shift;
				Main.settings.FOLLOW_AUTO_SWITCH = cameraController.follow_auto_switch;
				Main.settings.POV_FOV = cameraController.pov_fov;
				Main.settings.POV_REACT = cameraController.pov_react;
				Main.settings.POV_REACT_ROT = cameraController.pov_react_rot;
				Main.settings.POV_CLIP = cameraController.pov_clip;
				Main.settings.POV_SHIFT = cameraController.pov_shift;
				Main.settings.SKATE_FOV = cameraController.skate_fov;
				Main.settings.SKATE_REACT = cameraController.skate_react;
				Main.settings.SKATE_REACT_ROT = cameraController.skate_react_rot;
				Main.settings.SKATE_CLIP = cameraController.skate_clip;
				Main.settings.SKATE_SHIFT = cameraController.skate_shift;
			}
			catch (Exception e) {
				throw new Exception("Failed saving to settings, ex: " + e);
			}
			Logger.Debug("Saved settings");
		}

		public void ApplySettings() {
			Logger.Debug("Applying settings");
			// Basic
			{
				gameEffects.post_volume.enabled = Main.settings.ENABLE_POST;
				QualitySettings.vSyncCount = Main.settings.VSYNC;
				Screen.fullScreenMode = (FullScreenMode)Main.settings.SCREEN_MODE;
				var res = Main.settings.SCREEN_RESOLUTION;
				Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);

				// AntiAliasing
				{
					gameEffects.post_layer.antialiasingMode = Main.settings.AA_MODE;
					gameEffects.SMAA.quality = Main.settings.SMAA.quality;
					gameEffects.TAA.sharpness = Main.settings.TAA_sharpness;
					gameEffects.TAA.jitterSpread = Main.settings.TAA_jitter;
					gameEffects.TAA.stationaryBlending = Main.settings.TAA_stationary;
					gameEffects.TAA.motionBlending = Main.settings.TAA_motion;
				}
			}

			// Camera
			{
				cameraController.cameraMode = Main.settings.CAMERA;
				cameraController.replay_fov = Main.settings.REPLAY_FOV;
				cameraController.normal_fov = Main.settings.NORMAL_FOV;
				cameraController.normal_react = Main.settings.NORMAL_REACT;
				cameraController.normal_react_rot = Main.settings.NORMAL_REACT_ROT;
				cameraController.normal_clip = Main.settings.NORMAL_CLIP;
				cameraController.follow_fov = Main.settings.FOLLOW_FOV;
				cameraController.follow_react = Main.settings.FOLLOW_REACT;
				cameraController.follow_react_rot = Main.settings.FOLLOW_REACT_ROT;
				cameraController.follow_clip = Main.settings.FOLLOW_CLIP;
				cameraController.follow_shift = Main.settings.FOLLOW_SHIFT;
				cameraController.follow_auto_switch = Main.settings.FOLLOW_AUTO_SWITCH;
				cameraController.pov_fov = Main.settings.POV_FOV;
				cameraController.pov_react = Main.settings.POV_REACT;
				cameraController.pov_react_rot = Main.settings.POV_REACT_ROT;
				cameraController.pov_clip = Main.settings.POV_CLIP;
				cameraController.pov_shift = Main.settings.POV_SHIFT;
				cameraController.skate_fov = Main.settings.SKATE_FOV;
				cameraController.skate_react = Main.settings.SKATE_REACT;
				cameraController.skate_react_rot = Main.settings.SKATE_REACT_ROT;
				cameraController.skate_clip = Main.settings.SKATE_CLIP;
				cameraController.skate_shift = Main.settings.SKATE_SHIFT;
			}

			Logger.Debug("Applied settings");
		}
	}
}
