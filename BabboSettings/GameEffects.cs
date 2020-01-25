using GameManagement;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
    public class GameEffects : Module
    {
        // Settings stuff
        public PostProcessLayer post_layer;
        public PostProcessVolume post_volume;
        public PostProcessVolume map_post_volume;
        public FastApproximateAntialiasing FXAA;
        public TemporalAntialiasing TAA; // NOT SERIALIZABLE
        public SubpixelMorphologicalAntialiasing SMAA;

        public FocusMode focus_mode = FocusMode.Custom;

        public EffectSuite effectSuite;
        public EffectSuite map_effectSuite;

        private void getEffects() {
            post_layer = Camera.main.GetComponent<PostProcessLayer>();
            if (post_layer == null) {
                Logger.Debug("Null post layer");
            }
            if (post_layer.enabled == false) {
                post_layer.enabled = true;
                Logger.Debug("post_layer was disabled");
            }

            // get global volumes, order by decreasing priority
            var allVolumes = FindObjectsOfType<PostProcessVolume>().Where(v => v.isGlobal).OrderBy(v => -v.priority).ToList();
            Logger.Log($"Found {allVolumes.Count} volumes");
            if (allVolumes.Count == 0) {
                Logger.Log("No global volumes");
            }
            else if (allVolumes.Count == 1) {
                Logger.Log("Only 1 global volume");
                map_post_volume = post_volume = allVolumes.First();
            }
            else if (allVolumes.Count == 2) {   // expected behaviour
                post_volume = allVolumes[0];
                map_post_volume = allVolumes[1];
            }
            else {
                Logger.Log("More than 2 global volumes");
                post_volume = allVolumes[0];
                map_post_volume = allVolumes[1];
            }

            // get effects for the mod
            effectSuite = EffectSuite.FromVolume(post_volume);

            // get global map volume effects
            map_effectSuite = EffectSuite.FromVolume(map_post_volume);


            FXAA = post_layer.fastApproximateAntialiasing;
            TAA = post_layer.temporalAntialiasing;
            SMAA = post_layer.subpixelMorphologicalAntialiasing;

            mapPreset.GetMapEffects();

            // enable volumetric lighting/fog/...
            var pipelineAsset = GraphicsSettings.renderPipelineAsset as HDRenderPipelineAsset;
            pipelineAsset.renderPipelineSettings.supportVolumetrics = true;

            presetsManager.ApplySettings();
            presetsManager.ApplyPresets();

            // After applying, can now save
            Main.canSave = true;

            //cameraController.GetHeadMaterials();
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

            if (BabboSettings.Instance.IsReplayActive()) {
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
                effectSuite.DOF.focusDistance.Override(Vector3.Distance(player_pos, cameraController.mainCamera.transform.position));
            }
            else if (focus_mode == FocusMode.Skate) {
                effectSuite.DOF.focusDistance.Override(Vector3.Distance(skate_pos, cameraController.mainCamera.transform.position));
            }
        }
    }
}
