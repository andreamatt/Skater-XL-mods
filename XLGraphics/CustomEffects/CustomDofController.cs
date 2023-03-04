using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.EffectHandlers.CameraEffects;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.Presets;

namespace XLGraphics.CustomEffects
{
	public class CustomDofController : MonoBehaviour
	{
		public static CustomDofController Instance { get; private set; }
		private FocusMode? customFocusMode = null;
		private bool useCustomFocus => customFocusMode == FocusMode.Skate || customFocusMode == FocusMode.Player;
		private Transform replay_skater;
		private Transform replay_skateboard;
		private DepthOfField dof;

		public void Awake() {
			if (Instance != null) {
				throw new Exception("Instance not null on Awake");
			}
			Instance = this;
		}

		private void UpdateDofMode() {
			var presets = PresetManager.Instance.presets;
			var presetsWithActiveDof = presets.Where(p => p.enabled && p.depthOfField.active).ToList();
			if (presetsWithActiveDof.Count == 0) {
				customFocusMode = null;
			}
			else {
				customFocusMode = presetsWithActiveDof.First().focusMode;
			}
		}

		public void LateUpdate() {
			UpdateDofMode();

			Vector3 player_pos;
			Vector3 skate_pos;

			if (XLGraphics.Instance.IsReplayActive()) {
				if (replay_skater == null) {
					GetReplaySkaterAndBoard();
				}
				player_pos = replay_skater.position;
				skate_pos = replay_skateboard.position;
			}
			else {
				player_pos = PlayerController.Instances[PlayerController.Instances.Count - 1].gameplay.transformReference.skaterRoot.position;
				skate_pos = PlayerController.Instances[PlayerController.Instances.Count - 1].gameplay.transformReference.boardTransform.position;
			}

			if (dof == null) {
				dof = PresetManager.Instance.overriderVolume.profile.Add<DepthOfField>();
				dof.SetAllOverridesTo(true);
				dof.focusMode.value = DepthOfFieldMode.UsePhysicalCamera;
			}

			// activate DoF override only when using custom focus
			dof.active = useCustomFocus;
			if (useCustomFocus) {
				var camera = CustomCameraController.Instance.mainCamera;
				if (customFocusMode == FocusMode.Player) {
					dof.focusDistance.value = Vector3.Distance(player_pos, camera.transform.position);
				}
				else if (customFocusMode == FocusMode.Skate) {
					dof.focusDistance.value = Vector3.Distance(skate_pos, camera.transform.position);
				}
			}
		}

		public void GetReplaySkaterAndBoard() {
			var playback_root = GameObject.Find("Playback Skater Root");
			if (playback_root != null) {
				for (int i = 0; i < playback_root.transform.childCount; i++) {
					Main.Logger.Log($"Playback_root child: {playback_root.transform.GetChild(i).name}");
					var child = playback_root.transform.GetChild(i);
					if (child.name == "NewSkater") replay_skater = child;
					else if (child.name == "Skateboard") replay_skateboard = child;
				}
			}
			else {
				Main.Logger.Log("Playback_root not found");
			}
		}
	}
}
