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
	class VSyncHandler : EffectHandler
	{

		public override void ConnectUI() {
			var VSNameToValue = new Dictionary<string, int> {
				["VSyncDisabled"] = 0,
				["VSyncFull"] = 1,
				["VSyncHalf"] = 2
			};
			var VSValueToName = VSNameToValue.ToDictionary(kv => kv.Value, kv => kv.Key);

			var VSgrid = UI.Instance.selectionGrids["VSyncGrid"];

			// init UI values
			QualitySettings.vSyncCount = Main.settings.VSYNC;
			VSgrid.OverrideValue(VSValueToName[Main.settings.VSYNC]);

			// add listeners
			VSgrid.ValueChange += name => {
				QualitySettings.vSyncCount = Main.settings.VSYNC = VSNameToValue[name];
			};
		}
	}
}
