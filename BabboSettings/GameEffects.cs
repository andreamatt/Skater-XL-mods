using GameManagement;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
    public class GameEffects : Module
    {
        // Settings stuff
        public PostProcessLayer post_layer;
        public PostProcessVolume post_volume;
        public FastApproximateAntialiasing FXAA;
        public TemporalAntialiasing TAA; // NOT SERIALIZABLE
        public SubpixelMorphologicalAntialiasing SMAA;

        public AmbientOcclusion AO;
        public AutoExposure EXPO;
        public Bloom BLOOM;
        public ChromaticAberration CA;
        public ColorGrading COLOR; // NOT SERIALIZABLE
        public DepthOfField DOF;
        public FocusMode focus_mode = FocusMode.Custom;
        public Grain GRAIN;
        public LensDistortion LENS;
        public MotionBlur BLUR;
        public ScreenSpaceReflections REFL;
        public Vignette VIGN;

        private void getEffects() {
            post_layer = Camera.main.GetComponent<PostProcessLayer>();
            if (post_layer == null) {
                Logger.Debug("Null post layer");
            }
            if (post_layer.enabled == false) {
                post_layer.enabled = true;
                Logger.Debug("post_layer was disabled");
            }
            post_volume = FindObjectOfType<PostProcessVolume>();
            if (post_volume != null) {
                string not_found = "";
                if ((AO = post_volume.profile.GetSetting<AmbientOcclusion>()) == null) {
                    not_found += "ao,";
                    AO = post_volume.profile.AddSettings<AmbientOcclusion>();
                    AO.enabled.Override(false);
                }
                if ((EXPO = post_volume.profile.GetSetting<AutoExposure>()) == null) {
                    not_found += "expo,";
                    EXPO = post_volume.profile.AddSettings<AutoExposure>();
                    EXPO.enabled.Override(false);
                }
                if ((BLOOM = post_volume.profile.GetSetting<Bloom>()) == null) {
                    not_found += "bloom,";
                    BLOOM = post_volume.profile.AddSettings<Bloom>();
                    BLOOM.enabled.Override(false);
                }
                if ((CA = post_volume.profile.GetSetting<ChromaticAberration>()) == null) {
                    not_found += "ca,";
                    CA = post_volume.profile.AddSettings<ChromaticAberration>();
                    CA.enabled.Override(false);
                }
                if ((COLOR = post_volume.profile.GetSetting<ColorGrading>()) == null) {
                    not_found += "color,";
                    COLOR = post_volume.profile.AddSettings<ColorGrading>();
                    COLOR.enabled.Override(false);
                }
                if ((DOF = post_volume.profile.GetSetting<DepthOfField>()) == null) {
                    not_found += "dof,";
                    DOF = post_volume.profile.AddSettings<DepthOfField>();
                    DOF.enabled.Override(false);
                }
                if ((GRAIN = post_volume.profile.GetSetting<Grain>()) == null) {
                    not_found += "grain,";
                    GRAIN = post_volume.profile.AddSettings<Grain>();
                    GRAIN.enabled.Override(false);
                }
                if ((BLUR = post_volume.profile.GetSetting<MotionBlur>()) == null) {
                    not_found += "blur,";
                    BLUR = post_volume.profile.AddSettings<MotionBlur>();
                    BLUR.enabled.Override(false);
                }
                if ((LENS = post_volume.profile.GetSetting<LensDistortion>()) == null) {
                    not_found += "lens,";
                    LENS = post_volume.profile.AddSettings<LensDistortion>();
                    LENS.enabled.Override(false);
                }
                if ((REFL = post_volume.profile.GetSetting<ScreenSpaceReflections>()) == null) {
                    not_found += "refl,";
                    REFL = post_volume.profile.AddSettings<ScreenSpaceReflections>();
                    REFL.enabled.Override(false);
                }
                if ((VIGN = post_volume.profile.GetSetting<Vignette>()) == null) {
                    not_found += "vign,";
                    VIGN = post_volume.profile.AddSettings<Vignette>();
                    VIGN.enabled.Override(false);
                }
                if (not_found.Length > 0) {
                    Logger.Debug("Not found: " + not_found);
                }
            }
            else {
                Logger.Debug("Post_volume is null in getEffects");
            }

            FXAA = post_layer.fastApproximateAntialiasing;
            TAA = post_layer.temporalAntialiasing;
            SMAA = post_layer.subpixelMorphologicalAntialiasing;

            mapPreset.GetMapEffects();

            presetsManager.ApplySettings();
            presetsManager.ApplyPresets();

            // After applying, can now save
            Main.canSave = true;

            cameraController.GetHeadMaterials();
            //DayNightController.Instance.GetLights();

            Logger.Debug("Done getEffects");
        }

        public void checkAndGetEffects() {
            if (post_volume == null) {
                Logger.Debug("Post volume is null (probably map changed)");
                post_volume = UnityEngine.Object.FindObjectOfType<PostProcessVolume>();
                if (post_volume == null) {
                    Logger.Debug("Post volume not found => creating");
                    GameObject post_vol_go = new GameObject();
                    post_vol_go.layer = 8;
                    post_volume = post_vol_go.AddComponent<PostProcessVolume>();
                    post_volume.profile = new PostProcessProfile();
                    post_volume.isGlobal = true;
                    Logger.Debug("Now a & e:" + post_volume.isActiveAndEnabled);
                    Logger.Debug("Has profile: " + post_volume.HasInstantiatedProfile());
                }
                getEffects();
            }
        }

        public void reset<T>(ref T effect) where T : PostProcessEffectSettings {
            bool enabled = effect.enabled.value;
            var newEffect = ScriptableObject.CreateInstance(effect.GetType()) as T;
            newEffect.enabled.Override(enabled);
            effect = newEffect;
        }

        public override void LateUpdate() {
            Vector3 player_pos;
            Vector3 skate_pos;

            if (BabboSettings.IsReplayActive()) {
                if (cameraController.replay_skater == null) {
                    cameraController.GetReplaySkater();
                }
                player_pos = cameraController.replay_skater.position;
                skate_pos = cameraController.replay_skateboard.position;
            }
            else {
                player_pos = PlayerController.Instance.skaterController.skaterTransform.position;
                skate_pos = PlayerController.Instance.boardController.boardTransform.position;
            }
            if (focus_mode == FocusMode.Player) {
                DOF.focusDistance.Override(Vector3.Distance(player_pos, cameraController.mainCamera.transform.position));
            }
            else if (focus_mode == FocusMode.Skate) {
                DOF.focusDistance.Override(Vector3.Distance(skate_pos, cameraController.mainCamera.transform.position));
            }
        }
    }
}
