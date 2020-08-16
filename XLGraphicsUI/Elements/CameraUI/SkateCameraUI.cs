using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphicsUI.Elements.CameraUI
{
	public class SkateCameraUI : UIsingleton<SkateCameraUI>
	{
		public XLSlider fov;
		public XLSlider react;
		public XLSlider react_rot;
		public XLSlider clip;
		public XLSliderVector3 shift;
	}
}
