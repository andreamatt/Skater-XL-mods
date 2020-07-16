using GameManagement;
using ReplayEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
	public class BabboSettings : MonoBehaviour
	{
		private static BabboSettings _Instance;
		public static BabboSettings Instance {
			get {
				//if (_Instance == null) {
				//    Logger.Log("BabboSettings Instance is null");
				//}
				return _Instance;
			}
		}

		private GameEffects effects;
		private Window window;
		private MapPresetUpdater mapPresetUpdater;
		private CustomCameraController cameraController;
		public PresetsManager presetsManager;
		private LightController lightController;
		//private DayNightController dayNightController;

		public string currentGameStateName;

		public void Start() {
			if (Instance != null) {
				Logger.Log("BabboSettings Instance already exists");
			}
			else {
				_Instance = this;
			}

			DontDestroyOnLoad(gameObject);

			effects = gameObject.AddComponent<GameEffects>();
			mapPresetUpdater = gameObject.AddComponent<MapPresetUpdater>();
			window = gameObject.AddComponent<Window>();
			PatchData.Instance.window = window;
			cameraController = gameObject.AddComponent<CustomCameraController>();
			PatchData.Instance.cameraController = cameraController;
			presetsManager = gameObject.AddComponent<PresetsManager>();
			lightController = gameObject.AddComponent<LightController>();
			//dayNightController = DayNightController.Instance;

			currentGameStateName = GameStateMachine.Instance.CurrentState.GetType().Name;
			effects.checkAndGetEffects();
		}

		private void Update() {
			// if the map changed, needs to find/create new effects
			var newGameStateName = GameStateMachine.Instance.CurrentState.GetType().Name;
			if (newGameStateName != currentGameStateName) {
				currentGameStateName = newGameStateName;
				Main.Save();
			}
			effects.checkAndGetEffects();
		}

		private bool last_IsReplayActive = false;
		private bool first_time_ReplayActive = true;
		public bool IsReplayActive() {
			var active = currentGameStateName == "ReplayState";
			if (active != last_IsReplayActive) {
				if (first_time_ReplayActive) {
					first_time_ReplayActive = false;
					// the replay menu shows the wrong status for keyFrames On/Off the first time it is opened
					ReplayEditorController.Instance.cameraController.useKeyframeIndicator.TurnOff();
				}
				last_IsReplayActive = active;
				// if the status has changed (enter/exit replay editor), apply everything again
				presetsManager.ApplyPresets();
				presetsManager.ApplySettings();
			}
			return active;
		}
	}
}
