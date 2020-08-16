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
			var VSdropdown = UI.Instance.dropdowns["VSyncDropdown"];

			// init UI values
			QualitySettings.vSyncCount = Main.settings.VSYNC;
			VSdropdown.OverrideValue(Main.settings.VSYNC);

			// add listeners
			VSdropdown.ValueChange += value => {
				QualitySettings.vSyncCount = Main.settings.VSYNC = value;
			};
		}
	}
}
