using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour,HPBar
{
	[SerializeField]Image imgFill = null;
	float r = 0;

	public void SetColor (Color32 color)
	{
		color.a = 255;
		imgFill.color = color;
	}

	public void Change (float ratio = 0)
	{
		r = ratio;
		imgFill.fillAmount = ratio;
	}

	public void Disable ()
	{
		
	}

	public void Active (bool b)
	{
		transform.GetChild (0).gameObject.SetActive (b);
		GetComponent<UIAnimation> ().enabled = b;
	}

	public float Ratio ()
	{
		return r;
	}
}
