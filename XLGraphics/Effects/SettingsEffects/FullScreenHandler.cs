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
	class FullScreenHandler : EffectHandler
	{
		public override void ConnectUI() {
			var FSModeNameToValue = new Dictionary<string, FullScreenMode> {
				["FullScreenExclusive"] = FullScreenMode.ExclusiveFullScreen,
				["FullScreenFull"] = FullScreenMode.FullScreenWindow,
				["FullScreenWindowed"] = FullScreenMode.Windowed
			};
			var FSModeValueToName = FSModeNameToValue.ToDictionary(kv => kv.Value, kv => kv.Key);

			var FSmodeGrid = UI.Instance.selectionGrids["FullScreenGrid"];

			// init UI values
			Screen.fullScreenMode = Main.settings.SCREEN_MODE;
			FSmodeGrid.OverrideValue(FSModeValueToName[Main.settings.SCREEN_MODE]);

			// add listeners
			FSmodeGrid.ValueChange += name => {
				Screen.fullScreenMode = Main.settings.SCREEN_MODE = FSModeNameToValue[name];
			};
		}
	}
}
