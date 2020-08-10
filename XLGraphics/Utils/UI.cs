using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLGraphics.Presets;
using XLGraphicsUI;
using XLGraphicsUI.Elements;

namespace XLGraphics.Utils
{
	public class UI
	{
		static public UI Instance { get; private set; }

		public UI() {
			if (Instance != null) {
				throw new Exception("Cannot have multiple instances");
			}
			Instance = this;
		}

		public Dictionary<string, XLSlider> sliders;
		public Dictionary<string, XLButton> buttons;
		public Dictionary<string, XLToggle> toggles;
		public Dictionary<string, XLSelectionGrid> selectionGrids;
		public Dictionary<string, XLSliderValueText> sliderValueTexts;

		private GameObject presetsListContent;
		public Dictionary<Preset, GameObject> presetGOs;

		public void CollectElements() {
			var menu = XLGraphicsMenu.Instance;
			// activate and deactivate
			menu.basicContent.SetActive(true);
			menu.presetsContent.SetActive(true);
			menu.cameraContent.SetActive(true);
			menu.editPresetPanel.SetActive(true);

			sliders = XLSlider.xlSliders.ToDictionary(s => s.name);
			sliderValueTexts = XLSliderValueText.xlSliderValueTexts.ToDictionary(s => s.name);
			buttons = XLButton.xlButtons.ToDictionary(s => s.name);
			toggles = XLToggle.xlToggles.ToDictionary(s => s.name);
			selectionGrids = XLSelectionGrid.xlSelectionGrids.ToDictionary(s => s.name);

			presetsListContent = Main.menu.presetsListContent;

			PopulatePresetsList();

			menu.basicContent.SetActive(false);
			menu.presetsContent.SetActive(false);
			menu.cameraContent.SetActive(false);
			menu.editPresetPanel.SetActive(false);
		}

		public void PopulatePresetsList() {
			var presets = PresetManager.Instance.presets;
			var rectTransform = presetsListContent.GetComponent<RectTransform>();
			var rect = rectTransform.rect;
			var spacing = presetsListContent.GetComponent<VerticalLayoutGroup>().spacing;

			var presetHeight = 0f;
			presetGOs = new Dictionary<Preset, GameObject>();
			foreach (var preset in presets) {
				var presetGO = GameObject.Instantiate(Main.uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Preset.prefab"), presetsListContent.transform);
				presetHeight = presetGO.GetComponent<RectTransform>().rect.height;
				presetGOs[preset] = presetGO;

				presetGO.GetComponentsInChildren<Text>().First(t => t.name == "PresetNameLabel").text = preset.name;
				presetGO.GetComponentInChildren<XLToggle>().OverrideValue(Main.settings.presetOrder.IsEnabled(preset.name));
			}

			rect.height = presetHeight * presets.Count + spacing * (presets.Count - 1);
		}

		public void TogglePresets() {
			// change ui preset state based on replay mode being on/off
		}

		public void OnEditPreset(Preset preset) {
			PresetManager.Instance.selectedPreset = preset;
			foreach (var eH in XLGraphics.Instance.presetEffectHandlers) {
				eH.OnChangeSelectedPreset(preset);
			}
			XLGraphicsMenu.Instance.presetsContent.SetActive(false);
			XLGraphicsMenu.Instance.editPresetPanel.SetActive(true);

			XLGraphicsMenu.Instance.renamePresetInputField.GetComponentsInChildren<Text>().First(t => t.name == "Placeholder").text = preset.name;
		}

		public void OnSavePreset() {
			XLGraphicsMenu.Instance.presetsContent.SetActive(true);
			XLGraphicsMenu.Instance.editPresetPanel.SetActive(false);
			PresetManager.Instance.SavePreset(PresetManager.Instance.selectedPreset);
		}

		public void AddBaseListeners() {
			// presets editing
			buttons["SavePresetButton"].Click += () => OnSavePreset();
			buttons["RenamePresetButton"].Click += () => {
				PresetManager.Instance.RenamePreset();
				RebuildPresetList();
				XLGraphicsMenu.Instance.editPresetPanel.SetActive(true);
			};

			// sliders values
			foreach (var slider in sliders.Values) {
				var sliderValueName = slider.name.Replace("Slider", "Value");
				slider.ValueChanged += v => sliderValueTexts[sliderValueName].OverrideValue(v);
			}
		}

		public void AddPresetListeners() {
			var presets = PresetManager.Instance.presets;
			foreach (var preset in presets) {
				var editBTN = presetGOs[preset].GetComponentsInChildren<XLButton>().First(b => b.name == "PresetEditButton");
				editBTN.Click += () => OnEditPreset(preset);

				var toggle = presetGOs[preset].GetComponentsInChildren<XLToggle>().First(b => b.name == "PresetEnableToggle");
				toggle.ValueChanged += value => {
					Main.settings.presetOrder.SetEnabled(preset.name, value);
					preset.volume.gameObject.SetActive(value);
				};

				var deleteBTN = presetGOs[preset].GetComponentsInChildren<XLButton>().First(b => b.name == "PresetDeleteButton");
				deleteBTN.Click += () => {
					PresetManager.Instance.DeletePreset(preset);
					RebuildPresetList();
					XLGraphicsMenu.Instance.presetsContent.SetActive(true);
				};
			}
		}

		public void RebuildPresetList() {
			// destroy old
			var toDestroy = presetGOs.Values.ToList();
			foreach (var presetGO in toDestroy) {
				GameObject.DestroyImmediate(presetGO);
			}
			presetGOs.Clear();

			// build new
			CollectElements();

			// listeners
			AddPresetListeners();
		}
	}
}
