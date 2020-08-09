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
	public class XLSelectionGrid : MonoBehaviour
	{
		private Dictionary<string, Toggle> toggles;

		[HideInInspector]
		public event UnityAction<string> ValueChange = v => { };
		//private UnityAction<va>

		public void Awake() {
			toggles = this.gameObject.GetComponentsInChildren<Toggle>().ToDictionary(t => t.name);

			UnityAction<bool> changeSelected = v => {
				if (v) {
					var selected = toggles.First(kv => kv.Value.isOn);
					ValueChange.Invoke(selected.Value.name);
				}
			};

			foreach (var toggle in toggles.Values) {
				toggle.onValueChanged.AddListener(changeSelected);
			}
		}

		public void OverrideValue(string value) {
			if (toggles == null) {
				throw new Exception("Bad Start order?");
			}
			foreach (var toggle in toggles.Values) {
				//toggle.onValueChanged.RemoveAllListeners();
				//toggle.onValueChanged.li
				toggle.isOn = toggle.name == value;
			}
		}
	}
}
