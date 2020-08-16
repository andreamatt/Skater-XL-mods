using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class PhysicalCameraData
	{
		public bool active;

		public static PhysicalCameraData FromPreset(Preset p) {
			return new PhysicalCameraData() {
			};
		}

		public void OverrideValues(Preset p) {
		}
	}
}
