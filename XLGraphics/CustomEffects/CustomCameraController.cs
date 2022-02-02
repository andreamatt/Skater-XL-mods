using Cinemachine;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.EffectHandlers.CameraEffects;
using XLGraphics.Presets;

namespace XLGraphics.CustomEffects
{
	public class CustomCameraController : MonoBehaviour
	{
		public static CustomCameraController Instance { get; private set; }

		public void Awake() {
			if (Instance != null) {
				throw new Exception("Instance not null on Awake");
			}
			Instance = this;
		}

		public void Start() {
			old_pos = new Vector3();
			old_rot = new Quaternion();
			tra = new GameObject("Transform for customcameracontroller").transform;
			headIK = FindObjectOfType<HeadIK>();
			actualCam = PlayerController.Instance.cameraController._actualCam;
			cameraControllerTraverse = Traverse.Create(PlayerController.Instance.cameraController).Field<bool>("_right");
			follow_auto_side_right = SettingsManager.Instance.SelectedStance == SkaterXL.Core.Stance.Regular;
		}

		private CinemachineVirtualCamera _mainCamera;
		public CinemachineVirtualCamera mainCamera {
			get
            {
                if (this._mainCamera == null) {
                    this._mainCamera = PlayerController.Instance.cameraController._actualCam.GetComponent<CinemachineVirtualCamera>();
                }
                return this._mainCamera;
			}
		}

		private CinemachineVirtualCamera _replayCamera;
		public CinemachineVirtualCamera replayCamera {
			get {
				if (_replayCamera == null) {
					_replayCamera = ReplayEditor.ReplayEditorController.Instance.cameraController.VirtualCamera;
				}
				return _replayCamera;
			}
		}

		private bool last_is_replay = false;
		private Vector3 old_pos { get; set; }
		private Quaternion old_rot { get; set; }
		private Transform tra { get; set; }
		private HeadIK headIK { get; set; }
		private Transform actualCam { get; set; }
		private Traverse<bool> cameraControllerTraverse { get; set; }
		private bool follow_auto_side_right;

		public Transform replay_skater;
		public Transform replay_skateboard;

		public XLCameraMode cameraMode = XLCameraMode.Normal;
		public Vector3 follow_shift = new Vector3();
		public Vector3 pov_shift = new Vector3();
		private Vector3 low_shift = new Vector3(0.6f, -0.2f, -0.9f);
		public Vector3 skate_shift = new Vector3();
		public float old_true_shift_x = 0;
		public float replay_fov = 60;
		public float normal_fov = 60;
		public bool override_fov = false;
		public float override_fov_value = 90f;
		public float normal_react = 0.90f;
		public float normal_react_rot = 0.90f;
		public float normal_clip = 0.01f;
		public float follow_fov = 60;
		public float follow_react = 0.70f;
		public float follow_react_rot = 0.70f;
		public float follow_clip = 0.01f;
		public bool follow_auto_switch = false;
		public float pov_fov = 60;
		public float pov_react = 1;
		public float pov_react_rot = 0.07f;
		public float pov_clip = 0.01f;
		public Vector3 pov_rot_shift = new Vector3(-100, 13, 73);
		public bool hide_head = true;
		public float skate_fov = 60;
		public float skate_react = 0.90f;
		public float skate_react_rot = 0.90f;
		public float skate_clip = 0.01f;

		private void follow() {
			mainCamera.m_Lens.NearClipPlane = follow_clip;
			mainCamera.m_Lens.FieldOfView = override_fov ? override_fov_value : follow_fov;
			var pos = PlayerController.Instance.skaterController.skaterTransform.position;
			pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
			if (tra == null) {
				Main.Logger.Log("Null transform in follow");
				tra = new GameObject().transform;
			}
			tra.position = pos;
			tra.rotation = actualCam.rotation;
			follow_shift.x = Math.Abs(follow_shift.x);
			Vector3 true_shift = follow_shift;

			if (follow_auto_switch) {
				if (PlayerController.Instance.IsSwitch) {
					if (PlayerController.Instance.inputController.player.GetAxis("DPadX") < 0f) {
						follow_auto_side_right = true;
					}
					else if (PlayerController.Instance.inputController.player.GetAxis("DPadX") > 0f) {
						follow_auto_side_right = false;
					}
					// change side if in switch
					true_shift.x *= -1;
				}
				else {
					if (PlayerController.Instance.inputController.player.GetAxis("DPadX") < 0f) {
						follow_auto_side_right = false;
					}
					else if (PlayerController.Instance.inputController.player.GetAxis("DPadX") > 0f) {
						follow_auto_side_right = true;
					}
				}
				// change side based on default side
				if (!follow_auto_side_right) {
					true_shift.x *= -1;
				}
			}
			else {
				if (!cameraControllerTraverse.Value) {
					true_shift.x *= -1;
				}
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
			mainCamera.m_Lens.NearClipPlane = pov_clip;
			mainCamera.m_Lens.FieldOfView = override_fov ? override_fov_value : pov_fov;
			actualCam.position = headIK.head.position;
			actualCam.rotation = headIK.head.rotation * Quaternion.Euler(pov_rot_shift);
			actualCam.position = actualCam.TransformPoint(pov_shift);
			old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, pov_react);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, pov_react_rot);
		}

		private void skate_pov() {
			mainCamera.m_Lens.NearClipPlane = skate_clip;
			mainCamera.m_Lens.FieldOfView = override_fov ? override_fov_value : skate_fov;
			actualCam.position = PlayerController.Instance.boardController.boardTransform.position;
			actualCam.rotation = PlayerController.Instance.boardController.boardTransform.rotation;
			actualCam.position = actualCam.TransformPoint(skate_shift);
			old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, skate_react);
			old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, skate_react_rot);
		}

		private void normal() {
			mainCamera.m_Lens.NearClipPlane = normal_clip;
			mainCamera.m_Lens.FieldOfView = override_fov ? override_fov_value : normal_fov;

			if (actualCam != null) {
				old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, normal_react);
				old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, normal_react_rot);
			}
		}

		public void FixedUpdate() {
			if (XLGraphics.Instance.currentGameStateName == "PlayState") {
				if (last_is_replay == true) {
					// Save the new replay_fov, since it can be modified without opening the mod
					// The only other way of saving is when the mod is open
					Main.Save();
				}
				last_is_replay = false;

				switch (cameraMode) {
					case XLCameraMode.Follow:
						follow();
						break;
					case XLCameraMode.Normal:
						normal();
						break;
					case XLCameraMode.POV:
						//pov(); doesn't work on fixedUpdate, probably because of animations
						break;
					case XLCameraMode.Skate:
						skate_pov();
						break;
				}
			}
			else if (XLGraphics.Instance.currentGameStateName == "ReplayState") {
				// Normal camera values
				replayCamera.m_Lens.NearClipPlane = 0.01f;
				replayCamera.m_Lens.FieldOfView = override_fov ? override_fov_value : replay_fov;
				last_is_replay = true;
			}
		}

		public void LateUpdate() {
			var currentStateName = XLGraphics.Instance.currentGameStateName;
			if (currentStateName == "PlayState") {
				if (cameraMode == XLCameraMode.POV) {
					pov();
				}

				UpdateOverrideFov();
			}
			else if (currentStateName == "ReplayState") {
				UpdateOverrideFov();
			}
		}

		private void UpdateOverrideFov() {
			var presets = PresetManager.Instance.presets;
			var presetsWithActiveFov = presets.Where(p => p.enabled && p.fovOverrideData.active).ToList();
			if (presetsWithActiveFov.Count == 0) {
				override_fov = false;
			}
			else {
				override_fov = true;
				override_fov_value = presetsWithActiveFov.First().fovOverrideData.fov;
			}
		}
	}
}
