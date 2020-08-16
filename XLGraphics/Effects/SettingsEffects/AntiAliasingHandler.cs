using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.Effects.SettingsEffects
{
	class AntiAliasingHandler : EffectHandler
	{
		private HDAdditionalCameraData cameraData;

		public override void ConnectUI() {
			cameraData = Camera.main.GetComponent<HDAdditionalCameraData>();

			var aaUI = AntiAliasingUI.Instance;

			// init UI values
			cameraData.antialiasing = Main.settings.settingsData.AA_MODE;
			aaUI.dropdown.SetValueWithoutNotify((int)Main.settings.settingsData.AA_MODE);

			// add listeners
			aaUI.dropdown.onValueChanged.AddListener(new UnityAction<int>(value => {
				cameraData.antialiasing = Main.settings.settingsData.AA_MODE = (HDAdditionalCameraData.AntialiasingMode)value;
			}));
		}
	}
}
