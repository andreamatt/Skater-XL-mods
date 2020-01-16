using System;
using System.Collections.Generic;
using System.Text;
using UnityModManagerNet;

namespace GravityMod
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings
    {

        public float G = -9.8f;

        public Settings() {
        }

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
