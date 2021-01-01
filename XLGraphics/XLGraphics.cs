using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.EffectHandlers;
using XLGraphics.EffectHandlers.CameraEffects;
using XLGraphics.CustomEffects;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.EffectHandlers.SettingsEffects;
using XLGraphics.Presets;
using XLGraphics.Utils;
using XLGraphicsUI;
using UnityEngine.EventSystems;
using XLGraphicsUI.Elements;

namespace XLGraphics
{
	public class XLGraphics : MonoBehaviour
	{
		public List<PresetEffectHandler> presetEffectHandlers;
		public List<EffectHandler> basicEffectHandlers;
		public List<EffectHandler> cameraEffectHandlers;

		public static XLGraphics Instance { get; private set; }
		public string currentGameStateName;
		private bool isOpen = false;
		private bool last_IsReplayActive = false;
		public event Action OnReplayStateChange = () => { };

		public void Awake() {
			if (Instance != null) {
				throw new Exception("Instance of XLGraphics not null on Awake");
			}
			Instance = this;
		}

		public void Start() {
			Main.Logger.Log("Start of XLGraphics");

			// load settings
			Main.settings = Settings.Load();

			// initialize components
			PresetManager.Instantiate();
			UI.Instantiate();
			VolumeUtils.Instantiate();
			gameObject.AddComponent<CustomCameraController>();
			gameObject.AddComponent<CustomDofController>();
			gameObject.AddComponent<CustomLightController>();
			gameObject.AddComponent<CustomPhysicalCameraController>();

			PresetManager.Instance.LoadPresets();

			// prepare UI
			UI.Instance.CollectElements(false);
			UI.Instance.AddBaseListeners();
			UI.Instance.AddPresetListeners();

			basicEffectHandlers = new List<EffectHandler> {
				new PostProcessingHandler(),
				new VSyncHandler(),
				new FullScreenHandler(),
				new AntiAliasingHandler(),
				new RenderDistanceHandler(),
				new OverlaysHandler()
			};

			presetEffectHandlers = new List<PresetEffectHandler> {
				new AmbientOcclusionHandler(),
				new BloomHandler(),
				new ChromaticAberrationHandler(),
				new DepthOfFieldHandler(),
				new FilmGrainHandler(),
				new FovOverrideHandler(),
				new LensDistortionHandler(),
				new LightHandler(),
				new MotionBlurHandler(),
				new PhysicalCameraHandler(),
				new VignetteHandler()
			};

			cameraEffectHandlers = new List<EffectHandler> {
				new CameraModeHandler(),
				new ReplayFovHandler(),
				new FollowCameraHandler(),
				new NormalCameraHandler(),
				new PovCameraHandler(),
				new SkateCameraHandler()
			};

			foreach (var eH in basicEffectHandlers) {
				eH.ConnectUI();
			}
			foreach (var eH in presetEffectHandlers) {
				eH.ConnectUI();
			}
			foreach (var eH in cameraEffectHandlers) {
				eH.ConnectUI();
			}

			XLGraphicsMenu.Instance.basicContent.SetActive(true);
			XLGraphicsMenu.Instance.presetList.SetActive(true);
			XLGraphicsMenu.Instance.main.SetActive(false);

			Main.Logger.Log("End of XLGraphics");
		}

		public void Update() {
			Cursor.lockState = CursorLockMode.None;
			bool keyUp = Input.GetKeyUp(KeyCode.Backspace);
			if (keyUp) {
				if (isOpen && !XLInputField.anyFocused) {
					XLGraphicsMenu.Instance.main.SetActive(false);
					PresetManager.Instance.SaveAllPresets();
					Main.settings.Save();

					Cursor.visible = false;
				}
				else {
					XLGraphicsMenu.Instance.main.SetActive(true);
				}
				isOpen = !isOpen;
			}

			if (isOpen) {
				Cursor.visible = true;
			}

			// if the map changed, needs to find/create new effects
			var newGameStateName = GameStateMachine.Instance.CurrentState.GetType().Name;
			if (newGameStateName != currentGameStateName) {
				currentGameStateName = newGameStateName;
				//Main.Save(); WHY SAVING???
				Main.Logger.Log("State: " + currentGameStateName);
			}
		}

		public bool IsReplayActive() {
			var active = currentGameStateName == "ReplayState";
			if (active != last_IsReplayActive) {
				last_IsReplayActive = active;
				OnReplayStateChange.Invoke();
			}
			return active;
		}

		public void OnDestroy() {
			foreach (var preset in PresetManager.Instance.presets) {
				DestroyImmediate(preset.volume.gameObject);
			}
			DestroyImmediate(PresetManager.Instance.overriderVolume.gameObject);
		}
	}
}
