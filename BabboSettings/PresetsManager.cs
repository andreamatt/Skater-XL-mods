using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
    public sealed class PresetsManager
    {
        #region Singleton
        private static readonly Lazy<PresetsManager> _Instance = new Lazy<PresetsManager>(() => new PresetsManager());

        private PresetsManager() { }

        public static PresetsManager Instance {
            get => _Instance.Value;
        }
        #endregion

        private GameEffects effects { get => GameEffects.Instance; }
        private CustomCameraController customCameraController { get => CustomCameraController.Instance; }

        public void ApplyPresets() {
            effects.AO.enabled.Override(false);
            effects.EXPO.enabled.Override(false);
            effects.BLOOM.enabled.Override(false);
            effects.CA.enabled.Override(false);
            effects.COLOR.enabled.Override(false);
            effects.DOF.enabled.Override(false);
            effects.GRAIN.enabled.Override(false);
            effects.LENS.enabled.Override(false);
            effects.BLUR.enabled.Override(false);
            effects.REFL.enabled.Override(false);
            effects.VIGN.enabled.Override(false);
            for (int i = Main.settings.presetOrder.Count - 1; i >= 0; i--) {
                var preset = Main.presets[Main.settings.presetOrder[i]];
                if (preset.enabled) ApplyPreset(preset);
            }
        }

        private void ApplyPreset(Preset preset) {
            Logger.Debug("Applying " + preset.name);

            // Ambient Occlusion
            if (preset.AO.enabled.value) {
                effects.AO.enabled.Override(preset.AO.enabled.value);
                effects.AO.intensity.Override(preset.AO.intensity.value);
                effects.AO.quality.Override(preset.AO.quality.value);
                effects.AO.mode.Override(preset.AO.mode.value);
            }

            // Automatic Exposure
            if (preset.EXPO.enabled.value) {
                effects.EXPO.enabled.Override(preset.EXPO.enabled.value);
                effects.EXPO.keyValue.Override(preset.EXPO.keyValue.value);
            }

            // Bloom
            if (preset.BLOOM.enabled.value) {
                effects.BLOOM.enabled.Override(preset.BLOOM.enabled.value);
                effects.BLOOM.intensity.Override(preset.BLOOM.intensity.value);
                effects.BLOOM.threshold.Override(preset.BLOOM.threshold.value);
                effects.BLOOM.diffusion.Override(preset.BLOOM.diffusion.value);
                effects.BLOOM.fastMode.Override(preset.BLOOM.fastMode.value);
            }

            // Chromatic aberration
            if (preset.CA.enabled.value) {
                effects.CA.enabled.Override(preset.CA.enabled.value);
                effects.CA.intensity.Override(preset.CA.intensity.value);
                effects.CA.fastMode.Override(preset.CA.fastMode.value);
            }

            // Color Grading
            if (preset.COLOR.enabled) {
                effects.COLOR.gradingMode.Override(GradingMode.HighDefinitionRange);
                effects.COLOR.enabled.Override(preset.COLOR.enabled.value);
                effects.COLOR.tonemapper.Override(preset.COLOR.tonemapper.value);
                effects.COLOR.temperature.Override(preset.COLOR.temperature.value);
                effects.COLOR.tint.Override(preset.COLOR.tint.value);
                effects.COLOR.postExposure.Override(preset.COLOR.postExposure.value);
                effects.COLOR.hueShift.Override(preset.COLOR.hueShift.value);
                effects.COLOR.saturation.Override(preset.COLOR.saturation.value);
                effects.COLOR.contrast.Override(preset.COLOR.contrast.value);
                effects.COLOR.lift.Override(preset.COLOR.lift.value);
                effects.COLOR.gamma.Override(preset.COLOR.gamma.value);
                effects.COLOR.gain.Override(preset.COLOR.gain.value);
            }

            // Depth Of Field
            if (preset.DOF.enabled.value) {
                effects.DOF.enabled.Override(preset.DOF.enabled.value);
                effects.DOF.focusDistance.Override(preset.DOF.focusDistance.value);
                effects.DOF.aperture.Override(preset.DOF.aperture.value);
                effects.DOF.focalLength.Override(preset.DOF.focalLength.value);
                effects.DOF.kernelSize.Override(preset.DOF.kernelSize.value);
                effects.focus_mode = preset.FOCUS_MODE;
            }

            // Grain
            if (preset.GRAIN.enabled.value) {
                effects.GRAIN.enabled.Override(preset.GRAIN.enabled.value);
                effects.GRAIN.colored.Override(preset.GRAIN.colored.value);
                effects.GRAIN.intensity.Override(preset.GRAIN.intensity.value);
                effects.GRAIN.size.Override(preset.GRAIN.size.value);
                effects.GRAIN.lumContrib.Override(preset.GRAIN.lumContrib.value);
            }

            // Lens Distortion
            if (preset.LENS.enabled.value) {
                effects.LENS.enabled.Override(preset.LENS.enabled.value);
                effects.LENS.intensity.Override(preset.LENS.intensity.value);
                effects.LENS.intensityX.Override(preset.LENS.intensityX.value);
                effects.LENS.intensityY.Override(preset.LENS.intensityY.value);
                effects.LENS.scale.Override(preset.LENS.scale.value);
            }

            // Motion Blur
            if (preset.BLUR.enabled.value) {
                effects.BLUR.enabled.Override(preset.BLUR.enabled.value);
                effects.BLUR.shutterAngle.Override(preset.BLUR.shutterAngle.value);
                effects.BLUR.sampleCount.Override(preset.BLUR.sampleCount.value);
            }

            // Screen Space Reflections
            if (preset.REFL.enabled.value) {
                effects.REFL.enabled.Override(preset.REFL.enabled.value);
                effects.REFL.preset.Override(preset.REFL.preset.value);
            }

            // Vignette
            if (preset.VIGN.enabled.value) {
                effects.VIGN.enabled.Override(preset.VIGN.enabled.value);
                effects.VIGN.mode.Override(VignetteMode.Classic);
                effects.VIGN.intensity.Override(preset.VIGN.intensity.value);
                effects.VIGN.smoothness.Override(preset.VIGN.smoothness.value);
                effects.VIGN.roundness.Override(preset.VIGN.roundness.value);
                effects.VIGN.rounded.Override(preset.VIGN.rounded.value);
            }

            Logger.Debug("Applied " + preset.name);
        }

        public void SaveToSettings() {
            Logger.Debug("Saving to settings");
            try {
                if (effects.post_layer == null) throw new Exception("Post_layer is null");
                if (effects.post_volume == null) throw new Exception("Post_volume is null");
                if (Camera.main == null) throw new Exception("maincamera is null");

                Main.settings.ENABLE_POST = effects.post_volume.enabled;
                Main.settings.VSYNC = QualitySettings.vSyncCount;
                Main.settings.SCREEN_MODE = (int)Screen.fullScreenMode;

                Main.settings.AA_MODE = effects.post_layer.antialiasingMode;
                Main.settings.TAA_sharpness = effects.post_layer.temporalAntialiasing.sharpness;
                Main.settings.TAA_jitter = effects.post_layer.temporalAntialiasing.jitterSpread;
                Main.settings.TAA_stationary = effects.post_layer.temporalAntialiasing.stationaryBlending;
                Main.settings.TAA_motion = effects.post_layer.temporalAntialiasing.motionBlending;
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(effects.SMAA), Main.settings.SMAA);

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
            Logger.Debug("Saved settings");
        }

        public void ApplySettings() {
            Logger.Debug("Applying settings");
            // Basic
            {
                effects.post_volume.enabled = Main.settings.ENABLE_POST;
                QualitySettings.vSyncCount = Main.settings.VSYNC;
                Screen.fullScreenMode = (FullScreenMode)Main.settings.SCREEN_MODE;

                // AntiAliasing
                {
                    effects.post_layer.antialiasingMode = Main.settings.AA_MODE;
                    effects.SMAA.quality = Main.settings.SMAA.quality;
                    effects.TAA.sharpness = Main.settings.TAA_sharpness;
                    effects.TAA.jitterSpread = Main.settings.TAA_jitter;
                    effects.TAA.stationaryBlending = Main.settings.TAA_stationary;
                    effects.TAA.motionBlending = Main.settings.TAA_motion;
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

            Logger.Debug("Applied settings");
        }
    }
}
