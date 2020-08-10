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
	public class XLToggle : MonoBehaviour
	{
		private Toggle toggle;

		public static List<XLToggle> xlToggles = new List<XLToggle>();

		[HideInInspector]
		public event UnityAction<bool> ValueChanged = v => { };

		public void Awake() {
			toggle = this.gameObject.GetComponent<Toggle>();
			UnityAction<bool> action = v => ValueChanged.Invoke(v);
			toggle.onValueChanged.AddListener(action);
			xlToggles.Add(this);
		}

		public void OverrideValue(bool value) {
			toggle.isOn = value;
		}

		public void OnDestroy() {
			xlToggles.Remove(this);
		}
	}
}
