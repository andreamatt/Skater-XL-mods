using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements
{
	public class XLInputField : MonoBehaviour
	{
		public static List<XLInputField> textFields = new List<XLInputField>();
		public static bool anyFocused => textFields.Any(tf => tf.isFocused);

		public InputField inputField;
		public TMP_InputField tmp_inputField;
		public bool interactable {
			get {
				if (inputField != null) {
					return inputField.interactable;
				}
				else {
					return tmp_inputField.interactable;
				}
			}
			set {
				if (inputField != null) {
					inputField.interactable = value;
				}
				else {
					tmp_inputField.interactable = value;
				}
			}
		}

		private bool isFocused {
			get {
				if (!this.isActiveAndEnabled) {
					return false;
				}
				if (inputField != null) {
					return inputField.isFocused;
				}
				else {
					return tmp_inputField.isFocused;
				}
			}
		}

		public void Start() {
			textFields.Add(this);
		}

		public void OnDestroy() {
			textFields.Remove(this);
		}
	}
}
