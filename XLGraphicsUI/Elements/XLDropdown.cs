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
	public class XLDropdown : MonoBehaviour
	{
		public Button expandBTN;
		public Button collapseBTN;
		public GameObject properties;

		void Start() {
			var animator = gameObject.GetComponent<Animator>();

			expandBTN.gameObject.SetActive(true);
			collapseBTN.gameObject.SetActive(true);

			expandBTN.onClick.AddListener(new UnityAction(() => {
				expandBTN.gameObject.SetActive(false);
				collapseBTN.gameObject.SetActive(true);
				animator.Play("Expand");
				properties.SetActive(true);
			}));

			collapseBTN.onClick.AddListener(new UnityAction(() => {
				collapseBTN.gameObject.SetActive(false);
				expandBTN.gameObject.SetActive(true);
				animator.Play("Collapse");
				properties.SetActive(false);
			}));

			collapseBTN.gameObject.SetActive(false);
		}
	}
}
