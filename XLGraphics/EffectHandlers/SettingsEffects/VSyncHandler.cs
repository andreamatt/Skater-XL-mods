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

namespace XLGraphics.EffectHandlers.SettingsEffects
{
	class VSyncHandler : EffectHandler
	{

		public override void ConnectUI() {
			var vsUI = VSyncUI.Instance;

			// init UI values
			QualitySettings.vSyncCount = Main.settings.settingsData.VSYNC;
			vsUI.dropdown.SetValueWithoutNotify(Main.settings.settingsData.VSYNC);

			// add listeners
			vsUI.dropdown.onValueChanged.AddListener(new UnityAction<int>(value => {
				QualitySettings.vSyncCount = Main.settings.settingsData.VSYNC = value;
			}));
		}
	}
}
