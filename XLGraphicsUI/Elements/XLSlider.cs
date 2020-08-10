using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements
{
	public class XLSlider : MonoBehaviour
	{
		private Slider slider;
		public static List<XLSlider> xlSliders = new List<XLSlider>();

		[HideInInspector]
		public event UnityAction<float> ValueChanged = v => { };

		public void Awake() {
			slider = this.gameObject.GetComponent<Slider>();
			UnityAction<float> action = v => ValueChanged.Invoke(v);
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
