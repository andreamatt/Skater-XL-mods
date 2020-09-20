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
	public class CameraModeHandler : EffectHandler
	{
		public override void ConnectUI() {
			var cmUI = CameraModeUI.Instance;

			var mode = Main.settings.cameraData.CAMERA_MODE;
			cmUI.mode.SetValueWithoutNotify((int)mode);
			SetRelativeTab((int)mode, XLGraphics.Instance.IsReplayActive());
			CustomCameraController.Instance.cameraMode = mode;

			cmUI.mode.onValueChanged.AddListener(new UnityAction<int>(v => {
				CustomCameraController.Instance.cameraMode = Main.settings.cameraData.CAMERA_MODE = (CameraMode)v;
				SetRelativeTab(v, XLGraphics.Instance.IsReplayActive());
			}));

			XLGraphics.Instance.onReplayStateChange += () => {
				SetRelativeTab((int)CustomCameraController.Instance.cameraMode, XLGraphics.Instance.IsReplayActive());
			};
		}

		private void SetRelativeTab(int v, bool isReplayActive) {
			// same order as enum
			var modesUIs = new List<GameObject> {
				FollowCameraUI.Instance.gameObject,
				NormalCameraUI.Instance.gameObject,
				PovCameraUI.Instance.gameObject,
				SkateCameraUI.Instance.gameObject
			};

			// deactivate everything
			modesUIs.ForEach(go => go.SetActive(false));
			ReplayFovUI.Instance.gameObject.SetActive(false);
			CameraModeUI.Instance.gameObject.SetActive(false);

			if (isReplayActive) {
				ReplayFovUI.Instance.gameObject.SetActive(true);
			}
			else {
				modesUIs[v].SetActive(true);
				CameraModeUI.Instance.gameObject.SetActive(true);
			}
		}
	}

	public enum CameraMode
	{   // in alphabetical order
		Follow,
		Normal,
		POV,
		Skate
	}
}
