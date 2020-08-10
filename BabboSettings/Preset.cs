using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BabboSettings
{
	[Serializable]
	public class Preset
	{
		public string name = "no_name";
		public string[] serialized_effects = new string[11];
		public bool isMapPreset => name == Main.map_name;

		public FocusMode FOCUS_MODE = FocusMode.Custom;

		// FOV override (especially for lens distortion / fisheye presets)
		public bool OVERRIDE_FOV = false;
		public float OVERRIDE_FOV_VALUE = 90f;

		// LIGHT
		public bool LIGHT_ENABLED = false;
		public float LIGHT_RANGE = 0;
		public float LIGHT_ANGLE = 0;
		public float LIGHT_INTENSITY = 0;
		public Color LIGHT_COLOR = Color.white;
		public Vector3 LIGHT_POSITION = Vector3.zero;
		public string LIGHT_COOKIE = null;

		// Effects
		[NonSerialized]
		public AmbientOcclusion AO = ScriptableObject.CreateInstance<AmbientOcclusion>();
		[NonSerialized]
		public AutoExposure EXPO = ScriptableObject.CreateInstance<AutoExposure>();
		[NonSerialized]
		public Bloom BLOOM = ScriptableObject.CreateInstance<Bloom>();
		[NonSerialized]
		public ChromaticAberration CA = ScriptableObject.CreateInstance<ChromaticAberration>();
		[NonSerialized]
		public ColorGrading COLOR = ScriptableObject.CreateInstance<ColorGrading>();
		[NonSerialized]
		public DepthOfField DOF = ScriptableObject.CreateInstance<DepthOfField>();
		[NonSerialized]
		public Grain GRAIN = ScriptableObject.CreateInstance<Grain>();
		[NonSerialized]
		public LensDistortion LENS = ScriptableObject.CreateInstance<LensDistortion>();
		[NonSerialized]
		public MotionBlur BLUR = ScriptableObject.CreateInstance<MotionBlur>();
		[NonSerialized]
		public ScreenSpaceReflections REFL = ScriptableObject.CreateInstance<ScreenSpaceReflections>();
		[NonSerialized]
		public Vignette VIGN = ScriptableObject.CreateInstance<Vignette>();

		public Preset() {
			DisableAll();
		}

		public Preset(string name) {
			DisableAll();
			this.name = name;
		}

		private void DisableAll() {
			AO.enabled.Override(false);
			EXPO.enabled.Override(false);
			BLOOM.enabled.Override(false);
			CA.enabled.Override(false);
			COLOR.enabled.Override(false);
			DOF.enabled.Override(false);
			GRAIN.enabled.Override(false);
			LENS.enabled.Override(false);
			BLUR.enabled.Override(false);
			REFL.enabled.Override(false);
			VIGN.enabled.Override(false);
		}

		private void Serialize() {
			serialized_effects[0] = JsonUtility.ToJson(AO, true);
			serialized_effects[1] = JsonUtility.ToJson(EXPO, true);
			serialized_effects[2] = JsonUtility.ToJson(BLOOM, true);
			serialized_effects[3] = JsonUtility.ToJson(CA, true);
			serialized_effects[4] = JsonUtility.ToJson(COLOR, true);
			serialized_effects[5] = JsonUtility.ToJson(DOF, true);
			serialized_effects[6] = JsonUtility.ToJson(GRAIN, true);
			serialized_effects[7] = JsonUtility.ToJson(LENS, true);
			serialized_effects[8] = JsonUtility.ToJson(BLUR, true);
			serialized_effects[9] = JsonUtility.ToJson(REFL, true);
			serialized_effects[10] = JsonUtility.ToJson(VIGN, true);
		}

		private void Deserialize() {
			JsonUtility.FromJsonOverwrite(serialized_effects[0], AO);
			JsonUtility.FromJsonOverwrite(serialized_effects[1], EXPO);
			JsonUtility.FromJsonOverwrite(serialized_effects[2], BLOOM);
			JsonUtility.FromJsonOverwrite(serialized_effects[3], CA);
			JsonUtility.FromJsonOverwrite(serialized_effects[4], COLOR);
			JsonUtility.FromJsonOverwrite(serialized_effects[5], DOF);
			JsonUtility.FromJsonOverwrite(serialized_effects[6], GRAIN);
			JsonUtility.FromJsonOverwrite(serialized_effects[7], LENS);
			JsonUtility.FromJsonOverwrite(serialized_effects[8], BLUR);
			JsonUtility.FromJsonOverwrite(serialized_effects[9], REFL);
			JsonUtility.FromJsonOverwrite(serialized_effects[10], VIGN);
		}

		public Task Save() {
			return Task.Run(() => {
				var filepath = Main.modEntry.Path + "Presets\\";
				try {
					using (var writer = new StreamWriter($"{filepath}{name}.preset.json")) {
						Serialize();
						var serializedLine = JsonUtility.ToJson(this, true);
						writer.Write(serializedLine);
					}
				}
				catch (Exception e) {
					Logger.Log($"Can't save {filepath}{name} preset. ex: {e}");
				}
			});
		}

		static public Preset Load(string json) {
			var p = new Preset();
			JsonUtility.FromJsonOverwrite(json, p);
			p.Deserialize();
			return p;
		}
	}

	public class PresetSelection
	{
		public List<string> names = new List<string>();
		public List<bool> enables = new List<bool>();
		public bool map_enabled = true;

		public void Add(string name, bool enabled) {
			names.Add(name);
			enables.Add(enabled);
		}

		public void Remove(string name) {
			var i = names.IndexOf(name);
			names.RemoveAt(i);
			enables.RemoveAt(i);
		}

		public int Count => names.Count;

		public void Left(int i) {
			var tmp_name = names[i];
			names[i] = names[i - 1];
			names[i - 1] = tmp_name;

			var tmp_en = enables[i];
			enables[i] = enables[i - 1];
			enables[i - 1] = tmp_en;
		}

		public void Right(int i) {
			var tmp_name = names[i];
			names[i] = names[i + 1];
			names[i + 1] = tmp_name;

			var tmp_en = enables[i];
			enables[i] = enables[i + 1];
			enables[i + 1] = tmp_en;
		}
	}
}
