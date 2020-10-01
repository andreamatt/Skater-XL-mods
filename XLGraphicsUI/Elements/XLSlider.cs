using System;
using System.Collections.Generic;
using System.Globalization;
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
		public TMP_InputField inputField;
		public bool interactable {
			get => slider.interactable;
			set {
				slider.interactable = value;
				inputField.interactable = value;
			}
		}

		[HideInInspector]
		public event UnityAction<float> onValueChanged = v => { };

		public void Awake() {
			if (slider.wholeNumbers) {
				inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
			}

			slider.onValueChanged.AddListener(new UnityAction<float>(v => {
				onValueChanged.Invoke(v);
				SetValueText(v);
			}));

			inputField.onEndEdit.AddListener(new UnityAction<string>(s => {
				try {
					var v = float.Parse(s, CultureInfo.InvariantCulture);
					slider.value = v;
				}
				catch (Exception) {
					// nothing
				}
			}));
		}

		private void SetValueText(float v) {
			if (slider.wholeNumbers) {
				valueText.text = $"{(int)v}";
			}
			else {
				valueText.text = $"{v.ToString("0.00", CultureInfo.InvariantCulture)}";
			}
		}

		public void OverrideValue(float value) {
			//slider.value = value;
			slider.SetValueWithoutNotify(value);
			SetValueText(slider.value); // use slider.value instead of value because it is clamped
		}
	}
}
