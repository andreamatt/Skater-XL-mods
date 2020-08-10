using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Utils;

namespace XLGraphics.Effects.SettingsEffects
{
	public class RenderDistanceHandler : EffectHandler
	{
		public override void ConnectUI() {
			var distanceSlider = UI.Instance.sliders["RenderDistanceSlider"];
			var distanceSliderValue = UI.Instance.sliderValueTexts["RenderDistanceValue"];

			// init UI values
			var camera = Camera.main;
			camera.farClipPlane = Main.settings.RENDER_DISTANCE;
			distanceSlider.OverrideValue(Main.settings.RENDER_DISTANCE);
			distanceSliderValue.OverrideValue(Main.settings.RENDER_DISTANCE);

			// add listeners
			distanceSlider.ValueChanged += value => {
				camera.farClipPlane = Main.settings.RENDER_DISTANCE = value;
			};
		}
	}
}
