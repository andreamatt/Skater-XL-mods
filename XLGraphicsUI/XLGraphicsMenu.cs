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

		public GameObject presetsListContent;

		public void Awake() {
			Instance = this;
		}
	}
}
