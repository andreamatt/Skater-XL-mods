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
		private Slider slider;
		private TMP_Text text;
		public static List<XLSlider> xlSliders = new List<XLSlider>();

		[HideInInspector]
		public event UnityAction<float> ValueChange = v => { };

		public void Awake() {
			slider = this.gameObject.GetComponentInChildren<Slider>();
			text = this.gameObject.GetComponentInChildren<TMP_Text>();
			UnityAction<float> action = v => {
				ValueChange.Invoke(v);
				if (slider.wholeNumbers) {
					text.text = $"{(int)v}";
				}
				else {
					text.text = $"{v:3f}";
				}
			};
			slider.onValueChanged.AddListener(action);
			xlSliders.Add(this);
		}

		public void OverrideValue(float value) {
			slider.value = value;
		}

		public void OnDestroy() {
			xlSliders.Remove(this);
		}
	}
}
