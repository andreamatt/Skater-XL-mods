using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using XLGraphics.CustomEffects;
using XLGraphics.Presets;
using XLGraphicsUI.Elements.EffectsUI;
using static UnityEngine.Camera;

namespace XLGraphics.EffectHandlers.PresetEffects
{
	public class PhysicalCameraHandler : PresetEffectHandler
	{
		PhysicalCameraUI pcUI;

		public override void ConnectUI() {
			pcUI = PhysicalCameraUI.Instance;

			pcUI.toggle.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.active = v;
			pcUI.usePhysicalProperties.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.usePhysicalProperties = v;
			pcUI.sensorType.onValueChanged.AddListener(v => {
				var type = (SensorType)v;
				PresetManager.Instance.selectedPreset.physicalCameraData.sensorType = type;
				UpdateActiveSliders(type);
			});
			pcUI.sensorSize.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.sensorSize = v;
			pcUI.iso.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.iso = (int)v;
			pcUI.shutterSpeed.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.shutterSpeed = v;
			pcUI.gateFit.onValueChanged.AddListener(v => PresetManager.Instance.selectedPreset.physicalCameraData.gateFit = (GateFitMode)v);
			pcUI.focalLength.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.focalLength = v;
			pcUI.aperture.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.aperture = v;
			pcUI.bladeCount.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.bladeCount = (int)v;
			pcUI.anamorphism.onValueChanged += v => PresetManager.Instance.selectedPreset.physicalCameraData.anamorphism = v;
		}

		public override void OnChangeSelectedPreset(Preset preset) {
			var pc = preset.physicalCameraData;
			pcUI.toggle.SetIsOnWithoutNotify(pc.active);
			pcUI.usePhysicalProperties.SetIsOnWithoutNotify(pc.usePhysicalProperties);
			pcUI.sensorSize.OverrideValue(pc.sensorSize);
			pcUI.iso.OverrideValue(pc.iso);
			pcUI.shutterSpeed.OverrideValue(pc.shutterSpeed);
			pcUI.gateFit.SetValueWithoutNotify((int)pc.gateFit);
			pcUI.focalLength.OverrideValue(pc.focalLength);
			pcUI.aperture.OverrideValue(pc.aperture);
			pcUI.bladeCount.OverrideValue(pc.bladeCount);
			pcUI.anamorphism.OverrideValue(pc.anamorphism);

			UpdateActiveSliders(pc.sensorType);
		}

		private void UpdateActiveSliders(SensorType type) {
			if (type == SensorType.Custom) {
				pcUI.sensorSize.sliderX.slider.interactable = true;
				pcUI.sensorSize.sliderY.slider.interactable = true;
				pcUI.sensorSize.sliderX.inputField.interactable = true;
				pcUI.sensorSize.sliderY.inputField.interactable = true;
			}
			else {
				pcUI.sensorSize.OverrideValue(CustomPhysicalCameraController.sensorTypeToSize[(int)type]);
				pcUI.sensorSize.sliderX.slider.interactable = false;
				pcUI.sensorSize.sliderY.slider.interactable = false;
				pcUI.sensorSize.sliderX.inputField.interactable = false;
				pcUI.sensorSize.sliderY.inputField.interactable = false;
			}
		}
	}

	public enum SensorType
	{
		_8mm,
		Super8mm,
		_16mm,
		Super16mm,
		_35mm_2perf,
		_35mm_Academy,
		Super35mm,
		_65mm_ALEXA,
		_70mm,
		_70mm_IMAX,
		Custom
	}
}
