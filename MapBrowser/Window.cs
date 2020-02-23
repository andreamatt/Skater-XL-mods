using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLShredLib;

namespace MapBrowser
{
    public class Window : MonoBehaviour
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
        private string[] tab_names = { "Basic", "Presets", "Camera" };
        private string[] camera_names = { "Normal", "Low", "Follow", "POV", "Skate" };
        private Texture2D paypalTexture;
        #endregion

        #region GUI style
        private Rect windowRect = new Rect(50f, 50f, 600f, 0f);
        private GUIStyle windowStyle;
        private GUIStyle spoilerBtnStyle;
        private GUIStyle sliderStyle;
        private GUIStyle thumbStyle;
        private GUIStyle labelStyle;
        private GUIStyle labelStyleRed;
        private GUIStyle labelStyleMinMax;
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
        private bool sp_AA, sp_AO, sp_EXPO, sp_BLOOM, sp_CA, sp_COLOR, sp_COLOR_ADV, sp_DOF, sp_GRAIN, sp_LENS, sp_BLUR, sp_REFL, sp_VIGN, sp_LIGHT;
        private bool choosing_name, editing_preset;
        private string name_text = "";
        private SelectedTab selectedTab = SelectedTab.Basic;
        public Dictionary<string, string> sliderTextValues = new Dictionary<string, string>();
        #endregion

        public void Update() {
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
                fixedWidth = 150
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

            labelStyleRed = new GUIStyle(GUI.skin.label) {
                normal = {
                    textColor = Color.red
                }
            };

            labelStyleMinMax = new GUIStyle(GUI.skin.label) {
                normal = {
                    textColor = Color.green
                },
                fixedWidth = 30
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

        public void OnGUI() {
            if (!setUp) {
                setUp = true;
                SetUp();
            }

            GUI.backgroundColor = windowColor;

            if (showUI) {
                windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Map browser", windowStyle, GUILayout.Width(400));
            }
        }

        void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(750));
            {

            }
            GUILayout.EndScrollView();
        }

        #region GUI Utility
        private enum SelectedTab
        {
            Basic,
            Presets,
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
        private float Slider(string name, string id, float current, float min, float max, bool horizontal = true, int digits = 2) {
            var stringFormat = "0." + new string('0', digits);
            if (!sliderTextValues.ContainsKey(id)) {
                sliderTextValues.Add(id, current.ToString(stringFormat));
            }

            if (horizontal) BeginHorizontal();
            Label(name + ":");
            GUILayout.FlexibleSpace();
            var text = GUILayout.TextField(sliderTextValues[id]);
            GUILayout.Space(10);
            Label(min + "", labelStyleMinMax);
            float slider_res = GUILayout.HorizontalSlider(current, min, max, sliderStyle, thumbStyle);
            Label(max + "", labelStyleMinMax);
            if (horizontal) EndHorizontal();

            // if the user has typed
            if (text != sliderTextValues[id]) {
                // update text value
                sliderTextValues[id] = text;
                // if the value is valid
                float text_res;
                if (float.TryParse(text, out text_res) && text_res >= min && text_res <= max) {
                    return text_res;
                }
                else {
                    return current;
                }
            }
            // if the slider has moved
            else if (slider_res != current) {
                sliderTextValues[id] = slider_res.ToString(stringFormat);
                return slider_res;
            }
            // nothing has changed
            else {
                return current;
            }
        }
        private int SliderInt(string name, string id, int current, int min, int max, bool horizontal = true) {
            if (!sliderTextValues.ContainsKey(id)) {
                sliderTextValues.Add(id, current.ToString());
            }

            if (horizontal) BeginHorizontal();
            Label(name + ":");
            GUILayout.FlexibleSpace();
            var text = GUILayout.TextField(sliderTextValues[id]);
            GUILayout.Space(10);
            Label(min + "", labelStyleMinMax);
            int slider_res = Mathf.FloorToInt(GUILayout.HorizontalSlider(current, min, max, sliderStyle, thumbStyle));
            Label(max + "", labelStyleMinMax);
            if (horizontal) EndHorizontal();

            // if the user has typed
            if (text != sliderTextValues[id]) {
                // update text value
                sliderTextValues[id] = text;
                // if the value is valid
                int text_res;
                if (int.TryParse(text, out text_res) && text_res >= min && text_res <= max) {
                    return text_res;
                }
                else {
                    return current;
                }
            }
            else if (slider_res != current) {
                // if the slider has moved
                sliderTextValues[id] = slider_res.ToString();
                return slider_res;
            }
            else {
                return current;
            }
        }

        #endregion
    }
}