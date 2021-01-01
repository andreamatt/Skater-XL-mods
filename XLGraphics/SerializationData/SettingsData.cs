using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace XLGraphics.SerializationData
{
	public class SettingsData
	{
		// Basic
		public bool ENABLE_POST = true;
		public int VSYNC = 1;
		public FullScreenMode SCREEN_MODE = FullScreenMode.Windowed;
		public float RENDER_DISTANCE = 1000;
		public List<string> activeOverlays = new List<string>();

		// AA
		public HDAdditionalCameraData.AntialiasingMode AA_MODE = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
		//public float TAA_sharpness = new TemporalAntialiasing().sharpness;
		//public float TAA_jitter = new TemporalAntialiasing().jitterSpread;
		//public float TAA_stationary = new TemporalAntialiasing().stationaryBlending;
		//public float TAA_motion = new TemporalAntialiasing().motionBlending;
		//public SubpixelMorphologicalAntialiasing SMAA = new SubpixelMorphologicalAntialiasing();
	}
}
