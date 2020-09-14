using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class LightUI : UIsingleton<LightUI>
	{
		public XLToggle toggle;
		//public GameObject container;
		public XLSlider intensity;
		public XLSlider range;
		public XLSlider angle;
		//public Color tint;
		public XLSliderVector3 position;
		//public string cookie; Choose cookie from image
	}
}
