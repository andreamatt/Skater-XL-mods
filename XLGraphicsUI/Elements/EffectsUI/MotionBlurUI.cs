using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class MotionBlurUI : UIsingleton<MotionBlurUI>
	{
		public Toggle toggle;
		public GameObject container;
		public XLSlider intensity;
		public XLSlider sampleCount;
		public XLSlider maximumVelocity;
		public XLSlider minimumVelocity;
		public XLSlider depthComparisonExtent;
		public XLSlider cameraRotationVelocityClamp;
		public Toggle cameraMotionBlur;
	}
}
