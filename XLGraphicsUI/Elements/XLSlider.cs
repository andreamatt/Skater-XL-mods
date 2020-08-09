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

		[HideInInspector]
		public event UnityAction<float> ValueChanged = v => { };

		public void Awake() {
			slider = this.gameObject.GetComponent<Slider>();
			slider.onValueChanged.AddListener(ValueChanged);
		}

		public void OverrideValue(float value) {
			slider.value = value;
		}
	}
}
