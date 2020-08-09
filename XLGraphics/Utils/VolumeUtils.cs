using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace XLGraphics.Utils
{
	class VolumeUtils
	{
		private XLGraphics xlGraphics;

		private VolumeUtils() { }

		public VolumeUtils(XLGraphics xlGraphics) {
			this.xlGraphics = xlGraphics;
		}

		public float GetHighestPriority() {
			var volumes = UnityEngine.Object.FindObjectsOfType<Volume>();
			Logger.Log($"Found {volumes.Length} volumes");
			if (volumes.Length == 0) {
				return 0;
			}
			return volumes.Select(v => v.priority).Max();
		}

		public Volume CreateVolume(float priority) {
			var volume = new GameObject().AddComponent<Volume>();
			UnityEngine.Object.DontDestroyOnLoad(volume.gameObject);
			volume.priority = priority;
			volume.isGlobal = true;
			return volume;
		}
	}
}
