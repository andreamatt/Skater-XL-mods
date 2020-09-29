using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.CustomEffects;
using XLGraphicsUI.Elements.CameraUI;

namespace XLGraphics.EffectHandlers.CameraEffects
{
	public class ReplayFovHandler : EffectHandler
	{
		public override void ConnectUI() {
			var rfUI = ReplayFovUI.Instance;

			var fov = Main.settings.cameraData.REPLAY_FOV;
			rfUI.fov.OverrideValue(fov);
			CustomCameraController.Instance.replay_fov = fov;

			rfUI.fov.onValueChanged += v => {
				CustomCameraController.Instance.replay_fov = Main.settings.cameraData.REPLAY_FOV = v;
			};
		}
	}
}
