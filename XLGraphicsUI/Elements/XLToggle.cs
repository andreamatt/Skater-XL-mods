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

		[HideInInspector]
		public event UnityAction<bool> ValueChanged = v => { };

		public void Awake() {
			toggle = this.gameObject.GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(ValueChanged);
		}

		public void OverrideValue(bool value) {
			toggle.isOn = value;
		}
	}
}
