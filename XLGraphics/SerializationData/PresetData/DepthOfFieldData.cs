using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Effects.PresetEffects;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class DepthOfFieldData
	{
		public bool active;
		public FocusMode focusMode;
		public float focusDistance;
		public float nearFocusStart;
		public float nearFocusEnd;
		public float farFocusStart;
		public float farFocusEnd;

		public static DepthOfFieldData FromPreset(Preset p) {
			return new DepthOfFieldData() {
				active = p.depthOfField.active,
				focusMode = p.focusMode,
				focusDistance = p.depthOfField.focusDistance.value,
				nearFocusStart = p.depthOfField.nearFocusStart.value,
				nearFocusEnd = p.depthOfField.nearFocusEnd.value,
				farFocusStart = p.depthOfField.farFocusStart.value,
				farFocusEnd = p.depthOfField.farFocusEnd.value,
			};
		}

		public void OverrideValues(Preset p) {
			p.depthOfField.active = active;
			p.focusMode = focusMode;
			p.depthOfField.focusDistance.value = focusDistance;
			p.depthOfField.nearFocusStart.value = nearFocusStart;
			p.depthOfField.nearFocusEnd.value = nearFocusEnd;
			p.depthOfField.farFocusStart.value = farFocusStart;
			p.depthOfField.farFocusEnd.value = farFocusEnd;
		}
	}
}
