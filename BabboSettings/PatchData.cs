using Harmony12;
using ReplayEditor;
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
        public CameraMode cameraMode {
            get {
                if (cameraController == null) return CameraMode.Normal;
                return cameraController.cameraMode;
            }
        }
        private float positionSpeed => Main.settings.positionSpeed;
        private float rotationSpeed => Main.settings.rotationSpeed;
        private float fovSpeed => Main.settings.fovSpeed;
        private float xDownTime = 0f;
        private Traverse addKeyFrameTraverse;
        private Traverse deleteKeyFrameTraverse;

        public CustomCameraController cameraController;
        public Window window;

        public void SetJustRespawned() {
            just_respawned = true;
        }

        public void SetSpawnPos() {
            spawn_switch = isSwitch();
            spawn_velocity = PlayerController.Instance.skaterController.skaterRigidbody.velocity;
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
            if (cameraController != null) {
                cameraController.replay_fov = fov;
                window.sliderTextValues["REPLAY_FOV"] = fov.ToString("0.00");
            }
        }

        public void FreeInput(ReplayCameraController replayCameraController) {
            if (Input.GetMouseButton(2)) {
                InputPosition();
                InputRotation();
            }
            InputFOV();
            InputKeyFrames(replayCameraController);
        }

        private void InputPosition() {
            const float mult = 3f;
            float x = 0;
            if (Input.GetKey(KeyCode.D)) {
                x += positionSpeed * Time.unscaledDeltaTime * mult;
            }
            if (Input.GetKey(KeyCode.A)) {
                x += -1 * positionSpeed * Time.unscaledDeltaTime * mult;  // -1 because it's opposite direction
            }

            float y = 0;
            if (Input.GetKey(KeyCode.E)) {
                y += positionSpeed * Time.unscaledDeltaTime * mult;
            }
            if (Input.GetKey(KeyCode.Q)) {
                y += -1 * positionSpeed * Time.unscaledDeltaTime * mult;  // -1 because it's opposite direction
            }

            float z = 0;
            if (Input.GetKey(KeyCode.W)) {
                z += positionSpeed * Time.unscaledDeltaTime * mult;
            }
            if (Input.GetKey(KeyCode.S)) {
                z += -1 * positionSpeed * Time.unscaledDeltaTime * mult;  // -1 because it's opposite direction
            }

            Vector3 point = new Vector3(x, y, z);
            var cameraTransform = PatchData.Instance.cameraController.mainCamera.transform;
            Vector3 a = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * point;
            cameraTransform.position += a * positionSpeed * Time.unscaledDeltaTime;
        }

        private void InputRotation() {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            var cameraTransform = PatchData.Instance.cameraController.mainCamera.transform;
            cameraTransform.transform.rotation = Quaternion.AngleAxis(y * rotationSpeed * Time.unscaledDeltaTime, Vector3.ProjectOnPlane(-cameraTransform.right, Vector3.up)) * cameraTransform.rotation;
            cameraTransform.transform.rotation = Quaternion.AngleAxis(x * rotationSpeed * Time.unscaledDeltaTime, Vector3.up) * cameraTransform.rotation;
        }

        private void InputFOV() {
            // update the value only if the slider is enabled
            if (cameraController.override_fov == false) {
                float num = -Input.mouseScrollDelta.y;
                if (Mathf.Abs(num) > 0.01) {
                    // limit value between 1 and 179
                    var new_fov = cameraController.replay_fov + num * fovSpeed * Time.unscaledDeltaTime;
                    new_fov = Mathf.Max(1, Mathf.Min(179, new_fov));

                    UpdateReplayFov(new_fov);
                }
            }
        }

        private void InputKeyFrames(ReplayCameraController replayCameraController) {
            // started pressing
            if (Input.GetMouseButtonDown(1)) {
                xDownTime = Time.unscaledTime;
            }

            // still pressed after 1.5 seconds
            if (Input.GetMouseButton(1) && Time.unscaledTime - xDownTime > 1.5f) {
                if (deleteKeyFrameTraverse == null) {
                    deleteKeyFrameTraverse = Traverse.Create(replayCameraController).Method("DeleteKeyFrameAtCurrentPosition", new Type[] { typeof(float) });
                }
                deleteKeyFrameTraverse.GetValue(ReplayEditorController.Instance.PlaybackTime);
                replayCameraController.keyframeUI.UpdateKeyframes(replayCameraController.keyFrames);
            }

            // released in less than 0.5 seconds
            if (Input.GetMouseButtonUp(1) && Time.unscaledTime - xDownTime < 0.5f) {
                if (addKeyFrameTraverse == null) {
                    addKeyFrameTraverse = Traverse.Create(replayCameraController).Method("AddKeyFrame", new Type[] { typeof(float) });
                }
                if (addKeyFrameTraverse.MethodExists()) {
                    Logger.Log("Method found");
                    addKeyFrameTraverse.GetValue(ReplayEditorController.Instance.PlaybackTime);
                    replayCameraController.keyframeUI.UpdateKeyframes(replayCameraController.keyFrames);
                }
                else {
                    Logger.Log("No method found");
                }
            }
        }
    }
}
