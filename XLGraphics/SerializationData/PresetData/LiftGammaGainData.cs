using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class LiftGammaGainData
	{
		public bool active;
		public SerializableVector4 lift = new Vector4();
		public SerializableVector4 gamma = new Vector4();
		public SerializableVector4 gain = new Vector4();

		public static LiftGammaGainData FromPreset(Preset p) {
			return new LiftGammaGainData() {
				active = p.liftGammaGain.active,
				lift = p.liftGammaGain.lift.value,
				gamma = p.liftGammaGain.gamma.value,
				gain = p.liftGammaGain.gain.value
			};
		}

		public void OverrideValues(Preset p) {
			p.liftGammaGain.active = active;
			p.liftGammaGain.lift.value = lift;
			p.liftGammaGain.gamma.value = gamma;
			p.liftGammaGain.gain.value = gain;
		}
	}
}
