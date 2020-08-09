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
	public class XLButton : MonoBehaviour
	{
		private Button button;

		[HideInInspector]
		public event UnityAction Click = () => { };

		public void Awake() {
			button = this.gameObject.GetComponent<Button>();
			button.onClick.AddListener(Click);
		}
	}
}
