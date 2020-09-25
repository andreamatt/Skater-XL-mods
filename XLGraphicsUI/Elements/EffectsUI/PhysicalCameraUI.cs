using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XLGraphicsUI.Elements.EffectsUI
{
	public class PhysicalCameraUI : UIsingleton<PhysicalCameraUI>
	{
		public XLToggle toggle;
		//public GameObject container;

		public XLToggle usePhysicalProperties;
		public TMP_Dropdown sensorType;
		public XLSliderVector2 sensorSize;
		public XLSlider iso;
		public XLSlider shutterSpeed;
		public TMP_Dropdown gateFit;

		public XLSlider focalLength;
		public XLSlider aperture;

		public XLSlider bladeCount;
		public XLSlider anamorphism;
	}
}
