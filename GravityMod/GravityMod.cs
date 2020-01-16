using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

namespace GravityMod
{
    public class GravityMod : MonoBehaviour
    {
        public ModUIBox uiBox;
        public ModUICustom customSlider;
        public GUIStyle labelStyle;
        public GUIStyle labelStyleMinMax;
        public GUIStyle sliderStyle;
        public GUIStyle thumbStyle;
        public Dictionary<string, string> sliderTextValues;
        public string sliderID = "gravityMod_slider";
        bool need_styles = true;


        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("Macks_Babbo", "MacksHeadroom, Babbo Elu");

            customSlider = uiBox.AddCustom(sliderID, () => { drawgui(); }, () => Main.enabled);

            sliderTextValues = new Dictionary<string, string>();
        }

        public void drawgui() {
            if (need_styles) {
                need_styles = false;

                sliderStyle = new GUIStyle(GUI.skin.horizontalSlider) {
                    fixedWidth = 200
                };

                thumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb) {

                };

                labelStyle = new GUIStyle(GUI.skin.label) {
                };

                labelStyleMinMax = new GUIStyle(GUI.skin.label) {
                    normal = {
                    textColor = Color.green
                },
                    fixedWidth = 30
                };
            }

            // draw gui
            Main.settings.G = Slider("Gravity", sliderID, Main.settings.G, -20, 5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Earth")) {
                Main.settings.G = -9.81f;
                sliderTextValues[sliderID] = Main.settings.G.ToString("0.00");
            }
            if (GUILayout.Button("Moon")) {
                Main.settings.G = -1.62f;
                sliderTextValues[sliderID] = Main.settings.G.ToString("0.00");
            }
            if (GUILayout.Button("Mars")) {
                Main.settings.G = -3.77f;
                sliderTextValues[sliderID] = Main.settings.G.ToString("0.00");
            }
            if (GUILayout.Button("-11")) {
                Main.settings.G = -11f;
                sliderTextValues[sliderID] = Main.settings.G.ToString("0.00");
            }
            if (GUILayout.Button("-15")) {
                Main.settings.G = -15f;
                sliderTextValues[sliderID] = Main.settings.G.ToString("0.00");
            }
            GUILayout.EndHorizontal();
        }

        public void Update() {
            if (Main.enabled) {
                Physics.gravity = new Vector3(0, Main.settings.G, 0);
            }
            else {
                Physics.gravity = new Vector3(0, -9.81f, 0);
            }
        }

        public float Slider(string name, string id, float current, float min, float max, bool horizontal = true) {
            if (!sliderTextValues.ContainsKey(id)) {
                sliderTextValues.Add(id, current.ToString("0.00"));
            }

            if (horizontal) GUILayout.BeginHorizontal();
            GUILayout.Label(name + ":", labelStyle);
            GUILayout.FlexibleSpace();
            var text = GUILayout.TextField(sliderTextValues[id]);
            GUILayout.Space(20);
            GUILayout.Label(min + "", labelStyleMinMax);
            float slider_res = GUILayout.HorizontalSlider(current, min, max, sliderStyle, thumbStyle);
            GUILayout.Label(max + "", labelStyleMinMax);
            if (horizontal) GUILayout.EndHorizontal();

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
                sliderTextValues[id] = slider_res.ToString("0.00");
                return slider_res;
            }
            // nothing has changed
            else {
                return current;
            }
        }

        public void OnDestroy() {
            uiBox.RemoveCustom(sliderID);
        }
    }
}

