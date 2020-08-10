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
	public class XLSliderValueText : MonoBehaviour
	{
		private Text text;
		public static List<XLSliderValueText> xlSliderValueTexts = new List<XLSliderValueText>();

		[HideInInspector]
		public event UnityAction<float> ValueChanged = v => { };

		public void Awake() {
			text = this.gameObject.GetComponent<Text>();
			xlSliderValueTexts.Add(this);
		}

		public void OverrideValue(float value) {
			text.text = $"{value}";
		}

		public void OverrideValue(int value) {
			text.text = "" + value;
		}

		public void OnDestroy() {
			xlSliderValueTexts.Remove(this);
		}
	}
}
