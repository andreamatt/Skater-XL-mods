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
		private float skate_fov = 60;
		private Transform tra = new GameObject().transform;
		private HeadIK headIK = FindObjectOfType<HeadIK>();
		private Transform actualCam = PlayerController.Instance.cameraController._actualCam;
		private float moving_thresh = 0.3f;

		private float interpol = 1;
		private float interpol2 = 1;

		private void follow(float fov, Vector3 shift) {
			Camera.main.fieldOfView = fov;
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
			tra.position = pos;
			tra.rotation = actualCam.rotation;
			Vector3 true_shift = shift;
			if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
				true_shift.x *= -1;
			}
			if (PlayerController.Instance.IsSwitch) {
				true_shift.x *= -1;
			}
			true_shift.x = Mathf.Lerp(old_true_shift_x, true_shift.x, 0.02f);
			old_true_shift_x = true_shift.x;
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
			old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, 1);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, 0.06f);
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
				case CameraMode.Normal:
					normal();
					break;
				case CameraMode.Low:
					follow(low_fov, low_shift);
					break;
				case CameraMode.Follow:
					follow(follow_fov, follow_shift);
					break;
				case CameraMode.POV:
					//pov(); doesn't work on fixed, probably because of animations
					break;
				case CameraMode.Skate:
					skate_pov();
					break;
			}
		}

		private void LateUpdate() {
			if (focus_mode == FocusMode.Player) {
				Vector3 player_pos = PlayerController.Instance.skaterController.skaterTransform.position;
				GAME_DOF.focusDistance.Override(Vector3.Distance(player_pos, Camera.main.transform.position));
			}
			else if (focus_mode == FocusMode.Skate) {
				Vector3 skate_pos = PlayerController.Instance.boardController.boardTransform.position;
				GAME_DOF.focusDistance.Override(Vector3.Distance(skate_pos, Camera.main.transform.position));
			}
			if (cameraMode == CameraMode.POV) {
				pov();
			}
		}
	}
}