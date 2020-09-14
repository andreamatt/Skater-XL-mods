using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class ChannelMixerUI : UIsingleton<ChannelMixerUI>
	{
		public XLToggle toggle;
		//public GameObject container;
		public XLSlider intensity;
		public XLSlider scatter;
		public XLSlider threshold;
		public XLSlider redOutRedIn;
		public XLSlider redOutGreenIn;
		public XLSlider redOutBlueIn;
		public XLSlider greenOutRedIn;
		public XLSlider greenOutGreenIn;
		public XLSlider greenOutBlueIn;
		public XLSlider blueOutRedIn;
		public XLSlider blueOutGreenIn;
		public XLSlider blueOutBlueIn;
	}
}
