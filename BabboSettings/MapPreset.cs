using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabboSettings
{
    sealed class MapPreset
    {
        #region Singleton
        private static readonly Lazy<MapPreset> _Instance = new Lazy<MapPreset>(() => new MapPreset());

        private MapPreset() { }

        public static MapPreset Instance {
            get => _Instance.Value;
        }
        #endregion

        private Preset map_preset;
        private GameEffects effects { get => GameEffects.Instance; }

        public void GetMapEffects() {
            bool map_preset_enabled = false;
            if (Main.presets.ContainsKey(Main.map_name)) map_preset_enabled = Main.presets[Main.map_name].enabled;
            map_preset = new Preset(Main.map_name);
            map_preset.enabled = map_preset_enabled;

            Logger.Debug("Saving to " + map_preset.name);
            try {
                if (map_preset == null) throw new Exception("preset is null");
                if (effects.post_layer == null) throw new Exception("Post_layer is null");
                if (effects.post_volume == null) throw new Exception("Post_volume is null");
                if (Camera.main == null) throw new Exception("maincamera is null");

                map_preset.AO = DeepClone(effects.AO);
                map_preset.EXPO = DeepClone(effects.EXPO);
                map_preset.BLOOM = DeepClone(effects.BLOOM);
                map_preset.CA = DeepClone(effects.CA);
                map_preset.COLOR = DeepClone(effects.COLOR);
                map_preset.DOF = DeepClone(effects.DOF);
                map_preset.FOCUS_MODE = FocusMode.Custom;
                map_preset.GRAIN = DeepClone(effects.GRAIN);
                map_preset.LENS = DeepClone(effects.LENS);
                map_preset.BLUR = DeepClone(effects.BLUR);
                map_preset.REFL = DeepClone(effects.REFL);
                map_preset.VIGN = DeepClone(effects.VIGN);

                map_preset.Save();
            }
            catch (Exception e) {
                throw new Exception("Failed saving to preset, ex: " + e);
            }
            Logger.Debug("Saved to " + map_preset.name);

            Main.presets[map_preset.name] = map_preset;
            if (!Main.settings.presetOrder.Contains(Main.map_name)) Main.settings.presetOrder.Add(Main.map_name);
        }

        private T DeepClone<T>(T obj) where T : ScriptableObject {
            var t = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(obj), t);
            return t;
        }
    }
}
