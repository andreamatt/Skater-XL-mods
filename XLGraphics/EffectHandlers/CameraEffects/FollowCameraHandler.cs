using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using XLGraphics.CustomEffects;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.EffectHandlers.CameraEffects
{
	public class FollowCameraHandler : EffectHandler
	{
		public override void ConnectUI() {
			var fcUI = FollowCameraUI.Instance;

			var ccc = CustomCameraController.Instance;
			ccc.follow_fov = Main.settings.cameraData.FOLLOW_FOV;
			ccc.follow_react = Main.settings.cameraData.FOLLOW_REACT;
			ccc.follow_react_rot = Main.settings.cameraData.FOLLOW_REACT_ROT;
			ccc.follow_clip = Main.settings.cameraData.FOLLOW_CLIP;
			ccc.follow_shift = Main.settings.cameraData.FOLLOW_SHIFT;
			ccc.follow_auto_switch = Main.settings.cameraData.FOLLOW_AUTO_SWITCH;

			fcUI.fov.OverrideValue(ccc.follow_fov);
			fcUI.react.OverrideValue(ccc.follow_react);
			fcUI.react_rot.OverrideValue(ccc.follow_react_rot);
			fcUI.clip.OverrideValue(ccc.follow_clip);
			fcUI.shift.OverrideValue(ccc.follow_shift);
			fcUI.auto_switch.SetIsOnWithoutNotify(ccc.follow_auto_switch);

			fcUI.fov.onValueChanged += v => ccc.follow_fov = Main.settings.cameraData.FOLLOW_FOV = v;
			fcUI.react.onValueChanged += v => ccc.follow_react = Main.settings.cameraData.FOLLOW_REACT = v;
			fcUI.react_rot.onValueChanged += v => ccc.follow_react_rot = Main.settings.cameraData.FOLLOW_REACT_ROT = v;
			fcUI.clip.onValueChanged += v => ccc.follow_clip = Main.settings.cameraData.FOLLOW_CLIP = v;
			fcUI.shift.onValueChanged += v => ccc.follow_shift = Main.settings.cameraData.FOLLOW_SHIFT = v;
			fcUI.auto_switch.onValueChanged += v => ccc.follow_auto_switch = Main.settings.cameraData.FOLLOW_AUTO_SWITCH = v;
		}
	}
}
