using ReplayEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BabboSettings
{
    public sealed class CustomCameraController
    {
        #region Singleton
        private static readonly Lazy<CustomCameraController> _Instance = new Lazy<CustomCameraController>(() => new CustomCameraController());

        private CustomCameraController() {
            mainCamera = Camera.main;
        }

        public static CustomCameraController Instance {
            get => _Instance.Value;
        }
        #endregion

        #region Utilities
        public Camera mainCamera { get; private set; }
        private Vector3 old_pos = new Vector3();
        private Quaternion old_rot = new Quaternion();
        private Transform tra = new GameObject().transform;
        private HeadIK headIK = UnityEngine.Object.FindObjectOfType<HeadIK>();
        private Transform actualCam = PlayerController.Instance.cameraController._actualCam;
        private Transform switch_transform = new GameObject().transform;
        private bool last_is_switch = false;
        public bool spawn_switch = false;
        public bool just_respawned = true;
        private List<Material> head_materials = new List<Material>();
        private Shader standard_shader;
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
        public float normal_fov = 60;
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
            mainCamera.fieldOfView = 65;
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
            mainCamera.fieldOfView = follow_fov;
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
            mainCamera.fieldOfView = pov_fov;
            actualCam.position = headIK.head.position;
            actualCam.rotation = headIK.head.rotation * Quaternion.Euler(pov_rot_shift);
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

            Transform skater_transform = PlayerController.Instance.skaterController.transform;
            List<GameObject> toHide = new List<GameObject>();
            List<string> toHide_names = new List<string>(new string[]{
                "Cory_fixed_Karam:cory_001:body_geo", "Cory_fixed_Karam:cory_001:eyes_geo",
                "Cory_fixed_Karam:cory_001:lacrima_geo", "Cory_fixed_Karam:cory_001:lashes_geo",
                "Cory_fixed_Karam:cory_001:tear_geo", "Cory_fixed_Karam:cory_001:teethDn_geo",
                "Cory_fixed_Karam:cory_001:teethUp_geo", "Cory_fixed_Karam:cory_001:hat_geo"
            });
            for (int i = 0; i < skater_transform.childCount; i++) {
                var child = skater_transform.GetChild(i);
                //Logger.Log("Child: " + child.name);
                for (int j = 0; j < child.childCount; j++) {
                    var grandchild = child.GetChild(j);
                    //Logger.Log("Grand: " + grandchild.name);
                    for (int k = 0; k < grandchild.childCount; k++) {
                        var grandgrandchild = grandchild.GetChild(k);
                        //Logger.Log("Grandgrand: " + grandgrandchild.name);
                        if (toHide_names.Contains(grandgrandchild.name)) {
                            toHide.Add(grandgrandchild.gameObject);
                        }
                    }
                }
            }

            List<string> mat_names = new List<string>(new string[] {
                "UniqueShaders_head_mat (Instance)", "GenericShaders_eyeInner_mat (Instance)",
                "GenericShaders_eyeOuter_mat (Instance)", "GenericShaders_teeth_mat (Instance)",
                "UniqueShaders_hat_mat (Instance)", "GenericShaders_lacrima_mat (Instance)",
                "GenericShaders_lashes_mat (Instance)", "GenericShaders_tear_mat (Instance)"
            });
            standard_shader = Shader.Find("Standard");
            head_shader = Shader.Find("shaderStandard");
            head_materials = new List<Material>();
            foreach (var obj in toHide) {
                var materials = obj.GetComponent<SkinnedMeshRenderer>().materials;
                for (int k = 0; k < materials.Length; k++) {
                    var mat = materials[k];
                    //Logger.Log("Obj: " + obj.name + ", mat: " + mat.name);
                    if (mat_names.Contains(mat.name)) {
                        head_materials.Add(mat);
                    }
                }
            }

            mainCamera = Camera.main;
        }

        public void FixedUpdate() {
            if (ReplayEditorController.Instance == null || !ReplayEditorController.Instance.isActiveAndEnabled) {
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

        public void LateUpdate() {
            if (cameraMode == CameraMode.POV) {
                pov();
            }
            foreach (var mat in head_materials) {
                mat.shader = (cameraMode == CameraMode.POV && hide_head) ? standard_shader : head_shader;
            }
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