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
			Instance = (T)this;
		}
	}
}
