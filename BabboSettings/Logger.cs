using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace BabboSettings
{
	static class Logger
	{
		public static void Log(string s) {
			UnityModManager.Logger.Log("[BabboSettings] " + s);
		}

		public static void Debug(string s) {
			if (Main.settings.DEBUG) {
				Log("[DBG] " + s);
			}
		}
	}
}
