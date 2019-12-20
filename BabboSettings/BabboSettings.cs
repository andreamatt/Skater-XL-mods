using GameManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
    public class BabboSettings : MonoBehaviour
    {
        public static BabboSettings Instance { private set; get; }

        private GameEffects effects;
        private Window window;
        private MapPreset mapPreset;
        private CustomCameraController cameraController;
        public PresetsManager presetsManager;
        //private DayNightController dayNightController;

        public void Start() {
            if (Instance != null) {
                Logger.Log("BabboSettings Instance already exists");
            }
            else {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);

            effects = gameObject.AddComponent<GameEffects>();
            mapPreset = gameObject.AddComponent<MapPreset>();
            window = gameObject.AddComponent<Window>();
            cameraController = gameObject.AddComponent<CustomCameraController>();
            presetsManager = gameObject.AddComponent<PresetsManager>();
            //dayNightController = DayNightController.Instance;

            effects.checkAndGetEffects();
        }

        private void Update() {
            // if the map changed, needs to find/create new effects
            effects.checkAndGetEffects();
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
        public static bool IsReplayActive() {
            return GameStateMachine.Instance.CurrentState.GetType() == typeof(ReplayState);
        }
    }
}
