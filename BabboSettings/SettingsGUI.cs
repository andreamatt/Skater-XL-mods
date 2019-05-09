using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace BabboSettings {

	internal partial class SettingsGUI : MonoBehaviour {
		// Settings stuff
		private PostProcessLayer post_layer;
		private PostProcessVolume post_volume;
		private CustomCameraController customCameraController = XLShredLib.ModMenu.Instance.gameObject.AddComponent<CustomCameraController>();

		private FastApproximateAntialiasing GAME_FXAA;
		private TemporalAntialiasing GAME_TAA; // NOT SERIALIZABLE
		private SubpixelMorphologicalAntialiasing GAME_SMAA;

		private AmbientOcclusion GAME_AO;
		private AutoExposure GAME_EXPO;
		private Bloom GAME_BLOOM;
		private ChromaticAberration GAME_CA;
		private ColorGrading GAME_COLOR; // NOT SERIALIZABLE
		private DepthOfField GAME_DOF;
		private FocusMode focus_mode = FocusMode.Custom;
		private Grain GAME_GRAIN;
		private LensDistortion GAME_LENS;
		private MotionBlur GAME_BLUR;
		private ScreenSpaceReflections GAME_REFL;
		private Vignette GAME_VIGN;

		internal bool map_preset_selected = false;

		private void Update() {
			bool keyUp = Input.GetKeyUp(KeyCode.Backspace);
			if (keyUp) {
				if (showUI == false) {
					Open();
				}
				else {
					Close();
				}
			}
		}

		private void LateUpdate() {
			if (focus_mode == FocusMode.Player) {
				Vector3 player_pos = PlayerController.Instance.skaterController.skaterTransform.position;
				GAME_DOF.focusDistance.Override(Vector3.Distance(player_pos, customCameraController.mainCamera.transform.position));
			}
			else if (focus_mode == FocusMode.Skate) {
				Vector3 skate_pos = PlayerController.Instance.boardController.boardTransform.position;
				GAME_DOF.focusDistance.Override(Vector3.Distance(skate_pos, customCameraController.mainCamera.transform.position));
			}
		}

		private T reset<T>() where T : PostProcessEffectSettings {
			bool enabled = post_volume.profile.GetSetting<T>().enabled.value;
			post_volume.profile.RemoveSettings<T>();
			T eff = post_volume.profile.AddSettings<T>();
			eff.enabled.Override(enabled);
			return eff;
		}

		private void getSettings() {
			post_layer = Camera.main.GetComponent<PostProcessLayer>();
			if (post_layer == null) {
				if (Main.settings.DEBUG) log("Null post layer");
			}
			if (post_layer.enabled == false) {
				post_layer.enabled = true;
				if (Main.settings.DEBUG) log("post_layer was disabled");
			}
			post_volume = FindObjectOfType<PostProcessVolume>();
			if (post_volume != null) {
				string not_found = "";
				if ((GAME_AO = post_volume.profile.GetSetting<AmbientOcclusion>()) == null) {
					not_found += "ao,";
					GAME_AO = post_volume.profile.AddSettings<AmbientOcclusion>();
					GAME_AO.enabled.Override(false);
				}
				if ((GAME_EXPO = post_volume.profile.GetSetting<AutoExposure>()) == null) {
					not_found += "expo,";
					GAME_EXPO = post_volume.profile.AddSettings<AutoExposure>();
					GAME_EXPO.enabled.Override(false);
				}
				if ((GAME_BLOOM = post_volume.profile.GetSetting<Bloom>()) == null) {
					not_found += "bloom,";
					GAME_BLOOM = post_volume.profile.AddSettings<Bloom>();
					GAME_BLOOM.enabled.Override(false);
				}
				if ((GAME_CA = post_volume.profile.GetSetting<ChromaticAberration>()) == null) {
					not_found += "ca,";
					GAME_CA = post_volume.profile.AddSettings<ChromaticAberration>();
					GAME_CA.enabled.Override(false);
				}
				if ((GAME_COLOR = post_volume.profile.GetSetting<ColorGrading>()) == null) {
					not_found += "color,";
					GAME_COLOR = post_volume.profile.AddSettings<ColorGrading>();
					GAME_COLOR.enabled.Override(false);
				}
				if ((GAME_DOF = post_volume.profile.GetSetting<DepthOfField>()) == null) {
					not_found += "dof,";
					GAME_DOF = post_volume.profile.AddSettings<DepthOfField>();
					GAME_DOF.enabled.Override(false);
				}
				if ((GAME_GRAIN = post_volume.profile.GetSetting<Grain>()) == null) {
					not_found += "grain,";
					GAME_GRAIN = post_volume.profile.AddSettings<Grain>();
					GAME_GRAIN.enabled.Override(false);
				}
				if ((GAME_BLUR = post_volume.profile.GetSetting<MotionBlur>()) == null) {
					not_found += "blur,";
					GAME_BLUR = post_volume.profile.AddSettings<MotionBlur>();
					GAME_BLUR.enabled.Override(false);
				}
				if ((GAME_LENS = post_volume.profile.GetSetting<LensDistortion>()) == null) {
					not_found += "lens,";
					GAME_LENS = post_volume.profile.AddSettings<LensDistortion>();
					GAME_LENS.enabled.Override(false);
				}
				if ((GAME_REFL = post_volume.profile.GetSetting<ScreenSpaceReflections>()) == null) {
					not_found += "refl,";
					GAME_REFL = post_volume.profile.AddSettings<ScreenSpaceReflections>();
					GAME_REFL.enabled.Override(false);
				}
				if ((GAME_VIGN = post_volume.profile.GetSetting<Vignette>()) == null) {
					not_found += "vign,";
					GAME_VIGN = post_volume.profile.AddSettings<Vignette>();
					GAME_VIGN.enabled.Override(false);
				}
				if (not_found.Length > 0) {
					log("Not found: " + not_found);
				}
			}
			else {
				if (Main.settings.DEBUG) log("Post_volume is null in getSettings");
			}

			GAME_FXAA = post_layer.fastApproximateAntialiasing;
			GAME_TAA = post_layer.temporalAntialiasing;
			GAME_SMAA = post_layer.subpixelMorphologicalAntialiasing;

			Preset map_preset = new Preset(SceneManager.GetActiveScene().name + " (Original)");
			SaveToPreset(map_preset);
			Main.presets[map_preset.name] = map_preset;
			if (map_preset_selected) {
				Main.select(map_preset.name);
			}
			else {
				Main.select(Main.settings.presetName);
				map_preset_selected = isMapPresetSelected();
			}
			if (Main.settings.DEBUG) log("Before applying");
			ApplySettings();
			ApplyPreset(Main.selectedPreset);
			if (Main.settings.DEBUG) log("Before after");

			// After applying, can now save
			Main.canSave = true;

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
				//log("Child: " + child.name);
				for (int j = 0; j < child.childCount; j++) {
					var grandchild = child.GetChild(j);
					//log("Grand: " + grandchild.name);
					for (int k = 0; k < grandchild.childCount; k++) {
						var grandgrandchild = grandchild.GetChild(k);
						//log("Grandgrand: " + grandgrandchild.name);
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
			customCameraController.standard_shader = Shader.Find("Standard");
			customCameraController.head_shader = Shader.Find("shaderStandard");
			customCameraController.head_materials = new List<Material>();
			foreach (var obj in toHide) {
				var materials = obj.GetComponent<SkinnedMeshRenderer>().materials;
				for (int k = 0; k < materials.Length; k++) {
					var mat = materials[k];
					//log("Obj: " + obj.name + ", mat: " + mat.name);
					if (mat_names.Contains(mat.name)) {
						customCameraController.head_materials.Add(mat);
					}
				}
			}

			customCameraController.mainCamera = Camera.main;

			if (Main.settings.DEBUG) log("Done getSettings");
		}
	}
}
