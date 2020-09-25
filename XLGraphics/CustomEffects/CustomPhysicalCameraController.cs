using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.Presets;
using XLGraphics.SerializationData.PresetData;

namespace XLGraphics.CustomEffects
{
	public class CustomPhysicalCameraController : MonoBehaviour
	{
		public static CustomPhysicalCameraController Instance { get; private set; }
		private HDAdditionalCameraData hdCameraData;
		private Camera camera;
		private readonly PhysicalCameraData defaultCameraData = new PhysicalCameraData();
		private PhysicalCameraData currentCameraData;

		public void Awake() {
			if (Instance != null) {
				throw new Exception("Instance not null on Awake");
			}
			Instance = this;
		}

		public void Start() {
			camera = Camera.main;
			hdCameraData = camera.GetComponent<HDAdditionalCameraData>();
		}

		public void LateUpdate() {
			UpdateCameraData();
			ApplyCameraData();
		}

		public void UpdateCameraData() {
			var presets = PresetManager.Instance.presets;
			var presetsWithActivePC = presets.Where(p => p.enabled && p.physicalCameraData.active).ToList();
			if (presetsWithActivePC.Count == 0) {
				currentCameraData = defaultCameraData;
			}
			else {
				currentCameraData = presetsWithActivePC.First().physicalCameraData;
			}
		}

		public void ApplyCameraData() {
			var data = currentCameraData;
			if (data.sensorType == SensorType.Custom) {
				camera.sensorSize = data.sensorSize;
			}
			else {
				camera.sensorSize = sensorTypeToSize[(int)data.sensorType];
			}
			camera.usePhysicalProperties = data.usePhysicalProperties;
			hdCameraData.physicalParameters.iso = data.iso;
			hdCameraData.physicalParameters.shutterSpeed = data.shutterSpeed;
			camera.gateFit = data.gateFit;
			camera.focalLength = data.focalLength;
			hdCameraData.physicalParameters.aperture = data.aperture;
			hdCameraData.physicalParameters.bladeCount = data.bladeCount;
			hdCameraData.physicalParameters.anamorphism = data.anamorphism;
		}

		public static List<Vector2> sensorTypeToSize = new List<Vector2>() {
			new Vector2(4.8f,5f),
			new Vector2(5.79f,4.01f),
			new Vector2(10.26f,7.49f),
			new Vector2(12.52f,7.41f),
			new Vector2(21.95f,9.35f),
			new Vector2(21,15.2f),
			new Vector2(24.89f,18.66f),
			new Vector2(54.12f,25.59f),
			new Vector2(70,51),
			new Vector2(70.41f,52.63f)
		};
	}
}
