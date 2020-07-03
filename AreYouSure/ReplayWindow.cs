using AreYouSure.Patches;
using GameManagement;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AreYouSure
{
	public class ReplayWindow : Window
	{

		protected override void RenderWindow(int windowID) {
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
