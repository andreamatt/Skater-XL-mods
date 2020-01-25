using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabboSettings
{
    public class MapPresetUpdater : Module
    {
        private Preset map_preset;

        public void GetMapEffects() {
            map_preset = new Preset(Main.map_name);

            Logger.Debug("Saving to " + map_preset.name);
            try {
                if (map_preset == null) throw new Exception("preset is null");
                if (gameEffects.post_layer == null) throw new Exception("Post_layer is null");
                if (gameEffects.map_post_volume == null) throw new Exception("Map_post_volume is null");
                if (Camera.main == null) throw new Exception("maincamera is null");

                map_preset.AO = DeepClone(gameEffects.map_effectSuite.AO);
                map_preset.EXPO = DeepClone(gameEffects.map_effectSuite.EXPO);
                map_preset.BLOOM = DeepClone(gameEffects.map_effectSuite.BLOOM);
                map_preset.CA = DeepClone(gameEffects.map_effectSuite.CA);
                map_preset.COLOR = DeepClone(gameEffects.map_effectSuite.COLOR);
                map_preset.DOF = DeepClone(gameEffects.map_effectSuite.DOF);
                map_preset.FOCUS_MODE = FocusMode.Custom;
                map_preset.GRAIN = DeepClone(gameEffects.map_effectSuite.GRAIN);
                map_preset.LENS = DeepClone(gameEffects.map_effectSuite.LENS);
                map_preset.BLUR = DeepClone(gameEffects.map_effectSuite.BLUR);
                map_preset.REFL = DeepClone(gameEffects.map_effectSuite.REFL);
                map_preset.VIGN = DeepClone(gameEffects.map_effectSuite.VIGN);
            }
            catch (Exception e) {
                throw new Exception("Failed saving to preset, ex: " + e);
            }
            Logger.Debug("Saved to " + map_preset.name);

            Main.presets[map_preset.name] = map_preset;
        }

        private T DeepClone<T>(T obj) where T : ScriptableObject {
            var t = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(obj), t);
            return t;
        }
    }
}
