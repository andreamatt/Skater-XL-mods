using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XLGraphicsUI
{
	public class XLGraphicsMenu : UIsingleton<XLGraphicsMenu>
	{
		//public GameObject tabPanel;
		public GameObject basicContent;
		public GameObject presetsContent;
		public GameObject cameraContent;
		public GameObject presetsListContent;
		public GameObject editPresetPanel;
		public TMP_InputField renamePresetInputField;
		//public Button renamePresetButton;
		public Button savePresetButton;
		public Button newPresetButton;
		public EventSystem eventSystem;
	}
}
