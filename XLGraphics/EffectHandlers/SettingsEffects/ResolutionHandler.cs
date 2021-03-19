using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using XLGraphics.Utils;
using XLGraphicsUI.Elements.SettingsUI;

namespace XLGraphics.EffectHandlers.SettingsEffects
{
	class ResolutionHandler : EffectHandler
	{
		ResolutionUI resUI;
		List<RefreshLessResolution> availableResolutions;


		public override void ConnectUI() {
			resUI = ResolutionUI.Instance;

			// set gameplay res
			var res = Main.settings.settingsData.gameplayResolution;
			Screen.SetResolution(res.width, res.height, Main.settings.settingsData.SCREEN_MODE);

			// init UI values
			GetResolutions();

			// add listeners
			resUI.dropdown.onValueChanged.AddListener(new UnityAction<int>(value => {
				var res = availableResolutions[value];
				Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);

				// save res
				if (XLGraphics.Instance.IsReplayActive()) {
					Main.settings.settingsData.replayResolution = res;
				}
				else {
					Main.settings.settingsData.gameplayResolution = res;
				}
			}));

			XLGraphics.Instance.OnReplayStateChange += () => {
				var res = XLGraphics.Instance.IsReplayActive() ? Main.settings.settingsData.replayResolution : Main.settings.settingsData.gameplayResolution;
				if (!this.availableResolutions.Contains(res)) {
					this.GetResolutions();
				}
				resUI.dropdown.SetValueWithoutNotify(this.availableResolutions.IndexOf(res));
				Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
			};
		}

		private void GetResolutions() {
			var resolutions = Screen.resolutions.ToList();
			int i = 1;
			while (i < resolutions.Count) {
				if (resolutions[i - 1].width == resolutions[i].width && resolutions[i - 1].height == resolutions[i].height) {
					if (resolutions[i].refreshRate > resolutions[i - 1].refreshRate) {
						resolutions.RemoveAt(i - 1);
					}
					else {
						resolutions.RemoveAt(i);
					}
				}
				else {
					i++;
				}
			}

			this.availableResolutions = resolutions.Select(r => new RefreshLessResolution(r)).ToList();

			Main.Logger.Log("Resolutions: " + this.availableResolutions.Join(r => r.ToString(), "   "));
			resUI.dropdown.options = this.availableResolutions.Select(r => new TMPro.TMP_Dropdown.OptionData(r.width + " x " + r.height)).ToList();
			resUI.dropdown.SetValueWithoutNotify(this.availableResolutions.IndexOf(new RefreshLessResolution(Screen.currentResolution)));
		}
	}

	public class RefreshLessResolution : IEquatable<RefreshLessResolution>
	{
		public int width;
		public int height;

		public RefreshLessResolution(Resolution res) {
			width = res.width;
			height = res.height;
		}

		public override bool Equals(object obj) {
			return this.Equals(obj as RefreshLessResolution);
		}

		public bool Equals(RefreshLessResolution other) {
			return other != null &&
				   this.width == other.width &&
				   this.height == other.height;
		}

		public override int GetHashCode() {
			int hashCode = 1263118649;
			hashCode = hashCode * -1521134295 + this.width.GetHashCode();
			hashCode = hashCode * -1521134295 + this.height.GetHashCode();
			return hashCode;
		}

		public override string ToString() {
			return $"{width}x{height}";
		}

		public static bool operator ==(RefreshLessResolution left, RefreshLessResolution right) {
			return EqualityComparer<RefreshLessResolution>.Default.Equals(left, right);
		}

		public static bool operator !=(RefreshLessResolution left, RefreshLessResolution right) {
			return !(left == right);
		}
	}
}
