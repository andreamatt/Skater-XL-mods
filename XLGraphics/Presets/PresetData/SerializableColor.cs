using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.Presets.PresetData
{
	public class SerializableColor
	{

		public float r;
		public float g;
		public float b;
		public float a;

		public Color Color {
			get { return new Color(r, g, b, a); }
			set { r = value.r; g = value.g; b = value.b; a = value.a; }
		}

		//makes this class usable as Color, Color normalColor = mySerializableColor;
		public static implicit operator Color(SerializableColor instance) {
			return instance.Color;
		}

		//makes this class assignable by Color, SerializableColor myColor = Color.white;
		public static implicit operator SerializableColor(Color color) {
			return new SerializableColor { Color = color };
		}
	}
}
