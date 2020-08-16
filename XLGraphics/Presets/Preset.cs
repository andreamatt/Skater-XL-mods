using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Effects.PresetEffects;
using XLGraphics.Presets.PresetData;
using XLGraphicsUI.Elements;

namespace XLGraphics.Presets
{
	public class Preset
	{
		public string name = "testname";

		[JsonIgnore]
		public Volume volume;
		[JsonIgnore]
		public XLPreset presetUI;

		[JsonIgnore]
		public Bloom bloom;
		public BloomData bloomData = new BloomData();
		[JsonIgnore]
		public ChannelMixer channelMixer;
		public ChannelMixerData channelMixerData = new ChannelMixerData();
		[JsonIgnore]
		public ChromaticAberration chromaticAberration;
		public ChromaticAberrationData chromaticAberrationData = new ChromaticAberrationData();
		[JsonIgnore]
		public ColorAdjustments colorAdjustments;
		public ColorAdjustementsData colorAdjustementsData = new ColorAdjustementsData();
		[JsonIgnore]
		public ColorCurves colorCurves;
		public ColorCurvesData colorCurvesData = new ColorCurvesData();
		[JsonIgnore]
		public DepthOfField depthOfField;
		public DepthOfFieldData depthOfFieldData = new DepthOfFieldData();
		public FocusMode focusMode;
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
