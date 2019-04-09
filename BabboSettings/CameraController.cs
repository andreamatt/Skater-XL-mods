using UnityEngine;

namespace BabboSettings {
	internal partial class SettingsGUI : MonoBehaviour {
		private Vector3 follow_shift = new Vector3();
		private Vector3 pov_shift = new Vector3();
		private Vector3 low_shift = new Vector3(0.6f, -0.2f, -0.9f);
		private Vector3 skate_shift = new Vector3();
		private Vector3 old_pos = new Vector3();
		private Quaternion old_rot = new Quaternion();
		private float old_true_shift_x = 0;
		private float normal_fov = 60;
		private float low_fov = 60;
		private float follow_fov = 60;
		private float pov_fov = 60;
		private Transform tra = new GameObject().transform;
		private HeadIK headIK = FindObjectOfType<HeadIK>();
		private Transform actualCam = PlayerController.Instance.cameraController._actualCam;
		private float moving_thresh = 0.3f;

		private void follow() {
			Camera.main.fieldOfView = follow_fov;
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
			tra.position = pos;
			tra.rotation = actualCam.rotation;
			Vector3 true_shift = follow_shift;
			if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
				true_shift.x *= -1;
			}
			if (PlayerController.Instance.IsSwitch) {
				true_shift.x *= -1;
			}
			true_shift.x = old_true_shift_x = Mathf.Lerp(old_true_shift_x, true_shift.x, 0.02f);
			tra.position = tra.TransformPoint(true_shift);

			// if too low, high up a bit
			pos = tra.position;
			pos.y = Mathf.Max(tra.position.y, PlayerController.Instance.boardController.boardTransform.position.y + 0.2f);

			actualCam.position = Vector3.Lerp(old_pos, pos, 0.7f);
			actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, 0.7f);
			old_pos = actualCam.position;
			old_rot = actualCam.rotation;

		}

		private void low() {
			Camera.main.fieldOfView = low_fov;
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
			tra.position = pos;
			tra.rotation = actualCam.rotation;
			Vector3 true_shift = low_shift;
			if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
				true_shift.x *= -1;
			}
			if (PlayerController.Instance.IsSwitch) {
				true_shift.x *= -1;
			}
			true_shift.x = old_true_shift_x = Mathf.Lerp(old_true_shift_x, true_shift.x, 0.02f);
			tra.position = tra.TransformPoint(true_shift);

			// if too low, high up a bit
			pos = tra.position;
			pos.y = Mathf.Max(tra.position.y, PlayerController.Instance.boardController.boardTransform.position.y + 0.2f);

			actualCam.position = Vector3.Lerp(old_pos, pos, 0.7f);
			actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, 0.7f);
			old_pos = actualCam.position;
			old_rot = actualCam.rotation;
		}

		private void pov() {
			Camera.main.fieldOfView = pov_fov;
			actualCam.position = headIK.head.position;
			actualCam.rotation = headIK.head.rotation;
			actualCam.position = actualCam.TransformPoint(pov_shift);
		}

		private void skate_pov() {
			Camera.main.fieldOfView = 80;
			actualCam.position = PlayerController.Instance.boardController.boardTransform.position;
			actualCam.rotation = PlayerController.Instance.boardController.boardTransform.rotation;
			actualCam.position = actualCam.TransformPoint(skate_shift);
		}

		private void normal() {
			Camera.main.fieldOfView = normal_fov;
		}

		private void FixedUpdate() {
			switch (cameraMode) {
				case CameraMode.POV:
					pov();
					break;
				case CameraMode.Follow:
					follow();
					break;
				case CameraMode.Low:
					low();
					break;
				case CameraMode.Normal:
					normal();
					break;
				case CameraMode.Skate:
					skate_pov();
					break;
			}
		}

	}
}