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
	public class XLSlider : MonoBehaviour
	{
		public Slider slider;
		public TMP_Text valueText;

		[HideInInspector]
		public event UnityAction<float> onValueChange = v => { };

		public void Awake() {
			UnityAction<float> action = v => {
				onValueChange.Invoke(v);
				SetValueText(v);
			};
			slider.onValueChanged.AddListener(action);
		}

		private void SetValueText(float v) {
			if (slider.wholeNumbers) {
				valueText.text = $"{(int)v}";
			}
			else {
				valueText.text = $"{v:N}";
			}
		}

		public void OverrideValue(float value) {
			//slider.value = value;
			slider.SetValueWithoutNotify(value);
			SetValueText(slider.value); // use slider.value instead of value because it is clamped
		}
	}
}
