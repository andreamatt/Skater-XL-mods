using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.CameraUI
{
	public class PovCameraUI : UIsingleton<PovCameraUI>
	{
		public XLSlider fov;
		public XLSlider react;
		public XLSlider react_rot;
		public XLSlider clip;
		public XLSliderVector3 shift;
		public Toggle hide_head;
	}
}
