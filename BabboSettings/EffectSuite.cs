using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
	public class EffectSuite
	{
		public AmbientOcclusion AO;
		public AutoExposure EXPO;
		public Bloom BLOOM;
		public ChromaticAberration CA;
		public ColorGrading COLOR; // NOT SERIALIZABLE
		public DepthOfField DOF;
		public Grain GRAIN;
		public LensDistortion LENS;
		public MotionBlur BLUR;
		public ScreenSpaceReflections REFL;
		public Vignette VIGN;

		public static EffectSuite FromVolume(PostProcessVolume post_volume) {
			if (post_volume == null) {
				Logger.Debug("Post_volume is null in EffectSuite.FromVolume");
				return null;
			}

			var suite = new EffectSuite();
			string not_found = "";
			if ((suite.AO = post_volume.profile.GetSetting<AmbientOcclusion>()) == null) {
				not_found += "ao,";
				suite.AO = post_volume.profile.AddSettings<AmbientOcclusion>();
				suite.AO.enabled.Override(false);
			}
			if ((suite.EXPO = post_volume.profile.GetSetting<AutoExposure>()) == null) {
				not_found += "expo,";
				suite.EXPO = post_volume.profile.AddSettings<AutoExposure>();
				suite.EXPO.enabled.Override(false);
			}
			if ((suite.BLOOM = post_volume.profile.GetSetting<Bloom>()) == null) {
				not_found += "bloom,";
				suite.BLOOM = post_volume.profile.AddSettings<Bloom>();
				suite.BLOOM.enabled.Override(false);
			}
			if ((suite.CA = post_volume.profile.GetSetting<ChromaticAberration>()) == null) {
				not_found += "ca,";
				suite.CA = post_volume.profile.AddSettings<ChromaticAberration>();
				suite.CA.enabled.Override(false);
			}
			if ((suite.COLOR = post_volume.profile.GetSetting<ColorGrading>()) == null) {
				not_found += "color,";
				suite.COLOR = post_volume.profile.AddSettings<ColorGrading>();
				suite.COLOR.enabled.Override(false);
			}
			if ((suite.DOF = post_volume.profile.GetSetting<DepthOfField>()) == null) {
				not_found += "dof,";
				suite.DOF = post_volume.profile.AddSettings<DepthOfField>();
				suite.DOF.enabled.Override(false);
			}
			if ((suite.GRAIN = post_volume.profile.GetSetting<Grain>()) == null) {
				not_found += "grain,";
				suite.GRAIN = post_volume.profile.AddSettings<Grain>();
				suite.GRAIN.enabled.Override(false);
			}
			if ((suite.BLUR = post_volume.profile.GetSetting<MotionBlur>()) == null) {
				not_found += "blur,";
				suite.BLUR = post_volume.profile.AddSettings<MotionBlur>();
				suite.BLUR.enabled.Override(false);
			}
			if ((suite.LENS = post_volume.profile.GetSetting<LensDistortion>()) == null) {
				not_found += "lens,";
				suite.LENS = post_volume.profile.AddSettings<LensDistortion>();
				suite.LENS.enabled.Override(false);
			}
			if ((suite.REFL = post_volume.profile.GetSetting<ScreenSpaceReflections>()) == null) {
				not_found += "refl,";
				suite.REFL = post_volume.profile.AddSettings<ScreenSpaceReflections>();
				suite.REFL.enabled.Override(false);
			}
			if ((suite.VIGN = post_volume.profile.GetSetting<Vignette>()) == null) {
				not_found += "vign,";
				suite.VIGN = post_volume.profile.AddSettings<Vignette>();
				suite.VIGN.enabled.Override(false);
			}

			if (not_found.Length > 0) {
				Logger.Debug("Not found: " + not_found);
			}

			return suite;
		}
	}
}
