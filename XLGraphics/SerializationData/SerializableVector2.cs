
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.SerializationData
{
	public class SerializableVector2
	{

		public float x;
		public float y;

		[JsonIgnore]
		public Vector2 Vector2 {
			get { return new Vector2(x, y); }
			set { x = value.x; y = value.y; }
		}

		public static implicit operator Vector2(SerializableVector2 instance) {
			if (instance == null) {
				return new Vector2(0, 0);
			}
			return instance.Vector2;
		}

		public static implicit operator SerializableVector2(Vector2 vector2) {
			return new SerializableVector2 { Vector2 = vector2 };
		}
	}
}
