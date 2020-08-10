using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Effects;
using XLGraphics.Effects.PresetEffects;
using XLGraphics.Effects.SettingsEffects;
using XLGraphics.Presets;
using XLGraphics.Utils;
using XLGraphicsUI;

namespace XLGraphics
{
	public class XLGraphics : MonoBehaviour
	{
		public List<PresetEffectHandler> presetEffectHandlers;
		public List<EffectHandler> basicEffectHandlers;
		public List<EffectHandler> cameraEffectHandlers;

		public static XLGraphics Instance { get; private set; }

		public void Start() {
			Logger.Log("Start of XLGraphics");
			Screen.fullScreenMode = FullScreenMode.Windowed;
			Instance = this;

			// load settings
			Main.settings = Settings.Load();
			if (Main.settings.presetOrder == null) {
				throw new Exception("WTF");
			}

			// initiate components
			new VolumeUtils();
			new UI();
			new PresetManager();

			PresetManager.Instance.LoadPresets();

			UI.Instance.CollectElements();
			UI.Instance.AddBaseListeners();
			UI.Instance.AddPresetListeners();

			basicEffectHandlers = new List<EffectHandler> {
				new VSyncHandler(),
				new FullScreenHandler(),
				new AntiAliasingHandler(),
				new RenderDistanceHandler()
			};

			presetEffectHandlers = new List<PresetEffectHandler> {
				new ChromaticAberrationHandler()
			};

			foreach (var eH in basicEffectHandlers) {
				eH.ConnectUI();
			}
			foreach (var eH in presetEffectHandlers) {
				eH.ConnectUI();
			}

			XLGraphicsMenu.Instance.basicContent.SetActive(true);

			Logger.Log("End of XLGraphics");
		}

		public void Update() {
			bool keyUp = Input.GetKeyUp(KeyCode.Backspace);
			if (keyUp) {
				var menu = XLGraphicsMenu.Instance;
				if (menu.gameObject.activeSelf) {
					menu.gameObject.SetActive(false);
					PresetManager.Instance.SaveAllPresets();
					Main.settings.Save();
				}
				else {
					menu.gameObject.SetActive(true);
				}
			}
		}
	}
}
