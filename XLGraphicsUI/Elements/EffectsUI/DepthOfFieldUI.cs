using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class DepthOfFieldUI : UIsingleton<DepthOfFieldUI>
	{
		public Toggle toggle;
		public TMP_Dropdown focusMode;
		public XLSlider focusDistance;
		public XLSlider nearFocusStart;
		public XLSlider nearFocusEnd;
		public XLSlider farFocusStart;
		public XLSlider farFocusEnd;
	}
}
