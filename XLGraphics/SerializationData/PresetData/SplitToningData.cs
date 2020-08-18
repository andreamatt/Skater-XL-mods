using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class SplitToningData
	{
		public bool active;
		public SerializableColor shadows = Color.black;
		public SerializableColor highlights = Color.grey;
		public float balance;

		public static SplitToningData FromPreset(Preset p) {
			return new SplitToningData() {
				active = p.splitToning.active,
				shadows = p.splitToning.shadows.value,
				highlights = p.splitToning.highlights.value,
				balance = p.splitToning.balance.value
			};
		}

		public void OverrideValues(Preset p) {
			p.splitToning.active = active;
			p.splitToning.shadows.value = shadows;
			p.splitToning.highlights.value = highlights;
			p.splitToning.balance.value = balance;
		}
	}
}
