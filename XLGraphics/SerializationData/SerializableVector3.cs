using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.SerializationData
{
	public class SerializableVector3
	{

		public float x;
		public float y;
		public float z;

		[JsonIgnore]
		public Vector3 Vector3 {
			get { return new Vector3(x, y, z); }
			set { x = value.x; y = value.y; z = value.z; }
		}

		public static implicit operator Vector3(SerializableVector3 instance) {
			if (instance == null) {
				return new Vector3(0, 0, 0);
			}
			return instance.Vector3;
		}

		public static implicit operator SerializableVector3(Vector3 vector3) {
			return new SerializableVector3 { Vector3 = vector3 };
		}
	}
}
