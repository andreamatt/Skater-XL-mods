using AreYouSure.Patches;
using GameManagement;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AreYouSure
{
	public abstract class Window : MonoBehaviour
	{

		#region GUI style
		protected Rect windowRect = new Rect(Screen.width / 2 - 200f, 200f, 600f, 0f);
		protected GUIStyle windowStyle;
		protected GUIStyle labelStyle;
		protected readonly Color windowColor = new Color(0.8f, 0.8f, 0.8f);
		#endregion

		#region GUI status
		protected bool showUI = false;
		protected GameObject master;
		protected bool setUp;
		#endregion

		protected void SetUp() {
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
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		protected void Close() {
			showUI = false;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.None;
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

		protected abstract void RenderWindow(int windowID);
	}
}
