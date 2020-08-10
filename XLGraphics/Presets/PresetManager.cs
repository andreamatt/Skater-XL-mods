using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using XLGraphics.Presets.PresetData;
using XLGraphics.Utils;
using XLGraphicsUI;

namespace XLGraphics.Presets
{
	public class PresetManager
	{
		static public PresetManager Instance { get; private set; }

		public PresetManager() {
			if (Instance != null) {
				throw new Exception("Cannot have multiple instances");
			}
			Instance = this;
		}

		public List<Preset> presets;
		public Preset selectedPreset;
		public const string default_name = "DefaultPreset";

		public void LoadPresets() {

			// if no Presets folder, create it
			if (!Directory.Exists(Main.modEntry.Path + "Presets")) {
				Directory.CreateDirectory(Main.modEntry.Path + "Presets");
			}

			// load presets from files
			presets = new List<Preset>();
			string[] filePaths = Directory.GetFiles(Main.modEntry.Path + "Presets\\", "*.preset.json");
			foreach (var filePath in filePaths) {
				try {
					var json = File.ReadAllText(filePath);
					var preset = JsonConvert.DeserializeObject<Preset>(json);

					// avoid adding map preset to presetOrder?
					presets.Add(preset);
					var presetOrder = Main.settings.presetOrder;
					// if new preset detected, add it as disabled
					if (!presetOrder.Names.Contains(preset.name)) {
						presetOrder.Add(preset.name, false);
					}
					presetOrder = Main.settings.replay_presetOrder;
					if (!presetOrder.Names.Contains(preset.name)) {
						presetOrder.Add(preset.name, false);
					}
					Logger.Log("preset: " + preset.name + " loaded");
				}
				catch (Exception e) {
					Logger.Log($"ex: {e}");
				}
			}


			// if there are no presets, add the default one
			if (presets.Count == 0) {
				var default_preset = new Preset(default_name);
				SavePreset(default_preset);
				presets.Add(default_preset);
				if (!Main.settings.presetOrder.Names.Contains(default_preset.name)) {
					Main.settings.presetOrder.Add(default_preset.name, false);
				}
				if (!Main.settings.replay_presetOrder.Names.Contains(default_preset.name)) {
					Main.settings.replay_presetOrder.Add(default_preset.name, false);
				}
			}

			// remove presets that were not found
			foreach (var name in Main.settings.presetOrder.Names) {
				if (!presets.Any(p => p.name == name)) {
					Main.settings.presetOrder.Remove(name);
				}
			}
			foreach (var name in Main.settings.replay_presetOrder.Names) {
				if (!presets.Any(p => p.name == name)) {
					Main.settings.replay_presetOrder.Remove(name);
				}
			}


			// add volume components to all presets
			var highestPrior = VolumeUtils.Instance.GetHighestPriority();
			Logger.Log("highest prio is: " + highestPrior);

			for (int i = 0; i < presets.Count; i++) {
				var preset = presets[i];
				var volume = VolumeUtils.Instance.CreateVolume(highestPrior + i + 1);
				preset.volume = volume;
				var profile = ScriptableObject.CreateInstance<VolumeProfile>();
				volume.profile = profile;
				GameObject.DontDestroyOnLoad(volume);
				GameObject.DontDestroyOnLoad(profile);

				// add volume components
				preset.bloom = profile.Add<Bloom>();
				preset.channelMixer = profile.Add<ChannelMixer>();
				preset.chromaticAberration = profile.Add<ChromaticAberration>();
				preset.colorAdjustments = profile.Add<ColorAdjustments>();
				preset.colorCurves = profile.Add<ColorCurves>();
				preset.depthOfField = profile.Add<DepthOfField>();
				preset.filmGrain = profile.Add<FilmGrain>();
				preset.lensDistortion = profile.Add<LensDistortion>();
				preset.liftGammaGain = profile.Add<LiftGammaGain>();
				preset.motionBlur = profile.Add<MotionBlur>();
				preset.paniniProjection = profile.Add<PaniniProjection>();
				preset.shadowsMidtonesHighlights = profile.Add<ShadowsMidtonesHighlights>();
				preset.splitToning = profile.Add<SplitToning>();
				preset.tonemapping = profile.Add<Tonemapping>();
				preset.vignette = profile.Add<Vignette>();
				preset.whiteBalance = profile.Add<WhiteBalance>();

				// set all overrides
				preset.bloom.SetAllOverridesTo(true);
				preset.channelMixer.SetAllOverridesTo(true);
				preset.chromaticAberration.SetAllOverridesTo(true);
				preset.colorAdjustments.SetAllOverridesTo(true);
				preset.colorCurves.SetAllOverridesTo(true);
				preset.depthOfField.SetAllOverridesTo(true);
				preset.filmGrain.SetAllOverridesTo(true);
				preset.lensDistortion.SetAllOverridesTo(true);
				preset.liftGammaGain.SetAllOverridesTo(true);
				preset.motionBlur.SetAllOverridesTo(true);
				preset.paniniProjection.SetAllOverridesTo(true);
				preset.shadowsMidtonesHighlights.SetAllOverridesTo(true);
				preset.splitToning.SetAllOverridesTo(true);
				preset.tonemapping.SetAllOverridesTo(true);
				preset.vignette.SetAllOverridesTo(true);
				preset.whiteBalance.SetAllOverridesTo(true);

				// override values from data
				preset.chromaticAberrationData.OverrideValues(preset);

				// set active
				preset.volume.gameObject.SetActive(Main.settings.presetOrder.IsEnabled(preset.name));
			}
		}

		public void SaveAllPresets() {
			foreach (var preset in presets) {
				SavePreset(preset);
			}
		}

		public void SavePreset(Preset p) {
			// serialize data
			p.chromaticAberrationData = ChromaticAberrationData.FromPreset(p);

			// write to file
			var filepath = Main.modEntry.Path + "Presets\\";
			try {
				using (var writer = new StreamWriter($"{filepath}{p.name}.preset.json")) {
					//var serializedLine = JsonUtility.ToJson(p, true);
					var serializedLine = JsonConvert.SerializeObject(p, Formatting.Indented);
					writer.Write(serializedLine);
				}
			}
			catch (Exception e) {
				Logger.Log($"Can't save {filepath}{p.name} preset. ex: {e}");
			}
		}

		public void RenamePreset() {
			var newName = XLGraphicsMenu.Instance.renamePresetInputField.GetComponent<InputField>().text;
			if (newName != selectedPreset.name) {
				if (presets.Any(p => p.name == newName)) {
					Debug.LogError("Cant rename as another preset");
					return;
				}
				var oldName = selectedPreset.name;
				selectedPreset.name = newName;

				// delete old file
				var filepath = Main.modEntry.Path + "Presets\\" + oldName + ".preset.json";
				if (File.Exists(filepath)) {
					File.Delete(filepath);
				}

				// replace in presetOrder
				if (Main.settings.presetOrder.Names.Contains(oldName)) {
					var i = Main.settings.presetOrder.names_enables.FindIndex(n_e => n_e.Item1 == oldName);
					Main.settings.presetOrder.names_enables[i] = (newName, Main.settings.presetOrder.Enables[i]);
				}
				if (Main.settings.replay_presetOrder.Names.Contains(oldName)) {
					var i = Main.settings.replay_presetOrder.names_enables.FindIndex(n_e => n_e.Item1 == oldName);
					Main.settings.replay_presetOrder.names_enables[i] = (newName, Main.settings.replay_presetOrder.Enables[i]);
				}

				// save preset
				SavePreset(selectedPreset);
			}
		}

		public void DeletePreset(Preset p) {
			// delete old file
			var filepath = Main.modEntry.Path + "Presets\\" + p.name + ".preset.json";
			if (File.Exists(filepath)) {
				File.Delete(filepath);
			}

			// replace in presetOrder
			if (Main.settings.presetOrder.Names.Contains(p.name)) {
				var i = Main.settings.presetOrder.names_enables.FindIndex(n_e => n_e.Item1 == p.name);
				Main.settings.presetOrder.names_enables.RemoveAt(i);
			}
			if (Main.settings.replay_presetOrder.Names.Contains(p.name)) {
				var i = Main.settings.replay_presetOrder.names_enables.FindIndex(n_e => n_e.Item1 == p.name);
				Main.settings.replay_presetOrder.names_enables.RemoveAt(i);
			}

			// save preset
			presets.Remove(p);
		}
	}
}
