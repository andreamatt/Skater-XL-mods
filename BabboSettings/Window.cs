using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XLShredLib;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

namespace BabboSettings
{
    public class Window : Module
    {

        #region GUI content
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
        private string[] tab_names = { "Basic", "Effects", "Camera" };
        private string[] camera_names = { "Normal", "Low", "Follow", "POV", "Skate" };
        #endregion

        #region GUI style
        private Rect windowRect = new Rect(50f, 50f, 600f, 0f);
        private GUIStyle windowStyle;
        private GUIStyle spoilerBtnStyle;
        private GUIStyle sliderStyle;
        private GUIStyle thumbStyle;
        private GUIStyle labelStyle;
        private GUIStyle toggleStyle;
        private readonly Color windowColor = new Color(0.2f, 0.2f, 0.2f);
        private string separator;
        private GUIStyle separatorStyle;
        private Vector2 scrollPosition = new Vector2();
        #endregion

        #region GUI status
        private bool showUI = false;
        private GameObject master;
        private bool setUp;
        private bool sp_AA, sp_AO, sp_EXPO, sp_BLOOM, sp_CA, sp_COLOR, sp_COLOR_ADV, sp_DOF, sp_GRAIN, sp_LENS, sp_BLUR, sp_REFL, sp_VIGN;
        private bool choosing_name, editing_preset;
        private Preset edited_preset;
        private string name_text = "";
        private SelectedTab selectedTab = SelectedTab.Basic;
        #endregion

        public override void Update() {
            bool keyUp = Input.GetKeyUp(KeyCode.Backspace);
            if (keyUp) {
                if (showUI == false) {
                    Open();
                }
                else {
                    Close();
                }
            }
        }

        private void SetUp() {
            if (master == null) {
                master = GameObject.Find("New Master Prefab");
                if (master != null) {
                    UnityEngine.Object.DontDestroyOnLoad(master);
                }
            }

            windowStyle = new GUIStyle(GUI.skin.window) {
                padding = new RectOffset(10, 10, 25, 10),
                contentOffset = new Vector2(0, -23.0f)
            };

            spoilerBtnStyle = new GUIStyle(GUI.skin.button) {
                fixedWidth = 100
            };

            sliderStyle = new GUIStyle(GUI.skin.horizontalSlider) {
                fixedWidth = 200
            };

            thumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb) {

            };

            separatorStyle = new GUIStyle(GUI.skin.label) {
                fontSize = 4
            };
            separatorStyle.normal.textColor = Color.red;
            separator = new string('_', 188);

            labelStyle = new GUIStyle(GUI.skin.label) {
            };

            toggleStyle = new GUIStyle(GUI.skin.toggle) {
            };
        }

        private void Open() {
            showUI = true;
            ModMenu.Instance.ShowCursor(Main.modId);
        }

        private void Close() {
            showUI = false;
            ModMenu.Instance.HideCursor(Main.modId);
            Main.Save();
        }

        public override void OnGUI() {
            if (!setUp) {
                setUp = true;
                SetUp();
            }

            GUI.backgroundColor = windowColor;

            if (showUI) {
                windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Graphic Settings by Babbo", windowStyle, GUILayout.Width(400));
            }
        }

        void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(750));
            {
                //cameraController.pov_rot_shift.x = Slider("X", cameraController.pov_rot_shift.x, -180, 180); ???
                //cameraController.pov_rot_shift.y = Slider("Y", cameraController.pov_rot_shift.y, -180, 180);
                //cameraController.pov_rot_shift.z = Slider("Z", cameraController.pov_rot_shift.z, -180, 180);

                if (choosing_name) {
                    name_text = GUILayout.TextField(name_text);
                    BeginHorizontal();
                    if (name_text != "" && Button("Confirm")) {
                        choosing_name = false;
                        if (Main.presets.ContainsKey(name_text)) {
                            Logger.Log("Preset already exists");
                        }
                        else {
                            Preset new_preset = new Preset(name_text);
                            Main.presets[new_preset.name] = new_preset;
                            Main.settings.presetOrder.Insert(0, new_preset.name);
                        }
                        name_text = "";
                    }
                    if (Button("Cancel")) {
                        choosing_name = false;
                        name_text = "";
                    }
                    EndHorizontal();
                }
                else if (editing_preset) {
                    Separator();
                    #region Ambient Occlusion
                    BeginHorizontal();
                    edited_preset.AO.enabled.Override(Toggle(edited_preset.AO.enabled.value, "Ambient occlusion"));
                    if (edited_preset.AO.enabled.value) sp_AO = Spoiler(sp_AO ? "hide" : "show") ? !sp_AO : sp_AO;
                    EndHorizontal();
                    if (edited_preset.AO.enabled.value && sp_AO) {
                        edited_preset.AO.intensity.Override(Slider("Intensity", edited_preset.AO.intensity.value, 0, 4));
                        edited_preset.AO.quality.Override((AmbientOcclusionQuality)GUILayout.SelectionGrid((int)edited_preset.AO.quality.value, ao_quality, ao_quality.Length));
                        edited_preset.AO.mode.Override((AmbientOcclusionMode)GUILayout.SelectionGrid((int)edited_preset.AO.mode.value, ao_mode, ao_mode.Length));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.AO);
                    }
                    #endregion
                    Separator();
                    #region Automatic Exposure
                    BeginHorizontal();
                    edited_preset.EXPO.enabled.Override(Toggle(edited_preset.EXPO.enabled.value, "Automatic Exposure"));
                    if (edited_preset.EXPO.enabled.value) sp_EXPO = Spoiler(sp_EXPO ? "hide" : "show") ? !sp_EXPO : sp_EXPO;
                    EndHorizontal();
                    if (edited_preset.EXPO.enabled.value && sp_EXPO) {
                        edited_preset.EXPO.keyValue.Override(Slider("Compensation", edited_preset.EXPO.keyValue.value, 0, 4));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.EXPO);
                    }
                    #endregion
                    Separator();
                    #region Bloom
                    BeginHorizontal();
                    edited_preset.BLOOM.enabled.Override(Toggle(edited_preset.BLOOM.enabled.value, "Bloom"));
                    if (edited_preset.BLOOM.enabled.value) sp_BLOOM = Spoiler(sp_BLOOM ? "hide" : "show") ? !sp_BLOOM : sp_BLOOM;
                    EndHorizontal();
                    if (edited_preset.BLOOM.enabled.value && sp_BLOOM) {
                        edited_preset.BLOOM.intensity.Override(Slider("Intensity", edited_preset.BLOOM.intensity.value, 0, 4));
                        edited_preset.BLOOM.threshold.Override(Slider("Threshold", edited_preset.BLOOM.threshold.value, 0, 4));
                        edited_preset.BLOOM.diffusion.Override(Slider("Diffusion", edited_preset.BLOOM.diffusion.value, 1, 10));
                        edited_preset.BLOOM.fastMode.Override(Toggle(edited_preset.BLOOM.fastMode.value, "Fast mode"));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.BLOOM);
                    }
                    #endregion
                    Separator();
                    #region Chromatic aberration
                    BeginHorizontal();
                    edited_preset.CA.enabled.Override(Toggle(edited_preset.CA.enabled.value, "Chromatic aberration"));
                    if (edited_preset.CA.enabled.value) sp_CA = Spoiler(sp_CA ? "hide" : "show") ? !sp_CA : sp_CA;
                    EndHorizontal();
                    if (edited_preset.CA.enabled.value && sp_CA) {
                        edited_preset.CA.intensity.Override(Slider("Intensity", edited_preset.CA.intensity.value, 0, 1));
                        edited_preset.CA.fastMode.Override(Toggle(edited_preset.CA.fastMode.value, "Fast mode"));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.CA);
                    }
                    #endregion
                    Separator();
                    #region Color Grading
                    BeginHorizontal();
                    edited_preset.COLOR.enabled.Override(Toggle(edited_preset.COLOR.enabled.value, "Color Grading"));
                    if (edited_preset.COLOR.enabled) sp_COLOR = Spoiler(sp_COLOR ? "hide" : "show") ? !sp_COLOR : sp_COLOR;
                    EndHorizontal();
                    if (edited_preset.COLOR.enabled && sp_COLOR) {
                        BeginHorizontal();
                        Label("Tonemapper: ");
                        edited_preset.COLOR.tonemapper.Override((Tonemapper)GUILayout.SelectionGrid((int)edited_preset.COLOR.tonemapper.value, tonemappers, tonemappers.Length));
                        EndHorizontal();
                        edited_preset.COLOR.temperature.Override(Slider("Temperature", edited_preset.COLOR.temperature.value, -100, 100));
                        edited_preset.COLOR.tint.Override(Slider("Tint", edited_preset.COLOR.tint.value, -100, 100));
                        edited_preset.COLOR.postExposure.Override(Slider("Post-exposure", edited_preset.COLOR.postExposure.value, 0, 5));
                        edited_preset.COLOR.hueShift.Override(Slider("Hue shift", edited_preset.COLOR.hueShift.value, -180, 180));
                        edited_preset.COLOR.saturation.Override(Slider("Saturation", edited_preset.COLOR.saturation.value, -100, 100));
                        edited_preset.COLOR.contrast.Override(Slider("Contrast", edited_preset.COLOR.contrast.value, -100, 100));

                        Label(" ");
                        BeginHorizontal();
                        Label("Advanced");
                        sp_COLOR_ADV = Spoiler(sp_COLOR_ADV ? "hide" : "show") ? !sp_COLOR_ADV : sp_COLOR_ADV;
                        EndHorizontal();
                        if (sp_COLOR_ADV) {
                            Label("Lift");
                            edited_preset.COLOR.lift.value.x = Slider("r", edited_preset.COLOR.lift.value.x, -2, 2);
                            edited_preset.COLOR.lift.value.y = Slider("g", edited_preset.COLOR.lift.value.y, -2, 2);
                            edited_preset.COLOR.lift.value.z = Slider("b", edited_preset.COLOR.lift.value.z, -2, 2);
                            edited_preset.COLOR.lift.value.w = Slider("w", edited_preset.COLOR.lift.value.w, -2, 2);

                            Label("Gamma");
                            edited_preset.COLOR.gamma.value.x = Slider("r", edited_preset.COLOR.gamma.value.x, -2, 2);
                            edited_preset.COLOR.gamma.value.y = Slider("g", edited_preset.COLOR.gamma.value.y, -2, 2);
                            edited_preset.COLOR.gamma.value.z = Slider("b", edited_preset.COLOR.gamma.value.z, -2, 2);
                            edited_preset.COLOR.gamma.value.w = Slider("w", edited_preset.COLOR.gamma.value.w, -2, 2);

                            Label("Gain");
                            edited_preset.COLOR.gain.value.x = Slider("r", edited_preset.COLOR.gain.value.x, -2, 2);
                            edited_preset.COLOR.gain.value.y = Slider("g", edited_preset.COLOR.gain.value.y, -2, 2);
                            edited_preset.COLOR.gain.value.z = Slider("b", edited_preset.COLOR.gain.value.z, -2, 2);
                            edited_preset.COLOR.gain.value.w = Slider("w", edited_preset.COLOR.gain.value.w, -2, 2);
                        }
                        Label(" ");

                        if (Button("Reset")) gameEffects.reset(ref edited_preset.COLOR);
                    }
                    #endregion
                    Separator();
                    #region Depth Of Field
                    BeginHorizontal();
                    edited_preset.DOF.enabled.Override(Toggle(edited_preset.DOF.enabled.value, "Depth Of Field"));
                    if (edited_preset.DOF.enabled.value) sp_DOF = Spoiler(sp_DOF ? "hide" : "show") ? !sp_DOF : sp_DOF;
                    EndHorizontal();
                    if (edited_preset.DOF.enabled.value && sp_DOF) {
                        edited_preset.FOCUS_MODE = (FocusMode)GUILayout.SelectionGrid((int)edited_preset.FOCUS_MODE, focus_modes, focus_modes.Length);
                        if (edited_preset.FOCUS_MODE == FocusMode.Custom) {
                            edited_preset.DOF.focusDistance.Override(Slider("Focus distance", edited_preset.DOF.focusDistance.value, 0, 20));
                        }
                        else {
                            Label("focus distance: " + gameEffects.DOF.focusDistance.value.ToString("0.00"));
                        }
                        edited_preset.DOF.aperture.Override(Slider("Aperture (f-stop)", edited_preset.DOF.aperture.value, 0.1f, 32));
                        edited_preset.DOF.focalLength.Override(Slider("Focal length (mm)", edited_preset.DOF.focalLength.value, 1, 300));
                        BeginHorizontal();
                        Label("Max blur size: ");
                        edited_preset.DOF.kernelSize.Override((KernelSize)GUILayout.SelectionGrid((int)edited_preset.DOF.kernelSize.value, max_blur, max_blur.Length));
                        EndHorizontal();
                        if (Button("Reset")) {
                            gameEffects.reset(ref edited_preset.DOF);
                            edited_preset.FOCUS_MODE = FocusMode.Custom;
                        }
                    }
                    #endregion
                    Separator();
                    #region Grain
                    BeginHorizontal();
                    edited_preset.GRAIN.enabled.Override(Toggle(edited_preset.GRAIN.enabled.value, "Grain"));
                    if (edited_preset.GRAIN.enabled.value) sp_GRAIN = Spoiler(sp_GRAIN ? "hide" : "show") ? !sp_GRAIN : sp_GRAIN;
                    EndHorizontal();
                    if (edited_preset.GRAIN.enabled.value && sp_GRAIN) {
                        edited_preset.GRAIN.colored.Override(Toggle(edited_preset.GRAIN.colored.value, "colored"));
                        edited_preset.GRAIN.intensity.Override(Slider("Intensity", edited_preset.GRAIN.intensity.value, 0, 1));
                        edited_preset.GRAIN.size.Override(Slider("Size", edited_preset.GRAIN.size.value, 0.3f, 3));
                        edited_preset.GRAIN.lumContrib.Override(Slider("Luminance contribution", edited_preset.GRAIN.lumContrib.value, 0, 1));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.GRAIN);
                    }
                    #endregion
                    Separator();
                    #region Lens Distortion
                    BeginHorizontal();
                    edited_preset.LENS.enabled.Override(Toggle(edited_preset.LENS.enabled.value, "Lens distortion"));
                    if (edited_preset.LENS.enabled.value) sp_LENS = Spoiler(sp_LENS ? "hide" : "show") ? !sp_LENS : sp_LENS;
                    EndHorizontal();
                    if (edited_preset.LENS.enabled.value && sp_LENS) {
                        edited_preset.LENS.intensity.Override(Slider("Intensity", edited_preset.LENS.intensity.value, -100, 100));
                        edited_preset.LENS.intensityX.Override(Slider("X", edited_preset.LENS.intensityX.value, 0, 1));
                        edited_preset.LENS.intensityY.Override(Slider("Y", edited_preset.LENS.intensityY.value, 0, 1));
                        edited_preset.LENS.scale.Override(Slider("Scale", edited_preset.LENS.scale.value, 0.1f, 5));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.LENS);
                    }
                    #endregion
                    Separator();
                    #region Motion Blur
                    BeginHorizontal();
                    edited_preset.BLUR.enabled.Override(Toggle(edited_preset.BLUR.enabled.value, "Motion blur"));
                    if (edited_preset.BLUR.enabled.value) sp_BLUR = Spoiler(sp_BLUR ? "hide" : "show") ? !sp_BLUR : sp_BLUR;
                    EndHorizontal();
                    if (edited_preset.BLUR.enabled.value && sp_BLUR) {
                        edited_preset.BLUR.shutterAngle.Override(Slider("Shutter angle", edited_preset.BLUR.shutterAngle, 0, 360));
                        edited_preset.BLUR.sampleCount.Override(SliderInt("Sample count", edited_preset.BLUR.sampleCount, 4, 32));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.BLUR);
                    }
                    #endregion
                    Separator();
                    #region Screen Space Reflections
                    BeginHorizontal();
                    edited_preset.REFL.enabled.Override(Toggle(edited_preset.REFL.enabled.value, "Reflections"));
                    if (edited_preset.REFL.enabled.value) sp_REFL = Spoiler(sp_REFL ? "hide" : "show") ? !sp_REFL : sp_REFL;
                    EndHorizontal();
                    if (edited_preset.REFL.enabled.value && sp_REFL) {
                        edited_preset.REFL.preset.Override((ScreenSpaceReflectionPreset)GUILayout.SelectionGrid((int)edited_preset.REFL.preset.value, refl_presets, refl_presets.Length));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.REFL);
                    }
                    #endregion
                    Separator();
                    #region Vignette
                    BeginHorizontal();
                    edited_preset.VIGN.enabled.Override(Toggle(edited_preset.VIGN.enabled.value, "Vignette"));
                    if (edited_preset.VIGN.enabled.value) sp_VIGN = Spoiler(sp_VIGN ? "hide" : "show") ? !sp_VIGN : sp_VIGN;
                    EndHorizontal();
                    if (edited_preset.VIGN.enabled.value && sp_VIGN) {
                        edited_preset.VIGN.intensity.Override(Slider("Intensity", edited_preset.VIGN.intensity.value, 0, 1));
                        edited_preset.VIGN.smoothness.Override(Slider("Smoothness", edited_preset.VIGN.smoothness.value, 0.1f, 1));
                        edited_preset.VIGN.roundness.Override(Slider("Roundness", edited_preset.VIGN.roundness.value, 0, 1));
                        BeginHorizontal();
                        edited_preset.VIGN.rounded.Override(Toggle(edited_preset.VIGN.rounded.value, "Rounded"));
                        if (Button("Reset")) gameEffects.reset(ref edited_preset.VIGN);
                        EndHorizontal();
                    }
                    #endregion
                    Separator();
                    if (Button("Save")) editing_preset = false;
                }
                else {
                    BeginHorizontal();
                    selectedTab = (SelectedTab)GUILayout.SelectionGrid((int)selectedTab, tab_names, tab_names.Length);
                    EndHorizontal();

                    if (selectedTab == SelectedTab.Basic) {
                        #region Post in general
                        gameEffects.post_volume.enabled = Toggle(gameEffects.post_volume.enabled, "Enable post processing");
                        #endregion
                        Separator();
                        #region VSync
                        BeginHorizontal();
                        Label("Vsync");
                        QualitySettings.vSyncCount = GUILayout.SelectionGrid(QualitySettings.vSyncCount, vsync_names, vsync_names.Length);
                        EndHorizontal();
                        #endregion
                        Separator();
                        #region Fullscreen
                        BeginHorizontal();
                        Label("Fullscreen");
                        Screen.fullScreenMode = (FullScreenMode)GUILayout.SelectionGrid((int)Screen.fullScreenMode, screen_modes, screen_modes.Length);
                        EndHorizontal();
                        #endregion
                        Separator();
                        #region AntiAliasing
                        Label("AntiAliasing");
                        gameEffects.post_layer.antialiasingMode = (Antialiasing)(GUILayout.SelectionGrid((int)gameEffects.post_layer.antialiasingMode, aa_names, aa_names.Length));
                        if (gameEffects.post_layer.antialiasingMode == Antialiasing.SubpixelMorphologicalAntialiasing) {
                            sp_AA = Spoiler(sp_AA ? "hide" : "show") ? !sp_AA : sp_AA;
                            if (sp_AA) {
                                gameEffects.SMAA.quality = (SubpixelMorphologicalAntialiasing.Quality)GUILayout.SelectionGrid((int)gameEffects.SMAA.quality, smaa_quality, smaa_quality.Length);
                            }
                        }
                        else if (gameEffects.post_layer.antialiasingMode == Antialiasing.TemporalAntialiasing) {
                            sp_AA = Spoiler(sp_AA ? "hide" : "show") ? !sp_AA : sp_AA;
                            if (sp_AA) {
                                gameEffects.TAA.sharpness = Slider("Sharpness", gameEffects.TAA.sharpness, 0, 3);
                                gameEffects.TAA.jitterSpread = Slider("Jitter spread", gameEffects.TAA.jitterSpread, 0, 1);
                                gameEffects.TAA.stationaryBlending = Slider("Stationary blending", gameEffects.TAA.stationaryBlending, 0, 1);
                                gameEffects.TAA.motionBlending = Slider("Motion Blending", gameEffects.TAA.motionBlending, 0, 1);
                                if (Button("Default TAA")) {
                                    gameEffects.TAA = gameEffects.post_layer.temporalAntialiasing = new TemporalAntialiasing();
                                }
                            }
                        }
                        #endregion
                    }
                    else if (selectedTab == SelectedTab.Effects) {
                        for (int i = 0; i < Main.settings.presetOrder.Count; i++) {
                            var preset = Main.presets[Main.settings.presetOrder[i]];
                            BeginHorizontal();
                            preset.enabled = Toggle(preset.enabled, preset.name);
                            GUILayout.FlexibleSpace();
                            if (preset.enabled && Button("edit")) {
                                editing_preset = true;
                                edited_preset = preset;
                            }
                            GUILayout.FlexibleSpace();
                            if (i > 0 && Button("Λ")) {
                                var tmp = Main.settings.presetOrder[i - 1];
                                Main.settings.presetOrder[i - 1] = Main.settings.presetOrder[i];
                                Main.settings.presetOrder[i] = tmp;
                            }
                            if (i < Main.settings.presetOrder.Count - 1 && Button("V")) {
                                var tmp = Main.settings.presetOrder[i + 1];
                                Main.settings.presetOrder[i + 1] = Main.settings.presetOrder[i];
                                Main.settings.presetOrder[i] = tmp;
                            }
                            EndHorizontal();
                        }
                        if (Button("NEW PRESET")) {
                            choosing_name = true;
                        }
                    }
                    else if (selectedTab == SelectedTab.Camera) {
                        // Modes
                        var inReplay = BabboSettings.IsReplayActive();
                        if (inReplay) {
                            Label("While in replay only normal mode is available");
                            cameraController.replay_fov = cameraController.mainCamera.fieldOfView = Slider("Field of View", cameraController.mainCamera.fieldOfView, 1, 179);
                            if (Button("Reset")) {
                                cameraController.replay_fov = 60;
                            }
                        }
                        else {
                            cameraController.cameraMode = (CameraMode)GUILayout.SelectionGrid((int)cameraController.cameraMode, camera_names, camera_names.Length);
                            if (cameraController.cameraMode == CameraMode.Normal) {
                                // No longer needed since the controller stuff happens only while in replay
                                //cameraController.normal_fov = cameraController.mainCamera.fieldOfView = Slider("Field of View", cameraController.mainCamera.fieldOfView, 1, 179);
                                cameraController.normal_fov = Slider("Field of View", cameraController.normal_fov, 1, 179);
                                if (Button("Reset")) {
                                    cameraController.normal_fov = 60;
                                }
                                Separator();
                                cameraController.normal_clip = Slider("Near clipping", cameraController.normal_clip, 0.01f, 1);
                                Separator();
                                Label("Responsiveness. Suggested: 1, 1");
                                cameraController.normal_react = Slider("Position", cameraController.normal_react, 0.01f, 1);
                                cameraController.normal_react_rot = Slider("Rotation", cameraController.normal_react_rot, 0.01f, 1);
                            }
                            else if (cameraController.cameraMode == CameraMode.Low) {
                                Label("No controls here");
                                Label("If you want them, use follow cam");
                            }
                            else if (cameraController.cameraMode == CameraMode.Follow) {
                                cameraController.follow_fov = Slider("Field of View", cameraController.follow_fov, 1, 179);
                                if (Button("Reset")) {
                                    cameraController.follow_fov = 60;
                                }
                                Separator();
                                cameraController.follow_clip = Slider("Near clipping", cameraController.follow_clip, 0.01f, 1);
                                Separator();
                                Label("Responsiveness. Suggested: 0.8, 0.6");
                                cameraController.follow_react = Slider("Position", cameraController.follow_react, 0.01f, 1);
                                cameraController.follow_react_rot = Slider("Rotation", cameraController.follow_react_rot, 0.01f, 1);
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
                                cameraController.hide_head = Toggle(cameraController.hide_head, "Hide head");
                                cameraController.pov_clip = Slider("Near clipping", cameraController.pov_clip, 0.01f, 1);
                                Separator();
                                Label("Responsiveness. Suggested: 1, 0.1");
                                cameraController.pov_react = Slider("Position", cameraController.pov_react, 0.01f, 1);
                                cameraController.pov_react_rot = Slider("Rotation", cameraController.pov_react_rot, 0.01f, 1);
                                Separator();
                                Label("Move camera: ");
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
                                cameraController.skate_clip = Slider("Near clipping", cameraController.skate_clip, 0.01f, 1);
                                Separator();
                                Label("Responsiveness. Suggested: 1, 1");
                                cameraController.skate_react = Slider("Position", cameraController.skate_react, 0.01f, 1);
                                cameraController.skate_react_rot = Slider("Rotation", cameraController.skate_react_rot, 0.01f, 1);
                                Separator();
                                Label("Move camera: ");
                                cameraController.skate_shift.x = Slider("x", cameraController.skate_shift.x, -2, 2);
                                cameraController.skate_shift.y = Slider("y", cameraController.skate_shift.y, -2, 2);
                                cameraController.skate_shift.z = Slider("z", cameraController.skate_shift.z, -2, 2);
                                if (Button("Reset to skate")) cameraController.skate_shift = new Vector3();
                            }
                        }
                    }

                    GUILayout.FlexibleSpace();
                    Separator();
                    // Preset selection, save, close
                    {
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

            // apply presets while window is open
            presetsManager.ApplyPresets();
        }

        #region GUI Utility
        private enum SelectedTab
        {
            Basic,
            Effects,
            Camera
        }

        private void Label(string text, GUIStyle style) {
            GUILayout.Label(text, style);
        }
        private void Label(string text) {
            GUILayout.Label(text, labelStyle);
        }
        private bool Toggle(bool current, string text) {
            return GUILayout.Toggle(current, text, toggleStyle);
        }
        private void Separator() {
            Label(separator, separatorStyle);
        }
        private bool Button(string text) {
            return GUILayout.Button(text);
        }
        private bool Spoiler(string text) {
            return GUILayout.Button(text, spoilerBtnStyle);
        }
        private void BeginHorizontal() {
            GUILayout.BeginHorizontal();
        }
        private void EndHorizontal() {
            GUILayout.EndHorizontal();
        }
        private float Slider(string name, float current, float min, float max, bool horizontal = true) {
            if (horizontal) BeginHorizontal();
            Label(name + ": " + current.ToString("0.00"));
            float res = GUILayout.HorizontalSlider(current, min, max, sliderStyle, thumbStyle);
            if (horizontal) EndHorizontal();
            return res;
        }
        private int SliderInt(string name, int current, int min, int max, bool horizontal = true) {
            if (horizontal) BeginHorizontal();
            Label(name + ": " + current);
            float res = GUILayout.HorizontalSlider(current, min, max, sliderStyle, thumbStyle);
            if (horizontal) EndHorizontal();
            return Mathf.FloorToInt(res);
        }

        #endregion
    }
}