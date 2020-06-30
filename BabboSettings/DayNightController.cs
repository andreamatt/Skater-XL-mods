using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace BabboSettings
{
	public class DayNightController : Module
	{
		public override void Start() {
			GetLights();
			DisableBakedLights();
		}

		private Light Sun;
		private Light Sky0;
		private Light Sky1;
		private float SunInitialIntensity;
		private float SkyInitialIntensity0;
		private float SkyInitialIntensity1;
		private float secondsInDay = 20;
		private float currentTime = 0.5f;
		private float timeMult = 1;

		public void GetLights() {
			var lights = GameObject.FindObjectsOfType<Light>();
			foreach (var l in lights) {
				Logger.Log("Light: " + l.name);
			}
			Sun = lights.FirstOrDefault(l => l.name.ToLower().Contains("sun") || l.name.ToLower().Contains("directional"));
			if (Sun == null) {
				if (lights.Length > 1) {
					Logger.Log("Too many lights");
				}
				else if (lights.Length == 1) {
					Sun = lights[0];
					Logger.Log("Got first light available");
				}
				else {
					Logger.Log("No lights found");
				}
			}

			Sky0 = lights[0];
			Sky1 = lights[2];

			SunInitialIntensity = Sun.intensity;
			SkyInitialIntensity0 = Sky0.intensity;
			SkyInitialIntensity1 = Sky1.intensity;
		}

		public override void Update() {
			UpdateSun();
			UpdateTime();
			DisableBakedLights();
		}

		private void DisableBakedLights() {
			var lightMaps = LightmapSettings.lightmaps;
			Logger.Log($"Found {lightMaps.Length} lightmaps");

			var turnedOffLightmaps = new LightmapData[lightMaps.Length];

			for (int i = 0; i < turnedOffLightmaps.Length; i++) {
				var thisOriginalLightmap = lightMaps[i];
				var thisTurnedOffLightmap = new LightmapData();

				thisTurnedOffLightmap.lightmapDir = thisOriginalLightmap.lightmapDir;
				thisTurnedOffLightmap.shadowMask = thisOriginalLightmap.shadowMask;
				thisTurnedOffLightmap.lightmapColor = Texture2D.blackTexture;

				turnedOffLightmaps[i] = thisTurnedOffLightmap;
			}

			LightmapSettings.lightmaps = turnedOffLightmaps;
		}

		private void UpdateSun() {
			Sun.transform.localRotation = Quaternion.Euler(-161, 130, 90);//Quaternion.Euler((currentTime * 360f) - 90, 170, 0);

			float intensityMult = 1;
			//if (currentTime <= 0.25f || currentTime >= 0.75f) {
			//    intensityMult = 0;
			//}
			//else if (currentTime < 0.25f) {
			//    intensityMult = Mathf.Clamp01((currentTime - 0.25f) * (1 - 0.02f));
			//}
			//else if (currentTime > 0.75f) {
			//    intensityMult = Mathf.Clamp01(1 - (currentTime - 0.75f) * (1 - 0.02f));
			//}

			Sun.intensity = SunInitialIntensity * intensityMult;
			Sky0.intensity = 0;//SkyInitialIntensity0 * intensityMult;
			Sky1.intensity = 0;//SkyInitialIntensity1 * intensityMult;
		}

		private void UpdateTime() {
			currentTime += (Time.deltaTime / secondsInDay) * timeMult;
			if (currentTime >= 1) currentTime = 0;
		}
	}
}
