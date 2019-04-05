using UnityEngine;

namespace BabboSettings {
	internal partial class SettingsGUI : MonoBehaviour {
		private Vector3 follow_shift = new Vector3();
		private Vector3 pov_shift = new Vector3();
		private GameObject shift_go = new GameObject();
		private HeadIK headIK = FindObjectOfType<HeadIK>();
		private Transform actualCam = PlayerController.Instance.cameraController._actualCam;
		private float lerp = 1f;
		private bool use_pov = true;

		internal void follow() {
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			var vel = PlayerController.Instance.skaterController.skaterRigidbody.velocity;
			vel.y = 0;
			var new_pos = pos;
			if (vel.magnitude > 0.7) {
				vel.Normalize();
				new_pos.x += -1 * vel.x;
				new_pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
				new_pos.z += -1 * vel.z;
				var tra = shift_go.transform;
				tra.position = new_pos;
				tra.rotation = actualCam.rotation;
				tra.position = tra.TransformPoint(follow_shift);
				// if too low, high up a bit
				new_pos = tra.position;
				new_pos.y = Mathf.Max(tra.position.y, PlayerController.Instance.boardController.boardTransform.position.y + 0.2f);
				actualCam.position = new_pos;
			}
			else {
				//PlayerController.Instance.cameraController._actualCam.position = new_pos;
				//PlayerController.Instance.cameraController._actualCam.TransformDirection(new Vector3(3, 0, 0));
			}
		}

		internal void pov() {
			actualCam.position = headIK.head.position;
			actualCam.rotation = headIK.head.rotation;
			actualCam.position = actualCam.TransformPoint(pov_shift);
		}

	}
}