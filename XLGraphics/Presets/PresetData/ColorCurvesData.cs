using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets.PresetData
{
	public class ColorCurvesData
	{
		public bool active;

		public static ColorCurvesData FromPreset(Preset p) {
			return new ColorCurvesData() {
				active = p.colorCurves.active
			};
		}

		public void OverrideValues(Preset p) {
			p.colorCurves.active = active;
		}
	}
}
