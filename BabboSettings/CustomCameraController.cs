using System.Collections.Generic;
using UnityEngine;

namespace BabboSettings {
	internal class CustomCameraController : MonoBehaviour {
		internal Camera mainCamera = Camera.main;
		internal CameraMode cameraMode = CameraMode.Normal;

		internal Vector3 follow_shift = new Vector3();
		internal Vector3 pov_shift = new Vector3();
		private Vector3 low_shift = new Vector3(0.6f, -0.2f, -0.9f);
		internal Vector3 skate_shift = new Vector3();
		internal Vector3 old_pos = new Vector3();
		internal Quaternion old_rot = new Quaternion();
		internal float old_true_shift_x = 0;
		internal float normal_fov = 60;
		internal float normal_react = 0.90f;
		internal float normal_react_rot = 0.90f;
		internal float normal_clip = 0.01f;
		internal float follow_fov = 60;
		internal float follow_react = 0.70f;
		internal float follow_react_rot = 0.70f;
		internal float follow_clip = 0.01f;
		internal float pov_fov = 60;
		internal float pov_react = 1;
		internal float pov_react_rot = 0.07f;
		internal float pov_clip = 0.01f;
		internal bool hide_head = true;
		internal float skate_fov = 60;
		internal float skate_react = 0.90f;
		internal float skate_react_rot = 0.90f;
		internal float skate_clip = 0.01f;
		internal Transform tra = new GameObject().transform;
		internal HeadIK headIK = FindObjectOfType<HeadIK>();
		internal Transform actualCam = PlayerController.Instance.cameraController._actualCam;
		public Transform switch_transform = new GameObject().transform;
		public bool last_is_switch = false;
		public bool spawn_switch = false;
		public bool just_respawned = true;
		internal List<Material> head_materials = new List<Material>();
		internal Shader standard_shader;
		internal Shader head_shader;

		internal CustomCameraController() {
		}

		private void low() {
			mainCamera.nearClipPlane = 0.01f;
			mainCamera.fieldOfView = 65;
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
			if (tra == null) {
				if (Main.settings.DEBUG) log("Null transform in low");
				tra = new GameObject().transform;
			}
			tra.position = pos;
			tra.rotation = actualCam.rotation;
			Vector3 true_shift = low_shift;
			if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
				true_shift.x *= -1;
			}
			if (PlayerController.Instance.IsSwitch) {
				true_shift.x *= -1;
			}
			old_true_shift_x = true_shift.x = Mathf.Lerp(old_true_shift_x, true_shift.x, 0.02f);
			tra.position = tra.TransformPoint(true_shift);

			// if too low, high up a bit
			pos = tra.position;
			pos.y = Mathf.Max(tra.position.y, PlayerController.Instance.boardController.boardTransform.position.y + 0.2f);

			old_pos = actualCam.position = Vector3.Lerp(old_pos, pos, 0.7f);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, 0.7f);
		}

		private void follow() {
			mainCamera.nearClipPlane = follow_clip;
			mainCamera.fieldOfView = follow_fov;
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
			if (tra == null) {
				if (Main.settings.DEBUG) log("Null transform in follow");
				tra = new GameObject().transform;
			}
			tra.position = pos;
			tra.rotation = actualCam.rotation;
			Vector3 true_shift = follow_shift;
			if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
				true_shift.x *= -1;
			}
			if (PlayerController.Instance.IsSwitch) {
				true_shift.x *= -1;
			}
			old_true_shift_x = true_shift.x = Mathf.Lerp(old_true_shift_x, true_shift.x, 0.02f);
			tra.position = tra.TransformPoint(true_shift);

			// if too low, high up a bit
			pos = tra.position;
			pos.y = Mathf.Max(tra.position.y, PlayerController.Instance.boardController.boardTransform.position.y + 0.2f);

			old_pos = actualCam.position = Vector3.Lerp(old_pos, pos, follow_react);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, follow_react_rot);
		}

		private void pov() {
			mainCamera.nearClipPlane = pov_clip;
			mainCamera.fieldOfView = pov_fov;
			actualCam.position = headIK.head.position;
			actualCam.rotation = headIK.head.rotation;
			actualCam.position = actualCam.TransformPoint(pov_shift);
			old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, pov_react);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, pov_react_rot);
		}

		private void skate_pov() {
			mainCamera.nearClipPlane = skate_clip;
			mainCamera.fieldOfView = skate_fov;
			actualCam.position = PlayerController.Instance.boardController.boardTransform.position;
			actualCam.rotation = PlayerController.Instance.boardController.boardTransform.rotation;
			actualCam.position = actualCam.TransformPoint(skate_shift);
			old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, skate_react);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, skate_react_rot);
		}

		private void normal() {
			mainCamera.nearClipPlane = normal_clip;
			mainCamera.fieldOfView = normal_fov;
			old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, normal_react);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, normal_react_rot);
		}

		private void FixedUpdate() {
			switch (cameraMode) {
				case CameraMode.Normal:
					normal();
					break;
				case CameraMode.Low:
					low();
					break;
				case CameraMode.Follow:
					follow();
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
			if (cameraMode == CameraMode.POV) {
				pov();
			}
			foreach (var mat in head_materials) {
				mat.shader = (cameraMode == CameraMode.POV && hide_head) ? standard_shader : head_shader;
			}
		}

		private void log(string s) {
			Main.log(s);
		}

		internal bool isSwitch() {
			var vel = PlayerController.Instance.skaterController.skaterRigidbody.velocity;
			vel.y = 0;
			if (vel.magnitude < 0.3) {
				if (just_respawned) {
					just_respawned = false;
					last_is_switch = spawn_switch;
					return spawn_switch;
				}
				else {
					return last_is_switch;
				}
			}
			else {
				var p4 = PlayerController.Instance.skaterController.skaterTransform.position - (vel.normalized * 100000);
				if (switch_transform == null) switch_transform = new GameObject().transform;
				switch_transform.position = p4;
				switch_transform.LookAt(PlayerController.Instance.skaterController.skaterTransform.position);

				bool is_switch = Vector3.Angle(switch_transform.forward, PlayerController.Instance.skaterController.skaterTransform.forward) > 90f;
				last_is_switch = is_switch;
				return is_switch;
			}
		}
	}
}