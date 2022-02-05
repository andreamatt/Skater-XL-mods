using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.CustomEffects;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.EffectHandlers.SettingsEffects
{
	public class RenderDistanceHandler : EffectHandler
	{
		public override void ConnectUI() {
			var rdUI = RenderDistanceUI.Instance;

			// init UI values
			var camera = CustomCameraController.Instance.mainCamera;
			camera.m_Lens.FarClipPlane = Main.settings.settingsData.RENDER_DISTANCE;
			rdUI.slider.OverrideValue(Main.settings.settingsData.RENDER_DISTANCE);

			// add listeners
			rdUI.slider.onValueChanged += value => {
				camera.m_Lens.FarClipPlane = Main.settings.settingsData.RENDER_DISTANCE = value;
			};
		}
	}
}
