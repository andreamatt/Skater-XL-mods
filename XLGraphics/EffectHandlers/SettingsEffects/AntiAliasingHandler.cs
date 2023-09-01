using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using UnityModManagerNet;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.EffectHandlers.SettingsEffects
{
	class AntiAliasingHandler : EffectHandler
	{
		private HDAdditionalCameraData cameraData;

		public override void ConnectUI() {
			cameraData = GameStateMachine.Instance.MainPlayer.gameplay.transform.parent.parent.Find("Main Camera").GetComponent<HDAdditionalCameraData>();

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
