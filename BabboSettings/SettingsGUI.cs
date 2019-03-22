using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityModManagerNet;
using XLShredLib;

namespace BabboSettings {

    internal class SettingsGUI : MonoBehaviour {
        // GUI stuff
        private bool showUI = false;
        private GameObject master;
        private bool setUp;
        private Rect windowRect = new Rect(50f, 50f, 800f, 0f);
        private GUIStyle windowStyle;
        private GUIStyle columnLeftStyle = GUIStyle.none;
        private GUIStyle columnStyle = GUIStyle.none;
        private GUIStyle boxStyle;
        private GUIStyle toggleStyle;
        private readonly int largeFontSize = 28;
        private readonly int medFontSize = 18;
        private readonly int smallFontSize = 14;
        private readonly int spacing = 14;
        private readonly Color windowColor = new Color(0.2f, 0.2f, 0.2f);
        private readonly Color largeFontColor = Color.red;
        private readonly Color smallFontColor = Color.yellow;
        private GUIStyle fontLarge;
        private GUIStyle fontMed;
        private GUIStyle fontSmall;

        // Settings stuff
        private Camera main;
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

        private void LateUpdate() {
            // override everything that is implemented
            // AntiAliasing
            {
                post_layer.antialiasingMode = Main.settings.AA_MODE;

                // FXAA
                post_layer.fastApproximateAntialiasing.fastMode = false;
                post_layer.fastApproximateAntialiasing.keepAlpha = false;

                // SMAA
                post_layer.subpixelMorphologicalAntialiasing.quality = Main.settings.SMAA.quality;

                // TAA
                post_layer.temporalAntialiasing.sharpness = Main.settings.TAA_sharpness;
                post_layer.temporalAntialiasing.jitterSpread = Main.settings.TAA_jitter;
                post_layer.temporalAntialiasing.stationaryBlending = Main.settings.TAA_stationary;
                post_layer.temporalAntialiasing.motionBlending = Main.settings.TAA_motion;
            }

            // Field Of View
            {
                main.fieldOfView = Main.settings.FOV;
            }

            if (Main.settings.ENABLE_POST) {
                // Ambient Occlusion
                {
                    GAME_AO.enabled.Override(Main.settings.AO.enabled.value);
                    GAME_AO.quality.Override(Main.settings.AO.quality.value);
                    GAME_AO.mode.Override(Main.settings.AO.mode.value);
                }

                // Automatic Exposure
                {
                    GAME_EXPO.enabled.Override(Main.settings.EXPO.enabled.value);
                }

                // Bloom
                {
                    GAME_BLOOM.enabled.Override(Main.settings.BLOOM.enabled.value);
                }

                // Chromatic Aberration
                {
                    GAME_CA.enabled.Override(Main.settings.CA.enabled.value);
                }

                // Color Grading
                {
                    GAME_COLOR.enabled.Override(Main.settings.COLOR_enabled);
                    GAME_COLOR.saturation.Override(Main.settings.COLOR_saturation);
                }

                // Depth Of Field
                {
                    GAME_DOF.enabled.Override(Main.settings.DOF.enabled.value);
                }

                // Grain
                {
                    GAME_GRAIN.enabled.Override(Main.settings.GRAIN.enabled.value);
                }

                // Motion Blur
                {
                    GAME_BLUR.enabled.Override(Main.settings.BLUR.enabled.value);
                    GAME_BLUR.shutterAngle.Override(Main.settings.BLUR.shutterAngle.value);
                    GAME_BLUR.sampleCount.Override(Main.settings.BLUR.sampleCount.value);
                }

                // Screen Space Reflections
                {
                    GAME_REFL.enabled.Override(Main.settings.REFL.enabled.value);
                }

                // Vignette
                {
                    GAME_VIGN.enabled.Override(Main.settings.VIGN.enabled.value);
                }
            }
        }

        void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.BeginVertical();
            {
                // Post in general
                {
                    bool new_enable_post = GUILayout.Toggle(Main.settings.ENABLE_POST, "Enable post processing");
                    if (new_enable_post != Main.settings.ENABLE_POST) {
                        Main.settings.ENABLE_POST = new_enable_post;
                        post_volume.enabled = Main.settings.ENABLE_POST;
                    }
                }

                // AntiAliasing
                {
                    GUILayout.Space(5);
                    GUILayout.Label("AntiAliasing");
                    Main.settings.AA_MODE = (PostProcessLayer.Antialiasing)(GUILayout.SelectionGrid((int)Main.settings.AA_MODE, aa_names, aa_names.Length));
                    if (Main.settings.AA_MODE == PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing) {
                        Main.settings.SMAA.quality = (SubpixelMorphologicalAntialiasing.Quality)GUILayout.SelectionGrid((int)Main.settings.SMAA.quality, smaa_quality, smaa_quality.Length);
                    }
                    else if (Main.settings.AA_MODE == PostProcessLayer.Antialiasing.TemporalAntialiasing) {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("sharpness");
                            Main.settings.TAA_sharpness = GUILayout.HorizontalSlider(Main.settings.TAA_sharpness, 0, 3);
                            GUILayout.Label("jitter spread");
                            Main.settings.TAA_jitter = GUILayout.HorizontalSlider(Main.settings.TAA_jitter, 0, 1);
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("stationary blending");
                            Main.settings.TAA_stationary = GUILayout.HorizontalSlider(Main.settings.TAA_stationary, 0, 1);
                            GUILayout.Label("motion blending");
                            Main.settings.TAA_motion = GUILayout.HorizontalSlider(Main.settings.TAA_motion, 0, 1);
                        }
                        GUILayout.EndHorizontal();
                        if (GUILayout.Button("Default TAA")) {
                            var defaultAA = new TemporalAntialiasing();
                            Main.settings.TAA_sharpness = defaultAA.sharpness;
                            Main.settings.TAA_jitter = defaultAA.jitterSpread;
                            Main.settings.TAA_stationary = defaultAA.stationaryBlending;
                            Main.settings.TAA_motion = defaultAA.motionBlending;
                        }
                    }
                    GUILayout.FlexibleSpace();
                }

                // Field Of View
                {
                    GUILayout.Label("Field Of View");
                    Main.settings.FOV = GUILayout.HorizontalSlider(Main.settings.FOV, 30, 110);
                }

                if (Main.settings.ENABLE_POST) {
                    // Ambiental Occlusion
                    {
                        GUILayout.FlexibleSpace();
                        Main.settings.AO.enabled.Override(GUILayout.Toggle(Main.settings.AO.enabled.value, "Ambiental occlusion"));
                        if (Main.settings.AO.enabled.value) {
                            Main.settings.AO.quality.Override((AmbientOcclusionQuality)GUILayout.SelectionGrid((int)Main.settings.AO.quality.value, ao_quality, ao_quality.Length));
                            Main.settings.AO.mode.Override((AmbientOcclusionMode)GUILayout.SelectionGrid((int)Main.settings.AO.mode.value, ao_mode, ao_mode.Length));
                        }
                        GUILayout.FlexibleSpace();
                    }

                    // Automatic Exposure
                    {
                        Main.settings.EXPO.enabled.Override(GUILayout.Toggle(Main.settings.EXPO.enabled.value, "Automatic Exposure"));
                    }

                    // Bloom
                    {
                        GUILayout.FlexibleSpace();
                        Main.settings.BLOOM.enabled.Override(GUILayout.Toggle(Main.settings.BLOOM.enabled.value, "Bloom"));
                        GUILayout.FlexibleSpace();
                    }

                    // Chromatic aberration
                    {
                        GUILayout.FlexibleSpace();
                        Main.settings.CA.enabled.Override(GUILayout.Toggle(Main.settings.CA.enabled.value, "Chromatic aberration"));
                        GUILayout.FlexibleSpace();
                    }

                    // Color Grading
                    {
                        Main.settings.COLOR_enabled = GUILayout.Toggle(Main.settings.COLOR_enabled, "Color Grading");
                        Main.settings.COLOR_saturation = GUILayout.HorizontalSlider(Main.settings.COLOR_saturation, -100, 100);
                    }

                    // Depth Of Field
                    {
                        Main.settings.DOF.enabled.Override(GUILayout.Toggle(Main.settings.DOF.enabled.value, "Depth Of Field"));
                    }

                    // Grain
                    {
                        Main.settings.GRAIN.enabled.Override(GUILayout.Toggle(Main.settings.GRAIN.enabled.value, "Grain"));
                    }

                    // Motion Blur
                    {
                        GUILayout.FlexibleSpace();
                        Main.settings.BLUR.enabled.Override(GUILayout.Toggle(Main.settings.BLUR.enabled.value, "Motion blur"));
                        if (Main.settings.BLUR.enabled.value) {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Shutter angle");
                            Main.settings.BLUR.shutterAngle.Override((int)Math.Floor(GUILayout.HorizontalSlider(Main.settings.BLUR.shutterAngle, 0, 360)));
                            GUILayout.Label("Sample count");
                            Main.settings.BLUR.sampleCount.Override((int)Math.Floor(GUILayout.HorizontalSlider(Main.settings.BLUR.sampleCount, 0, 32)));
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.FlexibleSpace();
                    }

                    // Screen Space Reflections
                    {
                        Main.settings.REFL.enabled.Override(GUILayout.Toggle(Main.settings.REFL.enabled.value, "Reflections"));
                    }

                    // Vignette
                    {
                        Main.settings.VIGN.enabled.Override(GUILayout.Toggle(Main.settings.VIGN.enabled.value, "Vignette"));
                    }
                }
                // link to repo?
                if (GUILayout.Button("by Babbo Elu")) {
                    GUILayout.FlexibleSpace();
                    Application.OpenURL("http://github.com/andreamatt");
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
                log("Pos_volume is null in getSettings");
            }
            log("Searched all effects");

            GAME_FXAA = post_layer.fastApproximateAntialiasing;
            GAME_TAA = post_layer.temporalAntialiasing;
            GAME_SMAA = post_layer.subpixelMorphologicalAntialiasing;
            log("Found all AAs");

            log("Done reading post_layer and post_volume settings");
        }

        public SettingsGUI() {

        }

        public void Start() {
            log("Starting...");

            getSettings();

            //Open();
            log("Started");
        }

        void log(string s) {
            UnityModManager.Logger.Log(s);
        }

        void show(string s) {
            ModMenu.Instance.ShowMessage(s);
        }

        private void SetUp() {
            DontDestroyOnLoad(gameObject);
            master = GameObject.Find("Master Prefab");
            DontDestroyOnLoad(master);

            fontLarge = new GUIStyle() {
                fontSize = largeFontSize
            };
            fontLarge.normal.textColor = largeFontColor;
            fontMed = new GUIStyle() {
                fontSize = medFontSize
            };
            fontMed.normal.textColor = largeFontColor;
            fontSmall = new GUIStyle() {
                fontSize = smallFontSize,
                padding = new RectOffset(1, 0, 2, 0)
            };
            fontSmall.normal.textColor = smallFontColor;

            windowStyle = new GUIStyle(GUI.skin.window) {
                padding = new RectOffset(10, 10, 25, 10),
                contentOffset = new Vector2(0, -23.0f)
            };

            boxStyle = new GUIStyle(GUI.skin.box) {
                padding = new RectOffset(14, 14, 24, 9),
                contentOffset = new Vector2(0, -20f)
            };

            columnLeftStyle.margin.right = spacing;

            toggleStyle = new GUIStyle(GUI.skin.toggle) {
                fontSize = smallFontSize,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(20, 0, 2, 0),
                contentOffset = new Vector2(0, 0)
            };
            toggleStyle.normal.textColor = toggleStyle.active.textColor = toggleStyle.hover.textColor = largeFontColor;
            toggleStyle.onNormal.textColor = toggleStyle.onActive.textColor = toggleStyle.onHover.textColor = smallFontColor;


            toggleStyle.padding.left = 20;
            toggleStyle.imagePosition = ImagePosition.TextOnly;
        }

        private void Open() {
            showUI = true;
            ModMenu.Instance.RegisterShowCursor(Main.modId, () => 3);
        }

        private void Close() {
            showUI = false;
            ModMenu.Instance.UnregisterShowCursor(Main.modId);
            UnityModManager.SaveSettingsAndParams();
        }

        private void OnGUI() {
            if (!setUp) {
                setUp = true;
                SetUp();
            }

            GUI.backgroundColor = windowColor;

            if (master == null) {
                if (GameObject.Find("Master Prefab")) {
                    master = GameObject.Find("Master Prefab");
                    DontDestroyOnLoad(master);
                }
            }

            if (showUI) {
                windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Graphic Settings by Babbo", windowStyle, GUILayout.Width(600)); // no windowStyle?
            }
        }
    }
}
