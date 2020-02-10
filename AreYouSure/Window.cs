using AreYouSure.Patches;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLShredLib;

namespace AreYouSure
{
    public class Window : MonoBehaviour
    {

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
        private bool checkingIfSure => ReplayState_OnUpdate_Patch.checkingIfSure;
        #endregion

        public void Update() {
            bool keyUp = Input.GetKeyUp(KeyCode.F8);
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
        }

        public void OnGUI() {
            if (!setUp) {
                setUp = true;
                SetUp();
            }

            GUI.backgroundColor = windowColor;

            if (showUI) {
                windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Are You Sure ?", windowStyle, GUILayout.Width(400));
            }
        }

        void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            Label("ARE YOU REALLY SURE???");
            BeginHorizontal();
            if (Button("Yes")) {

            }
            if (Button("No")) {

            }
            EndHorizontal();
        }

        #region GUI Utility

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

        #endregion
    }
}