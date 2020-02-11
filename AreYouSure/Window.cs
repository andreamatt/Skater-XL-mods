using AreYouSure.Patches;
using GameManagement;
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
        private Rect windowRect = new Rect(Screen.width / 2 - 200f, 200f, 600f, 0f);
        private GUIStyle windowStyle;
        private GUIStyle labelStyle;
        private readonly Color windowColor = new Color(0.8f, 0.8f, 0.8f);
        #endregion

        #region GUI status
        private bool showUI = false;
        private GameObject master;
        private bool setUp;
        #endregion

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

            labelStyle = new GUIStyle(GUI.skin.label) {
            };
        }

        public void Open() {
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

            GUILayout.Label("ARE YOU SURE YOU WANT TO EXIT REPLAY EDITOR?", labelStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes, Exit")) {
                Close();
                var traverse = ReplayState_OnUpdate_Patch.replayStateTraverse;
                var requestTransitionTo = traverse.Method("RequestTransitionTo", new Type[] { typeof(Type) });
                requestTransitionTo.GetValue(typeof(PauseState));
            }
            if (GUILayout.Button("No, Stay HERE")) {
                Close();
            }
            GUILayout.EndHorizontal();
        }
    }
}