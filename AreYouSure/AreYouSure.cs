using AreYouSure.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AreYouSure
{
	class AreYouSure : MonoBehaviour
	{
		private ReplayWindow replayWindow;
		private ExitWindow exitWindow;
		public void Start() {
			replayWindow = this.gameObject.AddComponent<ReplayWindow>();
			ReplayState_OnUpdate_Patch.window = replayWindow;

			exitWindow = this.gameObject.AddComponent<ExitWindow>();
			GameStateMachine_Update_Patch.window = exitWindow;
		}
	}
}
