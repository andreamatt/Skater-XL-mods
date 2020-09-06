using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class DepthOfFieldData
	{
		public bool active = false;
		public FocusMode focusMode = FocusMode.Manual;
		// keep medium quality for now
		[JsonIgnore]
		public readonly int quality = (int)ScalableSettingLevelParameter.Level.Medium;
		public float focusDistance = 10;
		public float nearFocusStart = 0;
		public float nearFocusEnd = 4;
		public float farFocusStart = 10;
		public float farFocusEnd = 20;

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
			p.depthOfField.quality.value = quality;
			p.depthOfField.focusDistance.value = focusDistance;
			p.depthOfField.nearFocusStart.value = nearFocusStart;
			p.depthOfField.nearFocusEnd.value = nearFocusEnd;
			p.depthOfField.farFocusStart.value = farFocusStart;
			p.depthOfField.farFocusEnd.value = farFocusEnd;
		}
	}
}
