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
		public bool active = false;
		public float intensity = 0.5f;
		public float response = 0.2f;
		public FilmGrainLookup type = FilmGrainLookup.Medium2;

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
