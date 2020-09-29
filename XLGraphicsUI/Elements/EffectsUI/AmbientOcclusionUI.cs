using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class AmbientOcclusionUI : UIsingleton<AmbientOcclusionUI>
	{
		public XLToggle toggle;
		//public GameObject container;
		public XLSlider intensity;
		public XLSlider directLightingStrength;
		public XLSlider radius;
		public TMP_Dropdown quality;
		public XLSlider maximumRadius;
		public XLToggle fullResolution;
		public XLSlider stepCount;
		public XLToggle temporalAccumulation;
		public XLSlider ghostingReduction;
	}
}
