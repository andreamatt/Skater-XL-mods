using ReplayEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BabboSettings
{
    public class CustomCameraController : Module
    {
        public override void Start() {
            mainCamera = Camera.main;
        }

        #region Utilities
        public Camera mainCamera { get; private set; }
        private bool last_is_replay = false;
        private Vector3 old_pos = new Vector3();
        private Quaternion old_rot = new Quaternion();
        private Transform tra = new GameObject().transform;
        private HeadIK headIK = UnityEngine.Object.FindObjectOfType<HeadIK>();
        private Transform actualCam = PlayerController.Instance.cameraController._actualCam;
        private List<Material> head_materials = new List<Material>();
        private Shader hiding_shader;
        private Shader head_shader;

        public Transform replay_skater;
        public Transform replay_skateboard;
        #endregion

        #region Parameters
        public CameraMode cameraMode = CameraMode.Normal;
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
        #endregion

        #region Camera modes

        private void low() {
            mainCamera.nearClipPlane = 0.01f;
            mainCamera.fieldOfView = override_fov ? override_fov_value : 65;
            var pos = PlayerController.Instance.skaterController.skaterTransform.position;
            pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
            if (tra == null) {
                Logger.Debug("Null transform in low");
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
            mainCamera.fieldOfView = override_fov ? override_fov_value : follow_fov;
            var pos = PlayerController.Instance.skaterController.skaterTransform.position;
            pos.y = (pos.y + PlayerController.Instance.boardController.boardTransform.position.y) / 2;
            if (tra == null) {
                Logger.Debug("Null transform in follow");
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
            mainCamera.fieldOfView = override_fov ? override_fov_value : pov_fov;
            actualCam.position = headIK.head.position;
            actualCam.rotation = headIK.head.rotation * Quaternion.Euler(pov_rot_shift);
            actualCam.position = actualCam.TransformPoint(pov_shift);
            old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, pov_react);
            old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, pov_react_rot);
        }

        private void skate_pov() {
            mainCamera.nearClipPlane = skate_clip;
            mainCamera.fieldOfView = override_fov ? override_fov_value : skate_fov;
            actualCam.position = PlayerController.Instance.boardController.boardTransform.position;
            actualCam.rotation = PlayerController.Instance.boardController.boardTransform.rotation;
            actualCam.position = actualCam.TransformPoint(skate_shift);
            old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, skate_react);
            old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, skate_react_rot);
        }

        private void normal() {
            mainCamera.nearClipPlane = normal_clip;
            mainCamera.fieldOfView = override_fov ? override_fov_value : normal_fov;
            old_pos = actualCam.position = Vector3.Lerp(old_pos, actualCam.position, normal_react);
            old_rot = actualCam.rotation = Quaternion.Lerp(old_rot, actualCam.rotation, normal_react_rot);
        }

        #endregion

        public void GetReplaySkater() {
            var playback_root = GameObject.Find("Playback Skater Root");
            if (playback_root != null) {
                for (int i = 0; i < playback_root.transform.childCount; i++) {
                    Logger.Debug($"Playback_root child: {playback_root.transform.GetChild(i).name}");
                    var child = playback_root.transform.GetChild(i);
                    if (child.name == "NewSkater") replay_skater = child;
                    else if (child.name == "Skateboard") replay_skateboard = child;
                }
            }
            else {
                Logger.Debug("Playback_root not found");
            }
        }

        public void GetHeadMaterials() {

            head_materials = new List<Material>();

            var body = GameObject.Find("Body");
            if (body != null) {
                var renderer = body.GetComponent<Renderer>();
                if (renderer != null) {
                    foreach (var mat in renderer.materials) {
                        if (mat.name == "Head_mat (Instance)") {
                            Logger.Log("Head mat found");
                            head_materials.Add(mat);
                            head_shader = mat.shader;
                        }
                    }
                }
            }

            var eye_transforms = new List<string>() { "Hairs", "Body", "Body_armsAndHead", "Eyes_and_eyelashes", "eye", "eye_wetness", "eyelashes" };
            foreach (var eye_t in eye_transforms) {
                var t = GameObject.Find(eye_t);
                if (t != null) {
                    Logger.Log(t + " found");
                    var renderer = t.gameObject.GetComponent<Renderer>();
                    if (renderer != null) {
                        Logger.Log("renderer found");
                        foreach (var mat in renderer.materials) {
                            Logger.Log("Mat: " + mat.name);
                        }
                    }
                }
            }

            //LogGGC();

            mainCamera = Camera.main;
        }

        private void LogGGC() {
            Transform skater_transform = PlayerController.Instance.skaterController.transform;
            for (int i = 0; i < skater_transform.childCount; i++) {
                var child = skater_transform.GetChild(i);
                for (int j = 0; j < child.childCount; j++) {
                    var grandchild = child.GetChild(j);
                    for (int k = 0; k < grandchild.childCount; k++) {
                        var grandgrandchild = grandchild.GetChild(k);
                        Logger.Log("Grandgrand: " + grandgrandchild.name);
                        for (int a = 0; a < grandgrandchild.childCount; a++) {
                            Logger.Log("GGGC: " + grandgrandchild.GetChild(a).name);
                        }
                    }
                }
            }
        }

        public override void FixedUpdate() {
            if (BabboSettings.Instance.IsReplayActive()) {
                // Normal camera values
                mainCamera.nearClipPlane = 0.01f;
                mainCamera.fieldOfView = override_fov ? override_fov_value : replay_fov;
                last_is_replay = true;
            }
            else {
                if (last_is_replay == true) {
                    // Save the new replay_fov, since it can be modified without opening the mod
                    // The only other way of saving is when the mod is open
                    Main.Save();
                }
                last_is_replay = false;
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
        }

        public override void LateUpdate() {
            if (!BabboSettings.Instance.IsReplayActive()) {
                if (cameraMode == CameraMode.POV) {
                    pov();
                }
                foreach (var mat in head_materials) {
                    //mat.shader = (cameraMode == CameraMode.POV && hide_head) ? hiding_shader : head_shader;
                    //Logger.Log("Hiding " + mat.name);
                    mat.color = Color.clear;
                }
            }
        }
    }
}