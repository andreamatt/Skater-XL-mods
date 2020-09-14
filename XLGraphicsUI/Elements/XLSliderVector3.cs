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
		//public Slider sliderX;
		//public Slider sliderY;
		//public Slider sliderZ;
		//public TMP_Text valueTextX;
		//public TMP_Text valueTextY;
		//public TMP_Text valueTextZ;
		public XLSlider sliderX;
		public XLSlider sliderY;
		public XLSlider sliderZ;

		[HideInInspector]
		public event UnityAction<Vector3> onValueChange = v => { };

		public void Awake() {
			UnityAction<float> action = v => {
				var vec = new Vector3(sliderX.slider.value, sliderY.slider.value, sliderZ.slider.value);
				onValueChange.Invoke(vec);
				SetValueText(vec);
			};
			sliderX.onValueChange += action;
			sliderY.onValueChange += action;
			sliderZ.onValueChange += action;
		}

		private void SetValueText(Vector3 v) {
			if (sliderX.slider.wholeNumbers) {
				sliderX.valueText.text = $"{(int)v.x}";
				sliderY.valueText.text = $"{(int)v.y}";
				sliderZ.valueText.text = $"{(int)v.z}";
			}
			else {
				sliderX.valueText.text = $"{v.x:N}";
				sliderY.valueText.text = $"{v.y:N}";
				sliderZ.valueText.text = $"{v.z:N}";
			}
		}

		public void OverrideValue(Vector3 v) {
			sliderX.OverrideValue(v.x);
			sliderY.OverrideValue(v.y);
			sliderZ.OverrideValue(v.z);

			SetValueText(v);
		}
	}
}
