using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

		private GameObject presetsListContent;
		private bool firstBuild = true;

		public void DisableNavigation() {
			var nav = XLGraphicsMenu.Instance.GetComponentsInChildren<Selectable>();
			foreach (var n in nav) {
				n.navigation = new Navigation() {
					mode = Navigation.Mode.None
				};
			}
		}

		public void CollectElements(bool keepActive) {
			var menu = XLGraphicsMenu.Instance;
			var bcActive = menu.basicContent.activeSelf;
			var pcActive = menu.presetsContent.activeSelf;
			var ccActive = menu.cameraContent.activeSelf;
			var eppActive = menu.editPresetPanel.activeSelf;

			// activate and deactivate
			menu.basicContent.SetActive(true);
			menu.presetsContent.SetActive(true);
			menu.cameraContent.SetActive(true);
			menu.editPresetPanel.SetActive(true);

			presetsListContent = Main.menu.presetsListContent;
			if (firstBuild) {
				firstBuild = false;
				RemoveTestPresets();
			}
			PopulatePresetsList();
			//DisableNavigation();

			menu.basicContent.SetActive(false);
			menu.presetsContent.SetActive(false);
			menu.cameraContent.SetActive(false);
			menu.editPresetPanel.SetActive(false);

			if (keepActive) {
				if (bcActive) menu.basicContent.SetActive(true);
				if (pcActive) menu.presetsContent.SetActive(true);
				if (ccActive) menu.cameraContent.SetActive(true);
				if (eppActive) menu.editPresetPanel.SetActive(true);
			}
		}

		private void RemoveTestPresets() {
			var presetUIs = presetsListContent.GetComponentsInChildren<XLPreset>();
			foreach (var presetUI in presetUIs) {
				GameObject.DestroyImmediate(presetUI.gameObject);
			}
		}

		public void PopulatePresetsList() {
			// get presets in sorted order
			var presets = PresetManager.Instance.presets;
			var rectTransform = presetsListContent.GetComponent<RectTransform>();

			for (int i = 0; i < presets.Count; i++) {
				var preset = presets[i];
				var go = GameObject.Instantiate(Main.presetObjectAsset, presetsListContent.transform);
				var presetUI = go.GetComponent<XLPreset>();
				preset.presetUI = presetUI;

				presetUI.presetNameLabel.text = preset.name;
				var isOn = PresetManager.Instance.currentPresetOrder.IsEnabled(preset.name);
				presetUI.presetToggle.SetIsOnWithoutNotify(isOn);

				if (i == 0) {
					var upBTN = presetUI.presetUpButton;
					upBTN.interactable = false;
				}
				if (i == presets.Count - 1) {
					var downBTN = presetUI.presetDownButton;
					downBTN.interactable = false;
				}
			}
		}

		public void OnEditPreset(Preset preset) {
			PresetManager.Instance.selectedPreset = preset;
			foreach (var eH in XLGraphics.Instance.presetEffectHandlers) {
				eH.OnChangeSelectedPreset(preset);
			}
			XLGraphicsMenu.Instance.presetsContent.SetActive(false);
			XLGraphicsMenu.Instance.editPresetPanel.SetActive(true);

			var inputField = XLGraphicsMenu.Instance.renamePresetInputField;
			inputField.text = preset.name;
			inputField.GetComponentsInChildren<TMP_Text>().First(t => t.name == "Placeholder").text = preset.name;
			inputField.GetComponentsInChildren<TMP_Text>().First(t => t.name == "Text").text = preset.name;
		}

		public void AddBaseListeners() {
			// presets editing
			var menu = XLGraphicsMenu.Instance;

			var savePreset = new UnityAction(() => {
				var inputField = XLGraphicsMenu.Instance.renamePresetInputField;
				var text = inputField.text;
				if (text != PresetManager.Instance.selectedPreset.name) {
					PresetManager.Instance.RenamePreset();
				}
				PresetManager.Instance.SavePreset(PresetManager.Instance.selectedPreset);
				RebuildPresetList(false);
				XLGraphicsMenu.Instance.presetsContent.SetActive(true);
			});
			menu.savePresetButton.onClick.AddListener(savePreset);

			// removed rename preset button
			//menu.renamePresetButton.onClick.AddListener(new UnityAction(() => {
			//	PresetManager.Instance.RenamePreset();
			//}));

			// new preset
			menu.newPresetButton.onClick.AddListener(new UnityAction(() => {
				// create new preset with name
				PresetManager.Instance.CreateNewPreset();

				// edit it
				OnEditPreset(PresetManager.Instance.selectedPreset);
			}));

			// when replay state changes
			XLGraphics.Instance.onReplayStateChange += () => {
				PresetManager.Instance.SetActives();
				PresetManager.Instance.SetPriorities();
				RebuildPresetList(true);
			};
		}

		public void AddPresetListeners() {
			var presets = PresetManager.Instance.presets;
			foreach (var preset in presets) {
				UnityAction editClick = () => OnEditPreset(preset);
				preset.presetUI.presetEditButton.onClick.AddListener(editClick);

				UnityAction<bool> toggleChange = value => {
					PresetManager.Instance.currentPresetOrder.SetEnabled(preset.name, value);
					preset.enabled = value;
				};
				preset.presetUI.presetToggle.onValueChanged.AddListener(toggleChange);

				UnityAction deleteClick = () => {
					PresetManager.Instance.DeletePreset(preset);
					RebuildPresetList(true);
				};
				preset.presetUI.presetDeleteButton.onClick.AddListener(deleteClick);

				UnityAction upClick = () => {
					PresetManager.Instance.UpgradePriority(preset);
					RebuildPresetList(true);
				};
				preset.presetUI.presetUpButton.onClick.AddListener(upClick);

				UnityAction downClick = () => {
					PresetManager.Instance.DowngradePriority(preset);
					RebuildPresetList(true);
				};
				preset.presetUI.presetDownButton.onClick.AddListener(downClick);
			}
		}

		public void RebuildPresetList(bool keepActive) {
			// destroy old
			var presetUIs = presetsListContent.GetComponentsInChildren<XLPreset>();
			foreach (var presetUI in presetUIs) {
				GameObject.DestroyImmediate(presetUI.gameObject);
			}

			// build new
			CollectElements(keepActive);

			// listeners
			AddPresetListeners();
		}

		//public bool IsFocusedInput() {
		//	var eventSystem = XLGraphicsMenu.Instance.gameObject.GetComponent<EventSystem>();
		//	eventSystem.isFocused

		//	var inputField = XLGraphicsMenu.Instance.renamePresetInputField;
		//	return inputField.gameObject.activeSelf && inputField.isFocused;
		//}
	}
}
