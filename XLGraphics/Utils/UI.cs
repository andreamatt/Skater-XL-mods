using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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

		public void CollectElements() {
			var menu = XLGraphicsMenu.Instance;
			// activate and deactivate
			menu.basicContent.SetActive(true);
			menu.presetsContent.SetActive(true);
			menu.cameraContent.SetActive(true);
			menu.editPresetPanel.SetActive(true);

			presetsListContent = Main.menu.presetsListContent;
			if (firstBuild) {
				firstBuild = false;
				RemoveTestPresets();
				DisableNavigation();
			}
			PopulatePresetsList();

			menu.basicContent.SetActive(false);
			menu.presetsContent.SetActive(false);
			menu.cameraContent.SetActive(false);
			menu.editPresetPanel.SetActive(false);
		}

		private void RemoveTestPresets() {
			var presetUIs = presetsListContent.GetComponentsInChildren<XLPreset>();
			foreach (var presetUI in presetUIs) {
				GameObject.DestroyImmediate(presetUI.gameObject);
			}
		}

		public void PopulatePresetsList() {
			// get presets in sorted order
			var presets = PresetManager.Instance.presets.OrderByDescending(p => p.volume.priority).ToList();
			var rectTransform = presetsListContent.GetComponent<RectTransform>();

			for (int i = 0; i < presets.Count; i++) {
				var preset = presets[i];
				var go = GameObject.Instantiate(Main.uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Preset.prefab"), presetsListContent.transform);
				var presetUI = go.GetComponent<XLPreset>();
				preset.presetUI = presetUI;
				//var rect = presetUI.GetComponent<RectTransform>().rect;
				//presetUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, presetUIheight);
				//presetUI.GetComponent<RectTransform>().ForceUpdateRectTransforms();

				presetUI.presetNameLabel.text = preset.name;
				presetUI.presetToggle.isOn = Main.settings.presetOrder.IsEnabled(preset.name);

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

			var inputField = XLGraphicsMenu.Instance.renamePresetInputField;
			inputField.text = preset.name;
			inputField.GetComponentsInChildren<TMP_Text>().First(t => t.name == "Placeholder").text = preset.name;
			inputField.GetComponentsInChildren<TMP_Text>().First(t => t.name == "Text").text = preset.name;
		}

		public void AddBaseListeners() {
			// presets editing
			var menu = XLGraphicsMenu.Instance;
			menu.savePresetButton.onClick.AddListener(new UnityAction(() => {
				var inputField = XLGraphicsMenu.Instance.renamePresetInputField;
				var text = inputField.text;
				if (text != PresetManager.Instance.selectedPreset.name) {
					PresetManager.Instance.RenamePreset();
				}
				PresetManager.Instance.SavePreset(PresetManager.Instance.selectedPreset);
				RebuildPresetList();
				XLGraphicsMenu.Instance.presetsContent.SetActive(true);
			}));

			menu.renamePresetButton.onClick.AddListener(new UnityAction(() => {
				//RebuildPresetList();
				//XLGraphicsMenu.Instance.editPresetPanel.SetActive(true);
				PresetManager.Instance.RenamePreset();
			}));

			// new preset
			menu.newPresetButton.onClick.AddListener(new UnityAction(() => {
				// create new preset with name
				PresetManager.Instance.CreateNewPreset();

				// edit it
				OnEditPreset(PresetManager.Instance.selectedPreset);
			}));
		}

		public void AddPresetListeners() {
			var presets = PresetManager.Instance.presets;
			foreach (var preset in presets) {
				UnityAction editClick = () => OnEditPreset(preset);
				preset.presetUI.presetEditButton.onClick.AddListener(editClick);

				UnityAction<bool> toggleChange = value => {
					PresetManager.Instance.currentPresetOrder.SetEnabled(preset.name, value);
					preset.volume.gameObject.SetActive(value);
				};
				preset.presetUI.presetToggle.onValueChanged.AddListener(toggleChange);

				UnityAction deleteClick = () => {
					PresetManager.Instance.DeletePreset(preset);
					RebuildPresetList();
					XLGraphicsMenu.Instance.presetsContent.SetActive(true);
				};
				preset.presetUI.presetDeleteButton.onClick.AddListener(deleteClick);

				UnityAction upClick = () => {
					PresetManager.Instance.UpgradePriority(preset);
					RebuildPresetList();
					XLGraphicsMenu.Instance.presetsContent.SetActive(true);
				};
				preset.presetUI.presetUpButton.onClick.AddListener(upClick);

				UnityAction downClick = () => {
					PresetManager.Instance.DowngradePriority(preset);
					RebuildPresetList();
					XLGraphicsMenu.Instance.presetsContent.SetActive(true);
				};
				preset.presetUI.presetDownButton.onClick.AddListener(downClick);
			}
		}

		public void RebuildPresetList() {
			// destroy old
			var presets = PresetManager.Instance.presets;
			foreach (var preset in presets) {
				if (preset.presetUI != null) {
					GameObject.DestroyImmediate(preset.presetUI.gameObject);
				}
			}

			// build new
			CollectElements();

			// listeners
			AddPresetListeners();
		}

		public bool IsFocusedInput() {
			var inputField = XLGraphicsMenu.Instance.renamePresetInputField.gameObject;
			return inputField.activeSelf && inputField.GetComponent<TMP_InputField>().isFocused;
		}
	}
}
