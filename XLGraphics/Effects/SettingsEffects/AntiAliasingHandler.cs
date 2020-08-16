using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Utils;

namespace XLGraphics.Effects.SettingsEffects
{
	class AntiAliasingHandler : EffectHandler
	{
		private HDAdditionalCameraData cameraData;

		public override void ConnectUI() {
			cameraData = Camera.main.GetComponent<HDAdditionalCameraData>();

			var AAdropdown = UI.Instance.dropdowns["AntiAliasingDropdown"];

			// init UI values
			cameraData.antialiasing = Main.settings.AA_MODE;
			AAdropdown.OverrideValue((int)Main.settings.AA_MODE);

			// add listeners
			AAdropdown.ValueChange += value => {
				cameraData.antialiasing = Main.settings.AA_MODE = (HDAdditionalCameraData.AntialiasingMode)value;
			};
		}
	}
}
