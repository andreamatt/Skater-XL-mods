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
		public Button buttonOn;
		public Button buttonOff;
		public bool interactable {
			get => buttonOff.interactable || buttonOn.interactable;
			set {
				buttonOff.interactable = value;
				buttonOn.interactable = value;
			}
		}

		[HideInInspector]
		public event UnityAction<bool> onValueChanged = v => { };

		public void Start() {
			buttonOn.onClick.AddListener(new UnityAction(() => SetStatus(false)));
			buttonOff.onClick.AddListener(new UnityAction(() => SetStatus(true)));
		}

		private void SetStatus(bool value, bool notify = true) {
			buttonOn.gameObject.SetActive(value);
			buttonOff.gameObject.SetActive(!value);
			if (notify) {
				onValueChanged.Invoke(value);
			}
		}

		public void SetIsOnWithoutNotify(bool value) {
			SetStatus(value, false);
		}
	}
}
