using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using XLGraphics.CustomEffects;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.EffectHandlers.CameraEffects
{
	public class PovCameraHandler : EffectHandler
	{
		public override void ConnectUI() {
			var pcUI = PovCameraUI.Instance;

			var ccc = CustomCameraController.Instance;
			ccc.pov_fov = Main.settings.cameraData.POV_FOV;
			ccc.pov_react = Main.settings.cameraData.POV_REACT;
			ccc.pov_react_rot = Main.settings.cameraData.POV_REACT_ROT;
			ccc.pov_clip = Main.settings.cameraData.POV_CLIP;
			ccc.pov_shift = Main.settings.cameraData.POV_SHIFT;
			ccc.hide_head = Main.settings.cameraData.HIDE_HEAD;

			pcUI.fov.OverrideValue(ccc.pov_fov);
			pcUI.react.OverrideValue(ccc.pov_react);
			pcUI.react_rot.OverrideValue(ccc.pov_react_rot);
			pcUI.clip.OverrideValue(ccc.pov_clip);
			pcUI.shift.OverrideValue(ccc.pov_shift);
			pcUI.hide_head.SetIsOnWithoutNotify(ccc.hide_head);

			pcUI.fov.onValueChanged += v => ccc.pov_fov = Main.settings.cameraData.POV_FOV = v;
			pcUI.react.onValueChanged += v => ccc.pov_react = Main.settings.cameraData.POV_REACT = v;
			pcUI.react_rot.onValueChanged += v => ccc.pov_react_rot = Main.settings.cameraData.POV_REACT_ROT = v;
			pcUI.clip.onValueChanged += v => ccc.pov_clip = Main.settings.cameraData.POV_CLIP = v;
			pcUI.shift.onValueChanged += v => ccc.pov_shift = Main.settings.cameraData.POV_SHIFT = v;
			pcUI.hide_head.onValueChanged += v => ccc.hide_head = Main.settings.cameraData.HIDE_HEAD = v;

		}
	}
}
