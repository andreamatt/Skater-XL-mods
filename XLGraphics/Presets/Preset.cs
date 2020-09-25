using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.SerializationData.PresetData;
using XLGraphicsUI.Elements;

namespace XLGraphics.Presets
{
	public class Preset
	{
		[JsonIgnore]
		public string name = "testname";

		[JsonIgnore]
		public Volume volume;
		[JsonIgnore]
		public bool enabled {
			get {
				return volume.isActiveAndEnabled;
			}
			set {
				volume.enabled = value;
			}
		}
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
		public FocusMode focusMode;
		public DepthOfFieldData depthOfFieldData = new DepthOfFieldData();
		[JsonIgnore]
		public FilmGrain filmGrain;
		public FilmGrainData filmGrainData = new FilmGrainData();
		[JsonIgnore]
		public LensDistortion lensDistortion;
		public LensDistortionData lensDistortionData = new LensDistortionData();
		[JsonIgnore]
		public LiftGammaGain liftGammaGain;
		public LiftGammaGainData liftGammaGainData = new LiftGammaGainData();
		[JsonIgnore]
		public MotionBlur motionBlur;
		public MotionBlurData motionBlurData = new MotionBlurData();
		[JsonIgnore]
		public PaniniProjection paniniProjection;
		public PaniniProjectionData paniniProjectionData = new PaniniProjectionData();
		[JsonIgnore]
		public ShadowsMidtonesHighlights shadowsMidtonesHighlights;
		public ShadowsMidtonesHighlightsData shadowsMidtonesHighlightsData = new ShadowsMidtonesHighlightsData();
		[JsonIgnore]
		public SplitToning splitToning;
		public SplitToningData splitToningData = new SplitToningData();
		[JsonIgnore]
		public Tonemapping tonemapping;
		public ToneMappingData toneMappingData = new ToneMappingData();
		[JsonIgnore]
		public Vignette vignette;
		public VignetteData vignetteData = new VignetteData();
		[JsonIgnore]
		public WhiteBalance whiteBalance;
		public WhiteBalanceData whiteBalanceData = new WhiteBalanceData();

		// Data only fields (no need to save/load from an effect)
		public LightData lightData = new LightData();
		public FovOverrideData fovOverrideData = new FovOverrideData();
		public PhysicalCameraData physicalCameraData = new PhysicalCameraData();
	}
}
