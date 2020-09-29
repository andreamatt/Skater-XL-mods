using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.CustomEffects;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.EffectHandlers.CameraEffects
{
	public class NormalCameraHandler : EffectHandler
	{
		public override void ConnectUI() {
			var ncUI = NormalCameraUI.Instance;

			var ccc = CustomCameraController.Instance;
			ccc.normal_fov = Main.settings.cameraData.NORMAL_FOV;
			ccc.normal_react = Main.settings.cameraData.NORMAL_REACT;
			ccc.normal_react_rot = Main.settings.cameraData.NORMAL_REACT_ROT;
			ccc.normal_clip = Main.settings.cameraData.NORMAL_CLIP;

			ncUI.fov.OverrideValue(ccc.normal_fov);
			ncUI.react.OverrideValue(ccc.normal_react);
			ncUI.react_rot.OverrideValue(ccc.normal_react_rot);
			ncUI.clip.OverrideValue(ccc.normal_clip);

			ncUI.fov.onValueChanged += v => ccc.normal_fov = Main.settings.cameraData.NORMAL_FOV = v;
			ncUI.react.onValueChanged += v => ccc.normal_react = Main.settings.cameraData.NORMAL_REACT = v;
			ncUI.react_rot.onValueChanged += v => ccc.normal_react_rot = Main.settings.cameraData.NORMAL_REACT_ROT = v;
			ncUI.clip.onValueChanged += v => ccc.normal_clip = Main.settings.cameraData.NORMAL_CLIP = v;
		}
	}
}
