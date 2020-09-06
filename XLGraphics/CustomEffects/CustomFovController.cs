using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphics.CustomEffects
{

	public class CustomFovController : MonoBehaviour
	{
		public static CustomFovController Instance { get; private set; }

		public void Awake() {
			if (Instance != null) {
				throw new Exception("Instance not null on Awake");
			}
			Instance = this;
		}
	}
}
