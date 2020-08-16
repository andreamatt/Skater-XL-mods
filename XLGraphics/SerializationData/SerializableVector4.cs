using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.SerializationData
{
	public class SerializableVector4
	{

		public float x;
		public float y;
		public float z;
		public float w;

		[JsonIgnore]
		public Vector4 Vector4 {
			get { return new Vector4(x, y, z, w); }
			set { x = value.x; y = value.y; z = value.z; w = value.w; }
		}

		public static implicit operator Vector4(SerializableVector4 instance) {
			if (instance == null) {
				return new Vector4(0, 0, 0, 0);
			}
			return instance.Vector4;
		}

		public static implicit operator SerializableVector4(Vector4 vector4) {
			return new SerializableVector4 { Vector4 = vector4 };
		}
	}
}
