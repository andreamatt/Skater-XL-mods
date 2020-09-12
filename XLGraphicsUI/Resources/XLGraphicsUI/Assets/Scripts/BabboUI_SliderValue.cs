using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BabboUI_SliderValue : MonoBehaviour
{
	public Slider slider;
	public TextMeshProUGUI text;
	public TMP_InputField input_value;
	private string value_text;
	private float value_out;

	public void Slider_Changed(float Slider_Value)
	{
		Slider_Value = Mathf.Round(Slider_Value * 100f) / 100f;
		// Debug.Log("Slider Value" + Slider_Value);

		string value_text = Slider_Value.ToString();

		text.SetText(value_text);
	}
	public void Input_Changed()
	{
		if (float.TryParse(input_value.text, out value_out))
		{
			slider.value = value_out;
		}
		else
		{
			Debug.Log("Invalid Value");
		}
	}
}
