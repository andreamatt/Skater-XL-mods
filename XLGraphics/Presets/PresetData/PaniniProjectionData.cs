using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets.PresetData
{
	public class PaniniProjectionData
	{

		public bool active;
		public float distance;
		public float cropToFit;


		public static PaniniProjectionData FromPreset(Preset p) {
			return new PaniniProjectionData() {
				active = p.paniniProjection.active,
				distance = p.paniniProjection.distance.value,
				cropToFit = p.paniniProjection.cropToFit.value
			};
		}

		public void OverrideValues(Preset p) {
			p.paniniProjection.active = active;
			p.paniniProjection.distance.value = distance;
			p.paniniProjection.cropToFit.value = cropToFit;
		}
	}
}
