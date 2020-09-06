using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using XLGraphics.Presets;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.EffectHandlers.SettingsEffects
{
	public class PostProcessingHandler : EffectHandler
	{
		public override void ConnectUI() {
			var ppUI = PostProcessingUI.Instance;

			ppUI.toggle.SetIsOnWithoutNotify(Main.settings.settingsData.ENABLE_POST);
			PresetManager.Instance.SetActivePostProcessing(Main.settings.settingsData.ENABLE_POST);

			ppUI.toggle.onValueChanged.AddListener(new UnityAction<bool>(value => {
				Main.settings.settingsData.ENABLE_POST = value;
				PresetManager.Instance.SetActivePostProcessing(value);
			}));
		}
	}
}
