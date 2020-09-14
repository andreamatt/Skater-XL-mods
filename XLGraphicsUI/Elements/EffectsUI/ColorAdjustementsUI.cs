using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class ColorAdjustementsUI : UIsingleton<ColorAdjustementsUI>
	{
		public XLToggle toggle;
		//public GameObject container;
		public XLSlider postExposure;
		public XLSlider contrast;
		//public SerializableColor colorFilter;
		public XLSlider hueShift;
		public XLSlider saturation;
	}
}
