using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.CameraUI
{
	public class FollowCameraUI : UIsingleton<FollowCameraUI>
	{
		public XLSlider fov;
		public XLSlider react;
		public XLSlider react_rot;
		public XLSlider clip;
		//public Vector3 shift;
		public Toggle auto_switch;
	}
}
