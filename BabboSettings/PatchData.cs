using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabboSettings
{
    public class PatchData
    {
        private static PatchData _Instance;
        public static PatchData Instance {
            get {
                if (_Instance == null) {
                    _Instance = new PatchData();
                }
                return _Instance;
            }
        }

        private Transform switch_transform = new GameObject().transform;
        public bool last_is_switch = false;
        public bool spawn_switch = false;
        public bool just_respawned = true;
        public Vector3 spawn_velocity = Vector3.zero;
        public CameraMode cameraMode = CameraMode.Normal;
        public bool overrideCM = false;
        public float cameraFov;
        public CustomCameraController cameraController;

        public void SetJustRespawned() {
            just_respawned = true;
        }

        public void SetSpawnPos() {
            spawn_switch = isSwitch();
            spawn_velocity = PlayerController.Instance.skaterController.skaterRigidbody.velocity;
        }

        public void RotateSpawnCamera() {
            //Camera.main.transform.rotation = Quaternion.LookRotation(spawn_velocity);
        }

        public bool isSwitch() {
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
                var point_behind = PlayerController.Instance.skaterController.skaterTransform.position - (vel.normalized * 100000);
                if (switch_transform == null) switch_transform = new GameObject().transform;
                switch_transform.position = point_behind;
                switch_transform.LookAt(PlayerController.Instance.skaterController.skaterTransform.position);

                bool is_switch = Vector3.Angle(switch_transform.forward, PlayerController.Instance.skaterController.skaterTransform.forward) > 90f;
                last_is_switch = is_switch;
                return is_switch;
            }
        }

        public void UpdateReplayFov(float fov) {
            cameraFov = fov;
            if (cameraController != null) {
                cameraController.replay_fov = fov;
            }
        }
    }
}
