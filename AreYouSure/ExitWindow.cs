using AreYouSure.Patches;
using GameManagement;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLShredLib;

namespace AreYouSure
{
    public class ExitWindow : Window
    {

        protected override void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.Label("ARE YOU SURE YOU WANT TO EXIT THE GAME?", labelStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes, Exit")) {
                Close();
                Application.Quit();
            }
            if (GUILayout.Button("No, Stay HERE")) {
                Close();
            }
            GUILayout.EndHorizontal();
        }
    }
}