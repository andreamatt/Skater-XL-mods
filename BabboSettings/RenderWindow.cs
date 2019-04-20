using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

namespace BabboSettings {

    internal partial class SettingsGUI : MonoBehaviour {
        private string[] aa_names = { "None", "FXAA", "SMAA", "TAA" };
        private string[] smaa_quality = { "Low", "Medium", "High" };
        private string[] ao_quality = { "Lowest", "Low", "Medium", "High", "Ultra" };
        private string[] ao_mode = { "SAO", "MSVO" };
        private string[] refl_presets = { "Low", "Lower", "Medium", "High", "Higher", "Ultra", "Overkill" };
        private string[] vsync_names = { "Disabled", "Full", "Half" };
        private string[] screen_modes = { "Exclusive", "Full", "Maximized", "Windowed" };
        private string[] tonemappers = { "None", "Neutral", "ACES" };
        private string[] max_blur = { "Small", "Medium", "Large", "Very large" };
        private string[] focus_modes = { "Custom", "Player", "Skate" };
        private bool sp_AA, sp_AO, sp_EXPO, sp_BLOOM, sp_CA, sp_COLOR, sp_COLOR_ADV, sp_DOF, sp_GRAIN, sp_LENS, sp_BLUR, sp_REFL, sp_VIGN;
        private bool choosing_name, changing_preset;
        private string name_text = "";

        private enum SelectedTab {
            Basic,
            Effects,
            Camera
        }
        private SelectedTab selectedTab = SelectedTab.Basic;
        private string[] tab_names = { "Basic", "Effects", "Camera" };
        private string[] camera_names = { "Normal", "Low", "Follow", "POV", "Skate" };

        void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(750));
            {
                if (choosing_name) {
                    name_text = GUILayout.TextField(name_text);
                    BeginHorizontal();
                    if (Button("Confirm")) {
                        choosing_name = false;
                        if (Main.presets.ContainsKey(name_text)) {
                            log("Preset already exists");
                        }
                        Preset new_preset = new Preset(name_text);
                        SaveToPreset(new_preset);
                        Main.presets[new_preset.name] = new_preset;
                        Main.select(name_text);
                    }
                    if (Button("Cancel")) {
                        choosing_name = false;
                    }
                    EndHorizontal();
                }
                else if (changing_preset) {
                    foreach (var preset in Main.presets) {
                        if (Button(preset.Key)) {
                            SaveToPreset(Main.selectedPreset);
                            Main.select(preset.Key);
                            ApplyPreset(Main.selectedPreset);
                            changing_preset = false;
                        }
                    }
                    Separator();
                    if (Button("Cancel")) {
                        changing_preset = false;
                    }
                }
                else {
                    BeginHorizontal();
                    selectedTab = (SelectedTab)GUILayout.SelectionGrid((int)selectedTab, tab_names, tab_names.Length);
                    EndHorizontal();

                    if (selectedTab == SelectedTab.Basic) {
                        // Post in general
                        {
                            post_volume.enabled = GUILayout.Toggle(post_volume.enabled, "Enable post processing");
                        }
                        Separator();
                        // VSync
                        {
                            BeginHorizontal();
                            Label("Vsync");
                            QualitySettings.vSyncCount = GUILayout.SelectionGrid(QualitySettings.vSyncCount, vsync_names, vsync_names.Length);
                            EndHorizontal();
                        }
                        Separator();
                        // Fullscreen
                        {
                            BeginHorizontal();
                            Label("Fullscreen");
                            Screen.fullScreenMode = (FullScreenMode)GUILayout.SelectionGrid((int)Screen.fullScreenMode, screen_modes, screen_modes.Length);
                            EndHorizontal();
                        }
                        Separator();
                        // AntiAliasing
                        {
                            Label("AntiAliasing");
                            post_layer.antialiasingMode = (Antialiasing)(GUILayout.SelectionGrid((int)post_layer.antialiasingMode, aa_names, aa_names.Length));
                            if (post_layer.antialiasingMode == Antialiasing.SubpixelMorphologicalAntialiasing) {
                                sp_AA = Spoiler(sp_AA ? "hide" : "show") ? !sp_AA : sp_AA;
                                if (sp_AA) {
                                    GAME_SMAA.quality = (SubpixelMorphologicalAntialiasing.Quality)GUILayout.SelectionGrid((int)GAME_SMAA.quality, smaa_quality, smaa_quality.Length);
                                }
                            }
                            else if (post_layer.antialiasingMode == Antialiasing.TemporalAntialiasing) {
                                sp_AA = Spoiler(sp_AA ? "hide" : "show") ? !sp_AA : sp_AA;
                                if (sp_AA) {
                                    GAME_TAA.sharpness = Slider("Sharpness", GAME_TAA.sharpness, 0, 3);
                                    GAME_TAA.jitterSpread = Slider("Jitter spread", GAME_TAA.jitterSpread, 0, 1);
                                    GAME_TAA.stationaryBlending = Slider("Stationary blending", GAME_TAA.stationaryBlending, 0, 1);
                                    GAME_TAA.motionBlending = Slider("Motion Blending", GAME_TAA.motionBlending, 0, 1);
                                    if (Button("Default TAA")) {
                                        GAME_TAA = post_layer.temporalAntialiasing = new TemporalAntialiasing();
                                    }
                                }
                            }
                        }
                    }
                    else if (selectedTab == SelectedTab.Effects) {
                        if (post_volume.enabled) {
                            // Ambient Occlusion
                            {
                                BeginHorizontal();
                                GAME_AO.enabled.Override(GUILayout.Toggle(GAME_AO.enabled.value, "Ambient occlusion"));
                                if (GAME_AO.enabled.value) sp_AO = Spoiler(sp_AO ? "hide" : "show") ? !sp_AO : sp_AO;
                                EndHorizontal();
                                if (GAME_AO.enabled.value && sp_AO) {
                                    GAME_AO.intensity.Override(Slider("Intensity", GAME_AO.intensity.value, 0, 4));
                                    GAME_AO.quality.Override((AmbientOcclusionQuality)GUILayout.SelectionGrid((int)GAME_AO.quality.value, ao_quality, ao_quality.Length));
                                    GAME_AO.mode.Override((AmbientOcclusionMode)GUILayout.SelectionGrid((int)GAME_AO.mode.value, ao_mode, ao_mode.Length));
                                    if (Button("Reset")) GAME_AO = reset<AmbientOcclusion>();
                                }
                            }
                            Separator();
                            // Automatic Exposure
                            {
                                BeginHorizontal();
                                GAME_EXPO.enabled.Override(GUILayout.Toggle(GAME_EXPO.enabled.value, "Automatic Exposure"));
                                if (GAME_EXPO.enabled.value) sp_EXPO = Spoiler(sp_EXPO ? "hide" : "show") ? !sp_EXPO : sp_EXPO;
                                EndHorizontal();
                                if (GAME_EXPO.enabled.value && sp_EXPO) {
                                    GAME_EXPO.keyValue.Override(Slider("Compensation", GAME_EXPO.keyValue.value, 0, 4));
                                    if (Button("Reset")) GAME_EXPO = reset<AutoExposure>();
                                }
                            }
                            Separator();
                            // Bloom
                            {
                                BeginHorizontal();
                                GAME_BLOOM.enabled.Override(GUILayout.Toggle(GAME_BLOOM.enabled.value, "Bloom"));
                                if (GAME_BLOOM.enabled.value) sp_BLOOM = Spoiler(sp_BLOOM ? "hide" : "show") ? !sp_BLOOM : sp_BLOOM;
                                EndHorizontal();
                                if (GAME_BLOOM.enabled.value && sp_BLOOM) {
                                    GAME_BLOOM.intensity.Override(Slider("Intensity", GAME_BLOOM.intensity.value, 0, 4));
                                    GAME_BLOOM.threshold.Override(Slider("Threshold", GAME_BLOOM.threshold.value, 0, 4));
                                    GAME_BLOOM.diffusion.Override(Slider("Diffusion", GAME_BLOOM.diffusion.value, 1, 10));
                                    GAME_BLOOM.fastMode.Override(GUILayout.Toggle(GAME_BLOOM.fastMode.value, "Fast mode"));
                                    if (Button("Reset")) GAME_BLOOM = reset<Bloom>();
                                }
                            }
                            Separator();
                            // Chromatic aberration
                            {
                                BeginHorizontal();
                                GAME_CA.enabled.Override(GUILayout.Toggle(GAME_CA.enabled.value, "Chromatic aberration"));
                                if (GAME_CA.enabled.value) sp_CA = Spoiler(sp_CA ? "hide" : "show") ? !sp_CA : sp_CA;
                                EndHorizontal();
                                if (GAME_CA.enabled.value && sp_CA) {
                                    GAME_CA.intensity.Override(Slider("Intensity", GAME_CA.intensity.value, 0, 1));
                                    GAME_CA.fastMode.Override(GUILayout.Toggle(GAME_CA.fastMode.value, "Fast mode"));
                                    if (Button("Reset")) GAME_CA = reset<ChromaticAberration>();
                                }
                            }
                            Separator();
                            // Color Grading
                            {
                                BeginHorizontal();
                                GAME_COLOR.enabled.Override(GUILayout.Toggle(GAME_COLOR.enabled.value, "Color Grading"));
                                if (GAME_COLOR.enabled.value) sp_COLOR = Spoiler(sp_COLOR ? "hide" : "show") ? !sp_COLOR : sp_COLOR;
                                EndHorizontal();
                                if (GAME_COLOR.enabled.value && sp_COLOR) {
                                    BeginHorizontal();
                                    Label("Tonemapper: ");
                                    GAME_COLOR.tonemapper.Override((Tonemapper)GUILayout.SelectionGrid((int)GAME_COLOR.tonemapper.value, tonemappers, tonemappers.Length));
                                    EndHorizontal();
                                    GAME_COLOR.temperature.Override(Slider("Temperature", GAME_COLOR.temperature.value, -100, 100));
                                    GAME_COLOR.tint.Override(Slider("Tint", GAME_COLOR.tint.value, -100, 100));
                                    GAME_COLOR.postExposure.Override(Slider("Post-exposure", GAME_COLOR.postExposure.value, 0, 5));
                                    GAME_COLOR.hueShift.Override(Slider("Hue shift", GAME_COLOR.hueShift.value, -180, 180));
                                    GAME_COLOR.saturation.Override(Slider("Saturation", GAME_COLOR.saturation.value, -100, 100));
                                    GAME_COLOR.contrast.Override(Slider("Contrast", GAME_COLOR.contrast.value, -100, 100));

                                    Label(" ");
                                    BeginHorizontal();
                                    Label("Advanced");
                                    sp_COLOR_ADV = Spoiler(sp_COLOR_ADV ? "hide" : "show") ? !sp_COLOR_ADV : sp_COLOR_ADV;
                                    EndHorizontal();
                                    if (sp_COLOR_ADV) {
                                        Label("Lift");
                                        Vector4 lift = new Vector4();
                                        lift.x = Slider("r", GAME_COLOR.lift.value.x, -2, 2);
                                        lift.y = Slider("g", GAME_COLOR.lift.value.y, -2, 2);
                                        lift.z = Slider("b", GAME_COLOR.lift.value.z, -2, 2);
                                        lift.w = Slider("w", GAME_COLOR.lift.value.w, -2, 2);
                                        GAME_COLOR.lift.Override(lift);

                                        Label("Gamma");
                                        Vector4 gamma = new Vector4();
                                        gamma.x = Slider("r", GAME_COLOR.gamma.value.x, -2, 2);
                                        gamma.y = Slider("g", GAME_COLOR.gamma.value.y, -2, 2);
                                        gamma.z = Slider("b", GAME_COLOR.gamma.value.z, -2, 2);
                                        gamma.w = Slider("w", GAME_COLOR.gamma.value.w, -2, 2);
                                        GAME_COLOR.gamma.Override(gamma);

                                        Label("Gain");
                                        Vector4 gain = new Vector4();
                                        gain.x = Slider("r", GAME_COLOR.gain.value.x, -2, 2);
                                        gain.y = Slider("g", GAME_COLOR.gain.value.y, -2, 2);
                                        gain.z = Slider("b", GAME_COLOR.gain.value.z, -2, 2);
                                        gain.w = Slider("w", GAME_COLOR.gain.value.w, -2, 2);
                                        GAME_COLOR.gain.Override(gain);
                                    }
                                    Label(" ");

                                    if (Button("Reset")) {
                                        // NEED TO RESET ONE BY ONE
                                        GAME_COLOR.temperature.Override(0);
                                        GAME_COLOR.tint.Override(0);
                                        GAME_COLOR.postExposure.Override(0);
                                        GAME_COLOR.hueShift.Override(0);
                                        GAME_COLOR.saturation.Override(0);
                                        GAME_COLOR.contrast.Override(0);
                                        GAME_COLOR.lift.Override(new Vector4(1, 1, 1, 0));
                                        GAME_COLOR.gamma.Override(new Vector4(1, 1, 1, 0));
                                        GAME_COLOR.gain.Override(new Vector4(1, 1, 1, 0));
                                    }
                                }
                            }
                            Separator();
                            // Depth Of Field
                            {
                                BeginHorizontal();
                                GAME_DOF.enabled.Override(GUILayout.Toggle(GAME_DOF.enabled.value, "Depth Of Field"));
                                if (GAME_DOF.enabled.value) sp_DOF = Spoiler(sp_DOF ? "hide" : "show") ? !sp_DOF : sp_DOF;
                                EndHorizontal();
                                if (GAME_DOF.enabled.value && sp_DOF) {
                                    var new_mode = (FocusMode)GUILayout.SelectionGrid((int)focus_mode, focus_modes, focus_modes.Length);
                                    if (new_mode != focus_mode) GAME_DOF.focusDistance = Main.selectedPreset.DOF.focusDistance;
                                    focus_mode = new_mode;
                                    if (focus_mode == FocusMode.Custom) {
                                        GAME_DOF.focusDistance.Override(Slider("Focus distance", GAME_DOF.focusDistance.value, 0, 20));
                                    }
                                    else {
                                        Label("focus distance: " + GAME_DOF.focusDistance.value.ToString("0.00"));
                                    }
                                    GAME_DOF.aperture.Override(Slider("Aperture (f-stop)", GAME_DOF.aperture.value, 0.1f, 32));
                                    GAME_DOF.focalLength.Override(Slider("Focal length (mm)", GAME_DOF.focalLength.value, 1, 300));
                                    BeginHorizontal();
                                    Label("Max blur size: ");
                                    GAME_DOF.kernelSize.Override((KernelSize)GUILayout.SelectionGrid((int)GAME_DOF.kernelSize.value, max_blur, max_blur.Length));
                                    EndHorizontal();
                                    if (Button("Reset")) GAME_DOF = reset<DepthOfField>();
                                }
                            }
                            Separator();
                            // Grain
                            {
                                BeginHorizontal();
                                GAME_GRAIN.enabled.Override(GUILayout.Toggle(GAME_GRAIN.enabled.value, "Grain"));
                                if (GAME_GRAIN.enabled.value) sp_GRAIN = Spoiler(sp_GRAIN ? "hide" : "show") ? !sp_GRAIN : sp_GRAIN;
                                EndHorizontal();
                                if (GAME_GRAIN.enabled.value && sp_GRAIN) {
                                    GAME_GRAIN.colored.Override(GUILayout.Toggle(GAME_GRAIN.colored.value, "colored"));
                                    GAME_GRAIN.intensity.Override(Slider("Intensity", GAME_GRAIN.intensity.value, 0, 1));
                                    GAME_GRAIN.size.Override(Slider("Size", GAME_GRAIN.size.value, 0.3f, 3));
                                    GAME_GRAIN.lumContrib.Override(Slider("Luminance contribution", GAME_GRAIN.lumContrib.value, 0, 1));
                                    if (Button("Reset")) GAME_GRAIN = reset<Grain>();
                                }
                            }
                            Separator();
                            // Lens Distortion
                            {
                                BeginHorizontal();
                                GAME_LENS.enabled.Override(GUILayout.Toggle(GAME_LENS.enabled.value, "Lens distortion"));
                                if (GAME_LENS.enabled.value) sp_LENS = Spoiler(sp_LENS ? "hide" : "show") ? !sp_LENS : sp_LENS;
                                EndHorizontal();
                                if (GAME_LENS.enabled.value && sp_LENS) {
                                    GAME_LENS.intensity.Override(Slider("Intensity", GAME_LENS.intensity.value, -100, 100));
                                    GAME_LENS.intensityX.Override(Slider("X", GAME_LENS.intensityX.value, 0, 1));
                                    GAME_LENS.intensityY.Override(Slider("Y", GAME_LENS.intensityY.value, 0, 1));
                                    GAME_LENS.scale.Override(Slider("Scale", GAME_LENS.scale.value, 0.1f, 5));
                                    if (Button("Reset")) GAME_LENS = reset<LensDistortion>();
                                }
                            }
                            Separator();
                            // Motion Blur
                            {
                                BeginHorizontal();
                                GAME_BLUR.enabled.Override(GUILayout.Toggle(GAME_BLUR.enabled.value, "Motion blur"));
                                if (GAME_BLUR.enabled.value) sp_BLUR = Spoiler(sp_BLUR ? "hide" : "show") ? !sp_BLUR : sp_BLUR;
                                EndHorizontal();
                                if (GAME_BLUR.enabled.value && sp_BLUR) {
                                    GAME_BLUR.shutterAngle.Override(Slider("Shutter angle", GAME_BLUR.shutterAngle, 0, 360));
                                    GAME_BLUR.sampleCount.Override(SliderInt("Sample count", GAME_BLUR.sampleCount, 4, 32));
                                    if (Button("Reset")) GAME_BLUR = reset<MotionBlur>();
                                }
                            }
                            Separator();
                            // Screen Space Reflections
                            {
                                BeginHorizontal();
                                GAME_REFL.enabled.Override(GUILayout.Toggle(GAME_REFL.enabled.value, "Reflections"));
                                if (GAME_REFL.enabled.value) sp_REFL = Spoiler(sp_REFL ? "hide" : "show") ? !sp_REFL : sp_REFL;
                                EndHorizontal();
                                if (GAME_REFL.enabled.value && sp_REFL) {
                                    GAME_REFL.preset.Override((ScreenSpaceReflectionPreset)GUILayout.SelectionGrid((int)GAME_REFL.preset.value, refl_presets, refl_presets.Length));
                                    if (Button("Reset")) GAME_REFL = reset<ScreenSpaceReflections>();
                                }
                            }
                            Separator();
                            // Vignette
                            {
                                BeginHorizontal();
                                GAME_VIGN.enabled.Override(GUILayout.Toggle(GAME_VIGN.enabled.value, "Vignette"));
                                if (GAME_VIGN.enabled.value) sp_VIGN = Spoiler(sp_VIGN ? "hide" : "show") ? !sp_VIGN : sp_VIGN;
                                EndHorizontal();
                                if (GAME_VIGN.enabled.value && sp_VIGN) {
                                    GAME_VIGN.intensity.Override(Slider("Intensity", GAME_VIGN.intensity.value, 0, 1));
                                    GAME_VIGN.smoothness.Override(Slider("Smoothness", GAME_VIGN.smoothness.value, 0.1f, 1));
                                    GAME_VIGN.roundness.Override(Slider("Roundness", GAME_VIGN.roundness.value, 0, 1));
                                    BeginHorizontal();
                                    GAME_VIGN.rounded.Override(GUILayout.Toggle(GAME_VIGN.rounded.value, "Rounded"));
                                    if (Button("Reset")) GAME_VIGN = reset<Vignette>();
                                    EndHorizontal();
                                }
                            }
                        }
                        else {
                            Label("ENABLE POST PROCESSING FIRST");
                        }
                    }
                    else if (selectedTab == SelectedTab.Camera) {
                        // Modes
                        cameraController.cameraMode = (CameraMode)GUILayout.SelectionGrid((int)cameraController.cameraMode, camera_names, camera_names.Length);
                        if (cameraController.cameraMode == CameraMode.Normal) {
                            cameraController.normal_fov = Slider("Field of View", cameraController.normal_fov, 1, 179);
                            if (Button("Reset")) {
                                cameraController.normal_fov = 60;
                            }
                        }
                        else if (cameraController.cameraMode == CameraMode.Low) {
                            cameraController.low_fov = Slider("Field of View", cameraController.low_fov, 1, 179);
                            if (Button("Reset")) {
                                cameraController.low_fov = 60;
                            }
                        }
                        else if (cameraController.cameraMode == CameraMode.Follow) {
                            cameraController.follow_fov = Slider("Field of View", cameraController.follow_fov, 1, 179);
                            if (Button("Reset")) {
                                cameraController.follow_fov = 60;
                            }
                            Separator();
                            Label("Move camera: ");
                            cameraController.follow_shift.x = Slider("x", cameraController.follow_shift.x, -2, 2);
                            cameraController.follow_shift.y = Slider("y", cameraController.follow_shift.y, -2, 2);
                            cameraController.follow_shift.z = Slider("z", cameraController.follow_shift.z, -2, 2);
                            if (Button("Reset to player")) cameraController.follow_shift = new Vector3();
                        }
                        else if (cameraController.cameraMode == CameraMode.POV) {
                            cameraController.pov_fov = Slider("Field of View", cameraController.pov_fov, 1, 179);
                            if (Button("Reset")) {
                                cameraController.pov_fov = 60;
                            }
                            Separator();
                            Label("Move camera: ");
                            Camera.main.nearClipPlane = Slider("Near clipping", Camera.main.nearClipPlane, 0, 1);
                            cameraController.pov_smooth = Slider("Smoothness", cameraController.pov_smooth, 0, 1);
                            cameraController.pov_shift.x = Slider("x", cameraController.pov_shift.x, -2, 2);
                            cameraController.pov_shift.y = Slider("y", cameraController.pov_shift.y, -2, 2);
                            cameraController.pov_shift.z = Slider("z", cameraController.pov_shift.z, -2, 2);
                            if (Button("Reset to head")) cameraController.pov_shift = new Vector3();
                        }
                        else if (cameraController.cameraMode == CameraMode.Skate) {
                            cameraController.skate_fov = Slider("Field of View", cameraController.skate_fov, 1, 179);
                            if (Button("Reset")) {
                                cameraController.skate_fov = 60;
                            }
                            Separator();
                            Label("Move camera: ");
                            cameraController.skate_shift.x = Slider("x", cameraController.skate_shift.x, -2, 2);
                            cameraController.skate_shift.y = Slider("y", cameraController.skate_shift.y, -2, 2);
                            cameraController.skate_shift.z = Slider("z", cameraController.skate_shift.z, -2, 2);
                            if (Button("Reset to skate")) cameraController.skate_shift = new Vector3();
                        }
                    }

                    GUILayout.FlexibleSpace();
                    Separator();
                    // Preset selection, save, close
                    {
                        BeginHorizontal();
                        Label("Preset: ");
                        Label(Main.settings.presetName, usingStyle);
                        EndHorizontal();
                        BeginHorizontal();
                        if (Button("Save as new preset")) {
                            choosing_name = true;
                            name_text = "";
                        }
                        if (Button("Change preset")) {
                            changing_preset = true;
                        }
                        EndHorizontal();
                        BeginHorizontal();
                        if (Button("Save")) {
                            Main.Save();
                        }
                        if (Button("Save and close")) {
                            Close();
                        }
                        EndHorizontal();
                    }
                }
            }
            GUILayout.EndScrollView();

            if (Main.settings.DEBUG) log("End renderWindow");
        }
    }
}