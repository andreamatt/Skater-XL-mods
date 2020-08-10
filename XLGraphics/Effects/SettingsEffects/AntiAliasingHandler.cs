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

			var AAModeNameToValue = new Dictionary<string, HDAdditionalCameraData.AntialiasingMode> {
				["AntiAliasingNone"] = HDAdditionalCameraData.AntialiasingMode.None,
				["AntiAliasingFXAA"] = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing,
				["AntiAliasingTAA"] = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing,
				["AntiAliasingSMAA"] = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing
			};
			var AAModeValueToName = AAModeNameToValue.ToDictionary(kv => kv.Value, kv => kv.Key);

			var AAmodeGrid = UI.Instance.selectionGrids["AntiAliasingGrid"];

			// init UI values
			cameraData.antialiasing = Main.settings.AA_MODE;
			AAmodeGrid.OverrideValue(AAModeValueToName[Main.settings.AA_MODE]);

			// add listeners
			AAmodeGrid.ValueChange += name => {
				cameraData.antialiasing = Main.settings.AA_MODE = AAModeNameToValue[name];
			};
		}
	}
}
