using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAndChild : MonoBehaviour 
{
	Button btn;
	MaskableGraphic[] listChild;
	Color[] listColor;
	bool isInteractable = true;
	// Use this for initialization
	void Start () 
	{
		btn = GetComponent<Button>();
		listChild = GetComponentsInChildren<MaskableGraphic>();
		listColor = new Color[listChild.Length];
		for( int i = 0; i < listChild.Length; i++)
		{
			listColor[i] = listChild[i].color;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(btn != null && isInteractable != btn.interactable)
		{
			Refresh();
		}
		
	}

	void Refresh()
	{
		isInteractable = btn.interactable;
		if (btn.interactable)
		{
			for (int i = 0; i < listChild.Length; i++)
			{
				if( listChild[i].transform != btn.transform)
					listChild[i].color = listColor[i];
			}
		}
		else
		{
			for (int i = 0; i < listChild.Length; i++)
			{
				if (listChild[i].transform != btn.transform)
					listChild[i].color = listColor[i]*btn.colors.disabledColor;
			}
		}

	}
}
