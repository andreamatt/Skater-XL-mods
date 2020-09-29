using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.CustomEffects;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.EffectHandlers.CameraEffects
{
	public class SkateCameraHandler : EffectHandler
	{
		public override void ConnectUI() {
			var scUI = SkateCameraUI.Instance;

			var ccc = CustomCameraController.Instance;
			ccc.skate_fov = Main.settings.cameraData.SKATE_FOV;
			ccc.skate_react = Main.settings.cameraData.SKATE_REACT;
			ccc.skate_react_rot = Main.settings.cameraData.SKATE_REACT_ROT;
			ccc.skate_clip = Main.settings.cameraData.SKATE_CLIP;
			ccc.skate_shift = Main.settings.cameraData.SKATE_SHIFT;

			scUI.fov.OverrideValue(ccc.skate_fov);
			scUI.react.OverrideValue(ccc.skate_react);
			scUI.react_rot.OverrideValue(ccc.skate_react_rot);
			scUI.clip.OverrideValue(ccc.skate_clip);
			scUI.shift.OverrideValue(ccc.skate_shift);

			scUI.fov.onValueChanged += v => ccc.skate_fov = Main.settings.cameraData.SKATE_FOV = v;
			scUI.react.onValueChanged += v => ccc.skate_react = Main.settings.cameraData.SKATE_REACT = v;
			scUI.react_rot.onValueChanged += v => ccc.skate_react_rot = Main.settings.cameraData.SKATE_REACT_ROT = v;
			scUI.clip.onValueChanged += v => ccc.skate_clip = Main.settings.cameraData.SKATE_CLIP = v;
			scUI.shift.onValueChanged += v => ccc.skate_shift = Main.settings.cameraData.SKATE_SHIFT = v;
		}
	}
}
