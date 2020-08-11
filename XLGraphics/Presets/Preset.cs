using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Presets.PresetData;

namespace XLGraphics.Presets
{
	public class Preset
	{
		public string name = "testname";

		[JsonIgnore]
		public Volume volume;

		[JsonIgnore]
		public Bloom bloom;
		public BloomData bloomData = new BloomData();
		[JsonIgnore]
		public ChannelMixer channelMixer;
		[JsonIgnore]
		public ChromaticAberration chromaticAberration;
		public ChromaticAberrationData chromaticAberrationData = new ChromaticAberrationData();
		[JsonIgnore]
		public ColorAdjustments colorAdjustments;
		[JsonIgnore]
		public ColorCurves colorCurves;
		[JsonIgnore]
		public DepthOfField depthOfField;
		[JsonIgnore]
		public FilmGrain filmGrain;
		[JsonIgnore]
		public LensDistortion lensDistortion;
		[JsonIgnore]
		public LiftGammaGain liftGammaGain;
		[JsonIgnore]
		public MotionBlur motionBlur;
		[JsonIgnore]
		public PaniniProjection paniniProjection;
		[JsonIgnore]
		public ShadowsMidtonesHighlights shadowsMidtonesHighlights;
		[JsonIgnore]
		public SplitToning splitToning;
		[JsonIgnore]
		public Tonemapping tonemapping;
		[JsonIgnore]
		public Vignette vignette;
		[JsonIgnore]
		public WhiteBalance whiteBalance;

		public Preset(string name) {
			this.name = name;
		}

		public Preset() { }
	}
}
