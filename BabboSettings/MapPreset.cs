using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabboSettings
{
    public class MapPreset : Module
    {
        private Preset map_preset;

        public void GetMapEffects() {
            bool map_preset_enabled = false;
            if (Main.presets.ContainsKey(Main.map_name)) map_preset_enabled = Main.presets[Main.map_name].enabled;
            map_preset = new Preset(Main.map_name);
            map_preset.enabled = map_preset_enabled;

            Logger.Debug("Saving to " + map_preset.name);
            try {
                if (map_preset == null) throw new Exception("preset is null");
                if (gameEffects.post_layer == null) throw new Exception("Post_layer is null");
                if (gameEffects.post_volume == null) throw new Exception("Post_volume is null");
                if (Camera.main == null) throw new Exception("maincamera is null");

                map_preset.AO = DeepClone(gameEffects.AO);
                map_preset.EXPO = DeepClone(gameEffects.EXPO);
                map_preset.BLOOM = DeepClone(gameEffects.BLOOM);
                map_preset.CA = DeepClone(gameEffects.CA);
                map_preset.COLOR = DeepClone(gameEffects.COLOR);
                map_preset.DOF = DeepClone(gameEffects.DOF);
                map_preset.FOCUS_MODE = FocusMode.Custom;
                map_preset.GRAIN = DeepClone(gameEffects.GRAIN);
                map_preset.LENS = DeepClone(gameEffects.LENS);
                map_preset.BLUR = DeepClone(gameEffects.BLUR);
                map_preset.REFL = DeepClone(gameEffects.REFL);
                map_preset.VIGN = DeepClone(gameEffects.VIGN);

                map_preset.Save();
            }
            catch (Exception e) {
                throw new Exception("Failed saving to preset, ex: " + e);
            }
            Logger.Debug("Saved to " + map_preset.name);

            Main.presets[map_preset.name] = map_preset;
            if (!Main.settings.presetOrder.Contains(Main.map_name)) {
                Main.settings.presetOrder.Add(Main.map_name);
            }
        }

        private T DeepClone<T>(T obj) where T : ScriptableObject {
            var t = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(obj), t);
            return t;
        }
    }
}
