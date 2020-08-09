using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using XLGraphics.Presets;
using XLGraphicsUI.Elements;

namespace XLGraphics.Utils
{
	class UI
	{
		public Dictionary<string, XLSlider> sliders;
		public Dictionary<string, XLButton> buttons;
		public Dictionary<string, XLToggle> toggles;
		public Dictionary<string, XLSelectionGrid> selectionGrids;

		private GameObject presetsListContent;

		public void CollectElements() {
			sliders = UnityEngine.Object.FindObjectsOfType<XLSlider>().ToDictionary(s => s.name);
			buttons = UnityEngine.Object.FindObjectsOfType<XLButton>().ToDictionary(s => s.name);
			toggles = UnityEngine.Object.FindObjectsOfType<XLToggle>().ToDictionary(s => s.name);
			selectionGrids = UnityEngine.Object.FindObjectsOfType<XLSelectionGrid>().ToDictionary(s => s.name);

			presetsListContent = Main.menu.presetsListContent;
		}

		public void PopulatePresetsList(List<Preset> presets) {
			var rectTransform = presetsListContent.GetComponent<RectTransform>();
			var rect = rectTransform.rect;
			var spacing = presetsListContent.GetComponent<VerticalLayoutGroup>().spacing;

			var presetHeight = 0f;
			foreach (var preset in presets) {
				var presetGO = GameObject.Instantiate(Main.uiBundle.LoadAsset<GameObject>("Assets/Prefabs/Preset.prefab"), presetsListContent.transform);
				presetHeight = presetGO.GetComponent<RectTransform>().rect.height;
			}

			rect.height = presetHeight * presets.Count + spacing * (presets.Count - 1);
		}

		public void TogglePresets() {
			// change ui preset state based on replay mode being on/off
		}
	}
}
