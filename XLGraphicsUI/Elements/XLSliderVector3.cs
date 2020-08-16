using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements
{
	public class XLSliderVector3 : MonoBehaviour
	{
		public Slider sliderX;
		public Slider sliderY;
		public Slider sliderZ;
		public TMP_Text valueTextX;
		public TMP_Text valueTextY;
		public TMP_Text valueTextZ;

		[HideInInspector]
		public event UnityAction<Vector3> onValueChange = v => { };

		public void Awake() {
			UnityAction<float> action = v => {
				var vec = new Vector3(sliderX.value, sliderY.value, sliderZ.value);
				onValueChange.Invoke(vec);
				SetValueText(vec);
			};
			sliderX.onValueChanged.AddListener(action);
			sliderY.onValueChanged.AddListener(action);
			sliderZ.onValueChanged.AddListener(action);
		}

		private void SetValueText(Vector3 v) {
			if (sliderX.wholeNumbers) {
				valueTextX.text = $"{(int)v.x}";
				valueTextY.text = $"{(int)v.y}";
				valueTextZ.text = $"{(int)v.z}";
			}
			else {
				valueTextX.text = $"{v.x:N}";
				valueTextY.text = $"{v.y:N}";
				valueTextZ.text = $"{v.z:N}";
			}
		}

		public void OverrideValue(Vector3 v) {
			sliderX.SetValueWithoutNotify(v.x);
			sliderY.SetValueWithoutNotify(v.y);
			sliderZ.SetValueWithoutNotify(v.z);

			SetValueText(v);
		}
	}
}
