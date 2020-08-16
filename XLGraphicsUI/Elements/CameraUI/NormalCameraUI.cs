using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphicsUI.Elements.CameraUI
{
	public class NormalCameraUI : UIsingleton<NormalCameraUI>
	{
		public XLSlider fov;
		public XLSlider react;
		public XLSlider react_rot;
		public XLSlider clip;
	}
}
