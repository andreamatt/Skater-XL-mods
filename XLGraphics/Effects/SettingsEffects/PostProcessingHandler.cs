using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using XLGraphics.Presets;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.Effects.SettingsEffects
{
	public class PostProcessingHandler : EffectHandler
	{
		public override void ConnectUI() {
			var ppUI = PostProcessingUI.Instance;

			ppUI.toggle.SetIsOnWithoutNotify(Main.settings.settingsData.ENABLE_POST);

			ppUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(value => {
				// idea:
				// change settings value, then toggle presets based on that

				Main.settings.settingsData.ENABLE_POST = value;

				//UI.Instance.TogglePresets();
				//PresetManager.Instance.TogglePresets();
			}));
		}
	}
}
