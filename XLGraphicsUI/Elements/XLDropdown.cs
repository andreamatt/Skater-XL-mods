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
	public class XLDropdown : MonoBehaviour
	{
		public static List<XLDropdown> xlDropdowns = new List<XLDropdown>();

		[HideInInspector]
		public event UnityAction<int> ValueChange = v => { };
		private TMP_Dropdown dropdown;

		public void Awake() {
			xlDropdowns.Add(this);
			dropdown = GetComponentInChildren<TMP_Dropdown>();

			UnityAction<int> action = v => ValueChange.Invoke(v);
			dropdown.onValueChanged.AddListener(action);
		}

		public void OverrideValue(int value) {
			dropdown.value = value;
		}

		public void OnDestroy() {
			xlDropdowns.Remove(this);
		}
	}
}
