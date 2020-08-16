using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class ShadowsMidtonesHighlightsData
	{
		public bool active;
		public SerializableVector4 shadows = new Vector4();
		public SerializableVector4 midtones = new Vector4();
		public SerializableVector4 highlights = new Vector4();
		public float shadowsStart;
		public float shadowsEnd;
		public float highlightsStart;
		public float highlightsEnd;

		public static ShadowsMidtonesHighlightsData FromPreset(Preset p) {
			return new ShadowsMidtonesHighlightsData() {
				active = p.shadowsMidtonesHighlights.active,
				shadows = p.shadowsMidtonesHighlights.shadows.value,
				midtones = p.shadowsMidtonesHighlights.midtones.value,
				highlights = p.shadowsMidtonesHighlights.highlights.value,
				shadowsStart = p.shadowsMidtonesHighlights.shadowsStart.value,
				shadowsEnd = p.shadowsMidtonesHighlights.shadowsEnd.value,
				highlightsStart = p.shadowsMidtonesHighlights.highlightsStart.value,
				highlightsEnd = p.shadowsMidtonesHighlights.highlightsEnd.value
			};
		}

		public void OverrideValues(Preset p) {
			p.shadowsMidtonesHighlights.active = active;
			p.shadowsMidtonesHighlights.shadows.value = shadows;
			p.shadowsMidtonesHighlights.midtones.value = midtones;
			p.shadowsMidtonesHighlights.highlights.value = highlights;
			p.shadowsMidtonesHighlights.shadowsStart.value = shadowsStart;
			p.shadowsMidtonesHighlights.shadowsEnd.value = shadowsEnd;
			p.shadowsMidtonesHighlights.highlightsStart.value = highlightsStart;
			p.shadowsMidtonesHighlights.highlightsEnd.value = highlightsEnd;
		}
	}
}
