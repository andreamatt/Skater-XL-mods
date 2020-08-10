using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityModManagerNet;
using XLGraphics.Presets;

namespace XLGraphics
{
	public class Settings
	{
		public string testSetting = "test";

		public PresetSelection presetOrder = new PresetSelection();
		public PresetSelection replay_presetOrder = new PresetSelection();

		// Basic
		public bool ENABLE_POST = true;
		public int VSYNC = 1;
		public FullScreenMode SCREEN_MODE = FullScreenMode.Windowed;
		public float RENDER_DISTANCE = 1000;

		// AA
		public HDAdditionalCameraData.AntialiasingMode AA_MODE = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
		//public float TAA_sharpness = new TemporalAntialiasing().sharpness;
		//public float TAA_jitter = new TemporalAntialiasing().jitterSpread;
		//public float TAA_stationary = new TemporalAntialiasing().stationaryBlending;
		//public float TAA_motion = new TemporalAntialiasing().motionBlending;
		//public SubpixelMorphologicalAntialiasing SMAA = new SubpixelMorphologicalAntialiasing();

		// Camera
		//public CameraMode CAMERA = CameraMode.Normal;
		public float REPLAY_FOV = 60;
		public float NORMAL_FOV = 60;
		public float NORMAL_REACT = 0.90f;
		public float NORMAL_REACT_ROT = 0.90f;
		public float NORMAL_CLIP = 0.01f;
		public float FOLLOW_FOV = 60;
		public float FOLLOW_REACT = 0.70f;
		public float FOLLOW_REACT_ROT = 0.70f;
		public float FOLLOW_CLIP = 0.01f;
		public Vector3 FOLLOW_SHIFT = Vector3.zero;
		public bool FOLLOW_AUTO_SWITCH = false;
		public float POV_FOV = 80;
		public float POV_REACT = 1;
		public float POV_REACT_ROT = 0.07f;
		public float POV_CLIP = 0.01f;
		public bool HIDE_HEAD = true;
		public Vector3 POV_SHIFT = new Vector3(0, 0, 0.13f);
		public float SKATE_FOV = 60;
		public float SKATE_REACT = 0.90f;
		public float SKATE_REACT_ROT = 0.90f;
		public float SKATE_CLIP = 0.01f;
		public Vector3 SKATE_SHIFT = Vector3.zero;

		// Replay Controls
		public float positionSpeed = 10f;
		public float rotationSpeed = 50f;
		public float fovSpeed = 50f;

		public Task Save() {
			return Task.Run(() => {
				var filepath = $"{Main.modEntry.Path}Settings.json";
				try {
					using (var writer = new StreamWriter(filepath)) {
						var json = JsonConvert.SerializeObject(this, Formatting.Indented);
						writer.Write(json);
					}
				}
				catch (Exception e) {
					Logger.Log($"Can't save {filepath}. ex: {e}");
				}
			});
		}

		public static Settings Load() {
			var filepath = $"{Main.modEntry.Path}Settings.json";
			Settings settings = null;
			if (File.Exists(filepath)) {
				try {
					using (var reader = new StreamReader(filepath)) {
						var json = reader.ReadToEnd();
						settings = JsonConvert.DeserializeObject<Settings>(json);
						if (settings == null) {
							Debug.LogWarning("Could not read settings, creating new");
							settings = new Settings();
						}
					}
				}
				catch (Exception e) {
					Logger.Log($"Can't read {filepath}. ex: {e}");
				}
			}
			else {
				Logger.Log($"No settings found, using defaults");
				settings = new Settings();
			}
			return settings;
		}
	}
}
