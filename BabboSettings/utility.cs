using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XLShredLib;

namespace BabboSettings {
	internal partial class SettingsGUI : MonoBehaviour {
		// GUI stuff
		private bool showUI = false;
		private GameObject master;
		private bool setUp;
		private Rect windowRect = new Rect(50f, 50f, 600f, 0f);
		private GUIStyle windowStyle;
		private GUIStyle spoilerBtnStyle;
		private GUIStyle sliderStyle;
		private GUIStyle thumbStyle;
		private readonly Color windowColor = new Color(0.2f, 0.2f, 0.2f);
		private string separator;
		private GUIStyle separatorStyle;
		private Vector2 scrollPosition = new Vector2();
		private GUIStyle usingStyle;


		internal SettingsGUI() {

		}

		internal void Start() {
			getSettings();
		}

		internal void log(string s) {
			Main.log(s);
		}

		void show(string s) {
			ModMenu.Instance.ShowMessage(s);
		}

		private void SetUp() {
			DontDestroyOnLoad(gameObject);
			master = GameObject.Find("Master Prefab");
			DontDestroyOnLoad(master);

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

			};
			separatorStyle.normal.textColor = Color.red;
			separatorStyle.fontSize = 4;

			separator = new string('_', 188);

			usingStyle = new GUIStyle(GUI.skin.label);
			usingStyle.normal.textColor = Color.red;
			usingStyle.fontSize = 16;
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
			if (post_volume == null) {
				if (Main.settings.DEBUG) log("Post volume is null (probably map changed)");
				post_volume = FindObjectOfType<PostProcessVolume>();
				if (post_volume == null) {
					if (Main.settings.DEBUG) log("Post volume not found => creating");
					GameObject post_vol_go = new GameObject();
					post_vol_go.layer = 8;
					post_volume = post_vol_go.AddComponent<PostProcessVolume>();
					post_volume.profile = new PostProcessProfile();
					post_volume.isGlobal = true;
					if (Main.settings.DEBUG) log("Now a & e:" + post_volume.isActiveAndEnabled);
					if (Main.settings.DEBUG) log("Has profile: " + post_volume.HasInstantiatedProfile());
				}
				getSettings();
			}
			if (showUI) {
				windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Graphic Settings by Babbo", windowStyle, GUILayout.Width(400));
			}
		}

		private T DeepClone<T>(T obj) {
			var ms = new MemoryStream();
			var formatter = new XmlSerializer(typeof(T));
			try {
				formatter.Serialize(ms, obj);
				ms.Position = 0;
			}
			catch (Exception ex) {
				log(ex.Message);
			}
			return (T)formatter.Deserialize(ms);
		}

		public bool isSwitch() {
			return customCameraController.isSwitch();
		}

		internal void SetJustRespawned() {
			customCameraController.just_respawned = true;
		}

		public void SetSpawnSwitch() {
			customCameraController.spawn_switch = isSwitch();
		}
	}

	public enum CameraMode {
		Normal,
		Low,
		Follow,
		POV,
		Skate
	}

	public enum FocusMode {
		Custom,
		Player,
		Skate
	}
}