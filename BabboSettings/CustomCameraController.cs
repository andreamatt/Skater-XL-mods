using System;
using System.Collections.Generic;
using UnityEngine;

namespace BabboSettings {
    internal class CustomCameraController : MonoBehaviour {
        internal CameraMode cameraMode = CameraMode.Normal;

        internal Vector3 follow_shift = new Vector3();
        internal Vector3 pov_shift = new Vector3();
        internal Vector3 low_shift = new Vector3(0.6f, -0.2f, -0.9f);
        internal Vector3 skate_shift = new Vector3();
        internal Vector3 old_pos = new Vector3();
        internal Quaternion old_rot = new Quaternion();
        internal float old_true_shift_x = 0;
        internal float normal_fov = 60;
        internal float low_fov = 60;
        internal float follow_fov = 60;
        internal float pov_fov = 60;
        internal float skate_fov = 60;
        internal float pov_smooth = 0.10f;
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

        private void follow(float fov, Vector3 shift) {
            try {
                Camera.main.fieldOfView = fov;
                var pos = PlayerController.Instance.skaterController.skaterTransform.position;
                pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
                if (tra == null) {
                    if (Main.settings.DEBUG) log("Null transform in follow");
                    tra = new GameObject().transform;
                }
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
            catch (Exception e) {
                log("Failed follow, ex: " + e);
            }
        }

        private void pov() {
            Camera.main.fieldOfView = pov_fov;
            actualCam.position = headIK.head.position;
            actualCam.rotation = headIK.head.rotation;
            actualCam.position = actualCam.TransformPoint(pov_shift);
            old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, 1);
            old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, 1 - pov_smooth);
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
            if (cameraMode == CameraMode.POV) {
                pov();
            }
            foreach (var mat in head_materials) {
                mat.shader = (cameraMode == CameraMode.POV) ? standard_shader : head_shader;
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