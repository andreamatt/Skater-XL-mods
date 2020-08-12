using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.Presets.PresetData
{
	public class ColorAdjustementsData
	{
		public bool active;
		public float postExposure;
		public float contrast;
		public Color colorFilter;
		public float hueShift;
		public float saturation;

		public static ColorAdjustementsData FromPreset(Preset p) {
			return new ColorAdjustementsData() {
				active = p.colorAdjustments.active,
				postExposure = p.colorAdjustments.postExposure.value,
				contrast = p.colorAdjustments.contrast.value,
				colorFilter = p.colorAdjustments.colorFilter.value,
				hueShift = p.colorAdjustments.hueShift.value,
				saturation = p.colorAdjustments.saturation.value
			};
		}

		public void OverrideValues(Preset p) {
			p.colorAdjustments.active = active;
			p.colorAdjustments.postExposure.value = postExposure;
			p.colorAdjustments.contrast.value = contrast;
			p.colorAdjustments.colorFilter.value = colorFilter;
			p.colorAdjustments.hueShift.value = hueShift;
			p.colorAdjustments.saturation.value = saturation;
		}
	}
}
