using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabboSettings
{
	public abstract class Module : MonoBehaviour
	{
		protected class CachedModule<T> where T : Module
		{
			private T _Instance;
			public T Instance {
				get {
					if (_Instance != null) return _Instance;
					return _Instance = BabboSettings.Instance.gameObject.GetComponent<T>();
				}
			}
		}

		private CachedModule<DayNightController> _DayNightController = new CachedModule<DayNightController>();
		private CachedModule<CustomCameraController> _CustomCameraController = new CachedModule<CustomCameraController>();
		private CachedModule<GameEffects> _GameEffects = new CachedModule<GameEffects>();
		private CachedModule<Window> _Window = new CachedModule<Window>();
		private CachedModule<PresetsManager> _PresetsManager = new CachedModule<PresetsManager>();
		private CachedModule<MapPresetUpdater> _MapPreset = new CachedModule<MapPresetUpdater>();
		private CachedModule<LightController> _LightController = new CachedModule<LightController>();

		protected DayNightController dayNightController => _DayNightController.Instance;
		protected CustomCameraController cameraController => _CustomCameraController.Instance;
		protected GameEffects gameEffects => _GameEffects.Instance;
		protected Window window => _Window.Instance;
		protected PresetsManager presetsManager => _PresetsManager.Instance;
		protected MapPresetUpdater mapPreset => _MapPreset.Instance;
		protected LightController lightController => _LightController.Instance;

		virtual public void Start() { }
		virtual public void FixedUpdate() { }
		virtual public void Update() { }
		virtual public void LateUpdate() { }
		virtual public void OnGUI() { }
	}
}
