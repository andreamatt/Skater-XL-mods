using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class BloomUI : UIsingleton<BloomUI>
	{
		public Toggle toggle;
		public GameObject container;
		public XLSlider intensity;
		public XLSlider threshold;
		public XLSlider scatter;
		//public Color tint;
	}
}
