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

namespace XLGraphics
{
	public class XLGraphics : MonoBehaviour
	{
		VolumeUtils volumeUtils;
		UI ui;
		List<PresetEffectHandler> presetEffectHandlers;
		List<EffectHandler> basicEffectHandlers;
		List<EffectHandler> cameraEffectHandlers;

		public static XLGraphics Instance { get; private set; }

		public void Start() {
			Logger.Log("Start of XLGraphics");
			Instance = this;

			ui = new UI();
			ui.CollectElements();

			var presets = new List<Preset> {
				new Preset(),
				new Preset(),
				new Preset(),
				new Preset(),
				new Preset(),
				new Preset()
			};
			ui.PopulatePresetsList(presets);

			basicEffectHandlers = new List<EffectHandler> {
				new VSyncHandler(),
				new FullScreenHandler(),
				new AntiAliasingHandler()
			};

			presetEffectHandlers = new List<PresetEffectHandler> {
				new ChromaticAberrationHandler()
			};

			foreach (var eH in basicEffectHandlers) {
				eH.ConnectUI(ui);
			}

			//// find all volume priorities
			//volumeUtils = new VolumeUtils(this);
			//var minPrior = volumeUtils.GetHighestPriority();
			//Logger.Log("highest prio is: " + minPrior);

			////var random = new System.Random();
			//for (int i = 0; i < 10; i++) {
			//	var volume = volumeUtils.CreateVolume(-1 * i);

			Camera.main.GetComponent<HDAdditionalCameraData>().antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
			//Camera.main.GetComponent<HDAdditionalCameraData>().antialiasingQuality = AntialiasingQuality.High;

			//	DontDestroyOnLoad(profile);
			//	volume.profile = profile;

			//	var CA = profile.Add<ChromaticAberration>();
			//	CA.active = true;
			//	CA.intensity.value = i / 10;
			//	CA.SetAllOverridesTo(true);

			//}
			////throw new Exception("WTF");

			Logger.Log("End of XLGraphics");
		}

		public void Update() {

		}
	}
}
