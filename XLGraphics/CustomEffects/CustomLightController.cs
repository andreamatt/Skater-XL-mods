using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.Presets;

namespace XLGraphics.CustomEffects
{

	public class CustomLightController : MonoBehaviour
	{
		public static CustomLightController Instance { get; private set; }

		public void Awake() {
			if (Instance != null) {
				throw new Exception("Instance not null on Awake");
			}
			Instance = this;
		}

		#region Settings
		public bool LIGHT_ENABLED = false;
		public float LIGHT_RANGE = 0;
		public float LIGHT_ANGLE = 0;
		public float LIGHT_INTENSITY = 0;
		public Color LIGHT_COLOR = Color.white;
		public Vector3 LIGHT_POSITION = Vector3.zero;
		public string LIGHT_COOKIE = null;
		#endregion

		private Light light;
		private HDAdditionalLightData additionalLightData;
		public readonly string EmptyCookieName = "None";
		private Dictionary<string, Texture2D> Cookies = new Dictionary<string, Texture2D>();

		public string[] CookieNames => Cookies.Keys.ToArray();

		public void Start() {
			GetCookies();

			light = gameObject.AddComponent<Light>();

			additionalLightData = gameObject.AddComponent<HDAdditionalLightData>();
			//additionalLightData.intensity = 100;
			additionalLightData.lightUnit = LightUnit.Ev100;
			//additionalLightData.lightDimmer = 0;
			additionalLightData.volumetricDimmer = 0;

			light.type = LightType.Spot;

			light.color = LIGHT_COLOR;
			additionalLightData.intensity = LIGHT_INTENSITY;
			//light.intensity = LIGHT_INTENSITY;
			light.intensity = LIGHT_INTENSITY;
			light.range = LIGHT_RANGE;
			light.spotAngle = LIGHT_ANGLE;
			light.cookie = LIGHT_COOKIE == null ? null : Cookies[LIGHT_COOKIE];

			Logger.Log("Light started");
		}

		private void GetCookies() {
			Cookies[EmptyCookieName] = null;

			if (!Directory.Exists(Main.modEntry.Path + "Cookies")) {
				Directory.CreateDirectory(Main.modEntry.Path + "Cookies");
			}

			string[] filePaths = Directory.GetFiles(Main.modEntry.Path + "Cookies\\", "*.png");
			foreach (var filePath in filePaths) {
				// it will get auto resized
				var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
				texture.LoadImage(File.ReadAllBytes(filePath));
				texture.filterMode = FilterMode.Point;

				Cookies[Path.GetFileNameWithoutExtension(filePath)] = texture;
			}
		}

		public void Update() {
			UpdateLightData();

			var currentStateName = XLGraphics.Instance.currentGameStateName;
			if (LIGHT_ENABLED && currentStateName != "GearSelectionState" && currentStateName != "PinMovementState") {
				light.enabled = true;
				light.range = LIGHT_RANGE;
				light.spotAngle = LIGHT_ANGLE;
				additionalLightData.intensity = LIGHT_INTENSITY;
				//light.intensity = LIGHT_INTENSITY;
				light.color = LIGHT_COLOR;
				light.cookie = Cookies[LIGHT_COOKIE];

				// move relative to camera
				var camera = CustomCameraController.Instance.mainCamera;
				light.transform.position = camera.transform.TransformPoint(LIGHT_POSITION);
				light.transform.rotation = camera.transform.rotation;
			}
			else {
				//light.intensity = 0;
				additionalLightData.intensity = 0;
			}
		}

		private void UpdateLightData() {
			// get light data from presets
			var presets = PresetManager.Instance.presets;
			var presetsWithActiveLight = presets.Where(p => p.volume.isActiveAndEnabled && p.lightData.active).ToList();
			if (presetsWithActiveLight.Count == 0) {
				LIGHT_ENABLED = false;
			}
			else {
				var data = presetsWithActiveLight.First().lightData;
				LIGHT_ENABLED = true;
				LIGHT_RANGE = data.range;
				LIGHT_ANGLE = data.angle;
				LIGHT_INTENSITY = data.intensity;
				LIGHT_COLOR = data.color;
				LIGHT_POSITION = data.position;
				LIGHT_COOKIE = data.cookie ?? EmptyCookieName; // NOT A GOOD SOLUTION
			}
		}
	}
}
