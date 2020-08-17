using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class WhiteBalanceUI : UIsingleton<WhiteBalanceUI>
	{
		public Toggle toggle;
		public GameObject container;
		public XLSlider temperature;
		public XLSlider tint;
	}
}
