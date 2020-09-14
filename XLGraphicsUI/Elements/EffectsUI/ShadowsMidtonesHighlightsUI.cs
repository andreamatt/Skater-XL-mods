using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class ShadowsMidtonesHighlightsUI : UIsingleton<ShadowsMidtonesHighlightsUI>
	{
		public XLToggle toggle;
		//public GameObject container;
		//public Vector4 shadows = new Vector4();
		//public Vector4 midtones = new Vector4();
		//public Vector4 highlights = new Vector4();
		public XLSlider shadowsStart;
		public XLSlider shadowsEnd;
		public XLSlider highlightsStart;
		public XLSlider highlightsEnd;
	}
}
