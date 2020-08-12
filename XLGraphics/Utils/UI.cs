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
			// get presets in sorted order
			var presets = PresetManager.Instance.presets.OrderByDescending(p => p.volume.priority).ToList();
			var rectTransform = presetsListContent.GetComponent<RectTransform>();

			presetGOs = new Dictionary<Preset, GameObject>();
			for (int i = 0; i < presets.Count; i++) {
				var preset = presets[i];
				var presetGO = GameObject.Instantiate(Main.uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Preset.prefab"), presetsListContent.transform);
				presetGOs[preset] = presetGO;

				presetGO.GetComponentsInChildren<Text>().First(t => t.name == "PresetNameLabel").text = preset.name;
				presetGO.GetComponentInChildren<XLToggle>().OverrideValue(Main.settings.presetOrder.IsEnabled(preset.name));

				if (i == 0) {
					var upBTN = presetGO.GetComponentsInChildren<Button>().First(b => b.name == "PresetUpButton");
					upBTN.interactable = false;
				}
				else if (i == presets.Count - 1) {
					var downBTN = presetGO.GetComponentsInChildren<Button>().First(b => b.name == "PresetDownButton");
					downBTN.interactable = false;
				}
			}
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

			// new preset
			buttons["NewPresetButton"].Click += () => {
				// create new preset with name
				PresetManager.Instance.CreateNewPreset();

				// recreate preset list
				RebuildPresetList();

				// edit it
				OnEditPreset(PresetManager.Instance.selectedPreset);
			};
		}

		public void AddPresetListeners() {
			var presets = PresetManager.Instance.presets;
			foreach (var preset in presets) {
				var presetGO = presetGOs[preset];
				var editBTN = presetGO.GetComponentsInChildren<XLButton>().First(b => b.name == "PresetEditButton");
				editBTN.Click += () => OnEditPreset(preset);

				var toggle = presetGO.GetComponentsInChildren<XLToggle>().First(b => b.name == "PresetEnableToggle");
				toggle.ValueChanged += value => {
					PresetManager.Instance.currentPresetOrder.SetEnabled(preset.name, value);
					preset.volume.gameObject.SetActive(value);
				};

				var deleteBTN = presetGO.GetComponentsInChildren<XLButton>().First(b => b.name == "PresetDeleteButton");
				deleteBTN.Click += () => {
					PresetManager.Instance.DeletePreset(preset);
					RebuildPresetList();
					XLGraphicsMenu.Instance.presetsContent.SetActive(true);
				};

				var upBTN = presetGO.GetComponentsInChildren<XLButton>().First(b => b.name == "PresetUpButton");
				upBTN.Click += () => {
					PresetManager.Instance.UpgradePriority(preset);
					RebuildPresetList();
					XLGraphicsMenu.Instance.presetsContent.SetActive(true);
				};

				var downBTN = presetGO.GetComponentsInChildren<XLButton>().First(b => b.name == "PresetDownButton");
				downBTN.Click += () => {
					PresetManager.Instance.DowngradePriority(preset);
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

		public bool IsFocusedInput() {
			var inputField = XLGraphicsMenu.Instance.renamePresetInputField;
			return inputField.activeSelf && inputField.GetComponent<InputField>().isFocused;
		}
	}
}
