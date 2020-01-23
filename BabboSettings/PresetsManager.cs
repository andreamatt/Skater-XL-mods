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
            // so if one gets unchecked it needs to disable even if not read
            gameEffects.AO.enabled.Override(false);
            gameEffects.EXPO.enabled.Override(false);
            gameEffects.BLOOM.enabled.Override(false);
            gameEffects.CA.enabled.Override(false);
            gameEffects.COLOR.enabled.Override(false);
            gameEffects.DOF.enabled.Override(false);
            gameEffects.GRAIN.enabled.Override(false);
            gameEffects.LENS.enabled.Override(false);
            gameEffects.BLUR.enabled.Override(false);
            gameEffects.REFL.enabled.Override(false);
            gameEffects.VIGN.enabled.Override(false);

            // disable FOV override
            cameraController.override_fov = false;

            if (BabboSettings.Instance.IsReplayActive()) {
                for (int i = Main.settings.replay_presetOrder.Count - 1; i >= 0; i--) {
                    var preset = Main.presets[Main.settings.replay_presetOrder[i]];
                    if (preset.replay_enabled) ApplyPreset(preset);
                }
            }
            else {
                for (int i = Main.settings.presetOrder.Count - 1; i >= 0; i--) {
                    var preset = Main.presets[Main.settings.presetOrder[i]];
                    if (preset.enabled) ApplyPreset(preset);
                }
            }
        }

        private void ApplyPreset(Preset preset) {
            // FOV Override
            if (preset.OVERRIDE_FOV) {
                cameraController.override_fov = true;
                cameraController.override_fov_value = preset.OVERRIDE_FOV_VALUE;
            }

            // Ambient Occlusion
            if (preset.AO.enabled.value) {
                gameEffects.AO.enabled.Override(preset.AO.enabled.value);
                gameEffects.AO.intensity.Override(preset.AO.intensity.value);
                gameEffects.AO.quality.Override(preset.AO.quality.value);
                gameEffects.AO.mode.Override(preset.AO.mode.value);
            }

            // Automatic Exposure
            if (preset.EXPO.enabled.value) {
                gameEffects.EXPO.enabled.Override(preset.EXPO.enabled.value);
                gameEffects.EXPO.keyValue.Override(preset.EXPO.keyValue.value);
            }

            // Bloom
            if (preset.BLOOM.enabled.value) {
                gameEffects.BLOOM.enabled.Override(preset.BLOOM.enabled.value);
                gameEffects.BLOOM.intensity.Override(preset.BLOOM.intensity.value);
                gameEffects.BLOOM.threshold.Override(preset.BLOOM.threshold.value);
                gameEffects.BLOOM.diffusion.Override(preset.BLOOM.diffusion.value);
                gameEffects.BLOOM.fastMode.Override(preset.BLOOM.fastMode.value);
            }

            // Chromatic aberration
            if (preset.CA.enabled.value) {
                gameEffects.CA.enabled.Override(preset.CA.enabled.value);
                gameEffects.CA.intensity.Override(preset.CA.intensity.value);
                gameEffects.CA.fastMode.Override(preset.CA.fastMode.value);
            }

            // Color Grading
            if (preset.COLOR.enabled) {
                gameEffects.COLOR.gradingMode.Override(GradingMode.HighDefinitionRange);
                gameEffects.COLOR.enabled.Override(preset.COLOR.enabled.value);
                gameEffects.COLOR.tonemapper.Override(preset.COLOR.tonemapper.value);
                gameEffects.COLOR.temperature.Override(preset.COLOR.temperature.value);
                gameEffects.COLOR.tint.Override(preset.COLOR.tint.value);
                gameEffects.COLOR.postExposure.Override(preset.COLOR.postExposure.value);
                gameEffects.COLOR.hueShift.Override(preset.COLOR.hueShift.value);
                gameEffects.COLOR.saturation.Override(preset.COLOR.saturation.value);
                gameEffects.COLOR.contrast.Override(preset.COLOR.contrast.value);
                gameEffects.COLOR.lift.Override(preset.COLOR.lift.value);
                gameEffects.COLOR.gamma.Override(preset.COLOR.gamma.value);
                gameEffects.COLOR.gain.Override(preset.COLOR.gain.value);
            }

            // Depth Of Field
            if (preset.DOF.enabled.value) {
                gameEffects.DOF.enabled.Override(preset.DOF.enabled.value);
                gameEffects.focus_mode = preset.FOCUS_MODE;
                if (preset.FOCUS_MODE == FocusMode.Custom) {// if it is player/skate it gets set during lateupdate (instead of onGUI)
                    gameEffects.DOF.focusDistance.Override(preset.DOF.focusDistance.value);
                }
                gameEffects.DOF.aperture.Override(preset.DOF.aperture.value);
                gameEffects.DOF.focalLength.Override(preset.DOF.focalLength.value);
                gameEffects.DOF.kernelSize.Override(preset.DOF.kernelSize.value);
            }

            // Grain
            if (preset.GRAIN.enabled.value) {
                gameEffects.GRAIN.enabled.Override(preset.GRAIN.enabled.value);
                gameEffects.GRAIN.colored.Override(preset.GRAIN.colored.value);
                gameEffects.GRAIN.intensity.Override(preset.GRAIN.intensity.value);
                gameEffects.GRAIN.size.Override(preset.GRAIN.size.value);
                gameEffects.GRAIN.lumContrib.Override(preset.GRAIN.lumContrib.value);
            }

            // Lens Distortion
            if (preset.LENS.enabled.value) {
                gameEffects.LENS.enabled.Override(preset.LENS.enabled.value);
                gameEffects.LENS.intensity.Override(preset.LENS.intensity.value);
                gameEffects.LENS.intensityX.Override(preset.LENS.intensityX.value);
                gameEffects.LENS.intensityY.Override(preset.LENS.intensityY.value);
                gameEffects.LENS.scale.Override(preset.LENS.scale.value);
            }

            // Motion Blur
            if (preset.BLUR.enabled.value) {
                gameEffects.BLUR.enabled.Override(preset.BLUR.enabled.value);
                gameEffects.BLUR.shutterAngle.Override(preset.BLUR.shutterAngle.value);
                gameEffects.BLUR.sampleCount.Override(preset.BLUR.sampleCount.value);
            }

            // Screen Space Reflections
            if (preset.REFL.enabled.value) {
                gameEffects.REFL.enabled.Override(preset.REFL.enabled.value);
                gameEffects.REFL.preset.Override(preset.REFL.preset.value);
            }

            // Vignette
            if (preset.VIGN.enabled.value) {
                gameEffects.VIGN.enabled.Override(preset.VIGN.enabled.value);
                gameEffects.VIGN.mode.Override(VignetteMode.Classic);
                gameEffects.VIGN.intensity.Override(preset.VIGN.intensity.value);
                gameEffects.VIGN.smoothness.Override(preset.VIGN.smoothness.value);
                gameEffects.VIGN.roundness.Override(preset.VIGN.roundness.value);
                gameEffects.VIGN.rounded.Override(preset.VIGN.rounded.value);
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
