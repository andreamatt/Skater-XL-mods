using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphicsUI
{
	public class XLGraphicsMenu : MonoBehaviour
	{
		public static XLGraphicsMenu Instance { get; private set; }
		public Action<string> BTNclick;

		public void Awake() {
			if (Instance != null && Instance != this) {
				GameObject.Destroy(this);
			}
			else {
				Instance = this;
			}
		}

		public void OnBTNclick(string input) {
			BTNclick?.Invoke(input);
		}
	}
}
