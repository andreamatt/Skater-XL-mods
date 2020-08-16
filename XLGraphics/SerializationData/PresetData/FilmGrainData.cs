using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets;

namespace XLGraphics.SerializationData.PresetData
{
	public class FilmGrainData
	{
		public bool active;
		public float intensity;
		public float response;
		public FilmGrainLookup type;

		public static FilmGrainData FromPreset(Preset p) {
			return new FilmGrainData() {
				active = p.filmGrain.active,
				type = p.filmGrain.type.value,
				intensity = p.filmGrain.intensity.value,
				response = p.filmGrain.response.value
			};
		}

		public void OverrideValues(Preset p) {
			p.filmGrain.active = active;
			p.filmGrain.type.value = type;
			p.filmGrain.intensity.value = intensity;
			p.filmGrain.response.value = response;
		}
	}
}
