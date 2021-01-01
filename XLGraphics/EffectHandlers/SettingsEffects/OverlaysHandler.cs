using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.EffectHandlers.SettingsEffects
{
	class OverlaysHandler : EffectHandler
	{
		private List<ImgToggle> imgs;

		public override void ConnectUI() {
			// load images
			// if no Overlays folder, create it
			if (!Directory.Exists(Main.modEntry.Path + "Overlays")) {
				Directory.CreateDirectory(Main.modEntry.Path + "Overlays");
			}

			// load pngs from files
			imgs = new List<ImgToggle>();
			var filePaths = Directory.GetFiles(Main.modEntry.Path + "Overlays\\", "*.png");
			foreach (var filePath in filePaths) {
				try {
					Main.Logger.Log($"Reading overlay: {filePath}");
					var fileData = File.ReadAllBytes(filePath);
					var tex2D = new Texture2D(2, 2);
					tex2D.LoadImage(fileData);
					//var sprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0, 0));

					var imgToggle = new ImgToggle();
					imgToggle.imgName = Path.GetFileNameWithoutExtension(filePath);
					imgToggle.active = Main.settings.settingsData.activeOverlays.Contains(imgToggle.imgName);
					imgToggle.texture = tex2D;
					imgs.Add(imgToggle);
				}
				catch (Exception e) {
					Main.Logger.Log($"ex: {e}");
				}
			}

			// update active overlays to account for possibly deleted ones
			updateActiveOverlaysSettings();

			// create overlays
			var imgContainer = Main.menu.imgContainer;
			var imgOverlayAsset = Main.overlayImageObjectAsset;
			foreach (var img in imgs) {
				img.imgUI = GameObject.Instantiate(imgOverlayAsset, imgContainer.transform).GetComponent<RawImage>();
				img.imgUI.texture = img.texture;
				img.imgUI.gameObject.SetActive(img.active);
			}

			// create UI
			var imgListContent = Main.menu.imgNameListContent;
			var imgNameAsset = Main.imgNameObjectAsset;
			foreach (var img in imgs) {
				img.imgNameUI = GameObject.Instantiate(imgNameAsset, imgListContent.transform).GetComponent<OverlayImgName>();
				img.imgNameUI.imgName.text = img.imgName;
				img.imgNameUI.toggle.SetIsOnWithoutNotify(img.active);
			}

			// create Listeners
			foreach (var img in imgs) {
				img.imgNameUI.toggle.onValueChanged += value => {
					img.active = value;
					img.imgUI.gameObject.SetActive(value);
					updateActiveOverlaysSettings();
				};
			}
		}

		private void updateActiveOverlaysSettings() {
			Main.settings.settingsData.activeOverlays = imgs.Where(img => img.active).Select(img => img.imgName).ToList();
		}

		private class ImgToggle {
			public string imgName;
			public bool active;
			public Texture2D texture;
			public OverlayImgName imgNameUI;
			public RawImage imgUI;
		}
	}
}
