using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace BabboSettings
{
    public class LightController : Module
	{
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

		public override void Start() {
			GetCookies();

			var camera = cameraController.mainCamera;
			light = gameObject.AddComponent<Light>();
            
            additionalLightData = gameObject.AddComponent<HDAdditionalLightData>();
            additionalLightData.volumetricDimmer = 0;

            light.type = LightType.Spot;
			light.color = LIGHT_COLOR;
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

		public override void Update() {
			if (LIGHT_ENABLED) {
				light.enabled = true;
				light.range = LIGHT_RANGE;
				light.spotAngle = LIGHT_ANGLE;
				light.intensity = LIGHT_INTENSITY;
				light.color = LIGHT_COLOR;
				light.cookie = Cookies[LIGHT_COOKIE];
                
                // move relative to camera
				var camera = cameraController.mainCamera;
				light.transform.position = camera.transform.TransformPoint(LIGHT_POSITION);
                light.transform.rotation = camera.transform.rotation;
			}
			else {
				light.intensity = 0;
			}
		}
	}
}
