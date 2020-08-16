using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class VignetteUI : UIsingleton<VignetteUI>
	{
		public Toggle toggle;
		//public SerializableColor color;
		public XLSlider intensity;
		public XLSlider smoothness;
		public XLSlider roundness;
		public Toggle rounded;
	}
}
