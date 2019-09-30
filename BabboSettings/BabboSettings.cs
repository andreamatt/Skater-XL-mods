using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
    public class BabboSettings : MonoBehaviour
    {
        private GameEffects effects;
        private Window window;
        private MapPreset mapPreset;
        private CustomCameraController cameraController;

        public void Start() {
            DontDestroyOnLoad(gameObject);

            effects = GameEffects.Instance;
            effects.checkAndGetEffects();

            mapPreset = MapPreset.Instance;

            window = Window.Instance;

            cameraController = CustomCameraController.Instance;
        }

        private void Update() {
            // if the map changed, needs to find/create new effects
            effects.checkAndGetEffects();
            window.Update();
        }

        private void LateUpdate() {
            effects.LateUpdate();
            cameraController.LateUpdate();
        }

        private void FixedUpdate() {
            cameraController.FixedUpdate();
        }

        private void OnGUI() {
            window.OnGUI();
        }

        public bool isSwitch() {
            return cameraController.isSwitch();
        }

        public void SetJustRespawned() {
            cameraController.just_respawned = true;
        }

        public void SetSpawnSwitch() {
            cameraController.spawn_switch = isSwitch();
        }
    }
}
