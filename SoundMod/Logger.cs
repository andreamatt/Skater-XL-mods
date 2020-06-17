using SoundMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace SoundMod
{
	public static class Logger
	{
		public static void Log(string s) {
			UnityModManager.Logger.Log("[SoundMod] " + s);
		}
	}
}
