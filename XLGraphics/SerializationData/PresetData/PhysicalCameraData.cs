using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.EffectHandlers.PresetEffects;
using XLGraphics.Presets;
using static UnityEngine.Camera;

namespace XLGraphics.SerializationData.PresetData
{
	public class PhysicalCameraData
	{
		public bool active = false;
		public bool usePhysicalProperties = false;
		public SensorType sensorType = SensorType.Custom;
		public SerializableVector2 sensorSize = new Vector2(36, 24);
		public int iso = 200;
		public float shutterSpeed = 0.005f;
		public GateFitMode gateFit = GateFitMode.Horizontal;
		public float focalLength = 50;
		public float aperture = 16;
		public int bladeCount = 5;
		public float anamorphism = 0;
	}
}
