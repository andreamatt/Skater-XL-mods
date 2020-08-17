using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class SplitToningUI : UIsingleton<SplitToningUI>
	{
		public Toggle toggle;
		public GameObject container;
		//public SerializableColor shadows;
		//public SerializableColor highlights;
		public XLSlider balance;
	}
}
