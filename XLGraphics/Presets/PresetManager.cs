using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using XLGraphics.SerializationData.PresetData;
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
		public PresetSelection currentPresetOrder => XLGraphics.Instance.IsReplayActive() ? Main.settings.replay_presetOrder : Main.settings.presetOrder;
		public Preset selectedPreset;
		public const string default_name = "DefaultPreset";
		public Volume overriderVolume;  // required for custom DoF behaviour and other scripted effects
		private GameObject volumesParent;

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

					// if deserialize fails, delete the file
					if (preset == null) {
						Logger.Log($"Failed deserializing preset with path: {filePath}");
						// maybe dont delete the file?
						continue;
					}

					// avoid adding map preset to presetOrder?
					var match = Regex.Match(filePath, @".*\\(.*).preset.json");
					preset.name = match.Groups[1].Value;
					if (match.Groups.Count != 2) {
						Logger.Log($"Failed matching preset name with regex. File: {filePath}");
						continue;
					}
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
			//if (presets.Count == 0) {
			//	var default_preset = new Preset(default_name);
			//	SavePreset(default_preset);
			//	presets.Add(default_preset);
			//	if (!Main.settings.presetOrder.Names.Contains(default_preset.name)) {
			//		Main.settings.presetOrder.Add(default_preset.name, false);
			//	}
			//	if (!Main.settings.replay_presetOrder.Names.Contains(default_preset.name)) {
			//		Main.settings.replay_presetOrder.Add(default_preset.name, false);
			//	}
			//}

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

			// set volumeParent: used for disabling/enabling all presets without changing their self-active states
			volumesParent = new GameObject("XLGraphics volumes parent");
			volumesParent.transform.SetParent(XLGraphics.Instance.transform);

			// add volume components to all presets
			for (int i = 0; i < presets.Count; i++) {
				var preset = presets[i];
				// create volume with low priority, change it later
				PreparePreset(preset);

				// override values from data
				ReadPresetData(preset);
			}

			// prepare the overrider volume
			overriderVolume = VolumeUtils.Instance.CreateVolume(0);
			overriderVolume.profile = ScriptableObject.CreateInstance<VolumeProfile>();
			overriderVolume.transform.SetParent(volumesParent.transform);

			SetPriorities();
			SetActives();
		}

		private void PreparePreset(Preset preset) {
			var volume = VolumeUtils.Instance.CreateVolume(float.MinValue);
			volume.name = preset.name + " volume";
			preset.volume = volume;
			var profile = ScriptableObject.CreateInstance<VolumeProfile>();
			volume.profile = profile;
			volume.transform.SetParent(volumesParent.transform);
			//GameObject.DontDestroyOnLoad(volume); no need to call this, as it inherits from the parent
			//GameObject.DontDestroyOnLoad(profile);

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

			// add exposure to fix problem???
			var exposure = profile.Add<Exposure>();
			exposure.SetAllOverridesTo(true);
			exposure.active = false;

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
		}

		private void ReadPresetData(Preset preset) {
			preset.bloomData.OverrideValues(preset);
			preset.channelMixerData.OverrideValues(preset);
			preset.chromaticAberrationData.OverrideValues(preset);
			preset.colorAdjustementsData.OverrideValues(preset);
			preset.colorCurvesData.OverrideValues(preset);
			preset.depthOfFieldData.OverrideValues(preset);
			preset.filmGrainData.OverrideValues(preset);
			preset.lensDistortionData.OverrideValues(preset);
			preset.liftGammaGainData.OverrideValues(preset);
			preset.motionBlurData.OverrideValues(preset);
			preset.paniniProjectionData.OverrideValues(preset);
			preset.physicalCameraData.OverrideValues(preset);
			preset.shadowsMidtonesHighlightsData.OverrideValues(preset);
			preset.splitToningData.OverrideValues(preset);
			preset.toneMappingData.OverrideValues(preset);
			preset.vignetteData.OverrideValues(preset);
			preset.whiteBalanceData.OverrideValues(preset);
		}

		public void SaveAllPresets() {
			foreach (var preset in presets) {
				SavePreset(preset);
			}
		}

		public void SetPriorities() {
			// set all priorities to minimum
			presets.ForEach(p => p.volume.priority = float.MinValue);
			overriderVolume.priority = float.MinValue;

			var highestPrior = VolumeUtils.Instance.GetHighestPriority();
			Logger.Log("highest prio is: " + highestPrior);

			// set priorities based on presetOrder
			var orderNames = currentPresetOrder.Names;
			foreach (var preset in presets) {
				// first element (index 0) has highest priority
				// last element has priority 1+highestPrior
				preset.volume.priority = (orderNames.Count - orderNames.IndexOf(preset.name)) + highestPrior;
			}

			// overrider preset has highest priority
			overriderVolume.priority = highestPrior + orderNames.Count + 1;

			SortPresets();
		}

		public void SetActives() {
			foreach (var preset in presets) {
				preset.volume.gameObject.SetActive(currentPresetOrder.IsEnabled(preset.name));
			}
		}

		public void SavePreset(Preset p) {
			// serialize data
			p.bloomData = BloomData.FromPreset(p);
			p.channelMixerData = ChannelMixerData.FromPreset(p);
			p.chromaticAberrationData = ChromaticAberrationData.FromPreset(p);
			p.colorAdjustementsData = ColorAdjustementsData.FromPreset(p);
			p.colorCurvesData = ColorCurvesData.FromPreset(p);
			p.depthOfFieldData = DepthOfFieldData.FromPreset(p);
			p.filmGrainData = FilmGrainData.FromPreset(p);
			p.lensDistortionData = LensDistortionData.FromPreset(p);
			p.liftGammaGainData = LiftGammaGainData.FromPreset(p);
			p.motionBlurData = MotionBlurData.FromPreset(p);
			p.paniniProjectionData = PaniniProjectionData.FromPreset(p);
			p.physicalCameraData = PhysicalCameraData.FromPreset(p);
			p.shadowsMidtonesHighlightsData = ShadowsMidtonesHighlightsData.FromPreset(p);
			p.splitToningData = SplitToningData.FromPreset(p);
			p.toneMappingData = ToneMappingData.FromPreset(p);
			p.vignetteData = VignetteData.FromPreset(p);
			p.whiteBalanceData = WhiteBalanceData.FromPreset(p);

			// write to file
			var filepath = $"{Main.modEntry.Path}Presets\\{p.name}.preset.json";
			try {
				using (var writer = new StreamWriter(filepath)) {
					//var serializedLine = JsonUtility.ToJson(p, true);
					var serializedLine = JsonConvert.SerializeObject(p, Formatting.Indented);
					writer.Write(serializedLine);
				}
			}
			catch (Exception) {
				Logger.Log($"Can't save {filepath}{p.name} preset");
			}
		}

		public void RenamePreset() {
			var newName = XLGraphicsMenu.Instance.renamePresetInputField.text;
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

			presets.Remove(p);

			// remove volume
			GameObject.DestroyImmediate(p.volume.gameObject);
		}

		public void CreateNewPreset() {
			var p = new Preset();
			p.name = $"{default_name} {DateTime.Now.ToString("s").Replace(':', '-')}";
			presets.Add(p);

			PreparePreset(p);
			ReadPresetData(p);

			Main.settings.presetOrder.AddFirst(p.name, true);
			Main.settings.replay_presetOrder.AddFirst(p.name, true);

			SetPriorities();
			SetActives();

			SavePreset(p);

			selectedPreset = p;
		}

		public void UpgradePriority(Preset p) {
			currentPresetOrder.Upgrade(p.name);
			SetPriorities();
		}

		public void DowngradePriority(Preset p) {
			currentPresetOrder.Downgrade(p.name);
			SetPriorities();
		}

		public void SortPresets() {
			presets = presets.OrderByDescending(p => p.volume.priority).ToList();
		}

		public void SetActivePostProcessing(bool active) {
			// idea: when level is loaded, save all volumes' states.
			// set them all to either disabled or their state
			// on map change reload them

			// change xlgraphics' volumes' state
			volumesParent.SetActive(active);
		}
	}
}
