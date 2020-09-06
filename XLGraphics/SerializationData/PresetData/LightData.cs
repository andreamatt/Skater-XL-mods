using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace XLGraphics.SerializationData.PresetData
{
	public class LightData
	{
		public bool active = false;
		public float intensity = 5f;
		public float range = 10;
		public float angle = 150;
		public SerializableColor color = Color.white;
		public SerializableVector3 position = Vector3.zero;
		public string cookie = null;
	}
}
