using System;
using System.Collections.Generic;
using System.Text;
using UnityModManagerNet;

namespace SoundMod
{
	[Serializable]
	public class Settings : UnityModManager.ModSettings
	{
		public Settings() {
		}
		public override void Save(UnityModManager.ModEntry modEntry) {
			Save(this, modEntry);
		}
	}
}
