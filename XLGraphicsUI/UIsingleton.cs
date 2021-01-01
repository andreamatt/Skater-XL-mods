using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XLGraphicsUI
{
	public abstract class UIsingleton<T> : MonoBehaviour where T : UIsingleton<T>
	{
		public static T Instance;

		public void Awake() {
			if (Instance != null) {
				throw new Exception($"Instance of {this.GetType().FullName} not null on Awake");
			}
			Instance = (T)this;
		}
	}
}
