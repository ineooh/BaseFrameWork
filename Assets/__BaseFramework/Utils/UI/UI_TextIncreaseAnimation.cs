using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextIncreaseAnimation : MonoBehaviour {

	public Text txt;
	public bool timeFormat = false;
	public bool CommasFormat = false;
	public string prefix = string.Empty;
	public string suffix = string.Empty;

	float value_display = 0;
	float value_cur = 0;

	private void OnDrawGizmosSelected()
	{
		txt = GetComponent<Text>();
	}
	// Use this for initialization
	public void SetDisplay(float value)
	{
		value_display = (int)value;
	}
	public void Set (float value)
	{
		value_cur = (int)value;
	}
	public void SetUnKnown()
	{
		value_cur = value_display = 0;
		txt.text = string.Empty;
	}

	// Update is called once per frame
	void Update () 
	{
		if (value_cur != value_display)
		{
			value_display = Mathf.Lerp((float)value_display, (float)value_cur, 0.2f);
			if (Mathf.Abs(value_cur - value_display) <= 1)
				value_display = value_cur;

			string str = string.Empty;
			if (timeFormat)
				str = MyUtils.ConvertSecondToString((int)value_display, TIME_FORMAT.SHORT);
			else if (CommasFormat)
				str = MyUtils.convertNumberToStringWithCommas((int)value_display);
			else
				str =  ((int) value_display).ToString();

			txt.text = prefix + str + suffix ;
		}
	}
}
