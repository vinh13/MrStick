using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSync : MonoBehaviour
{
	[SerializeField]Image img = null;
	public void Setup (float ratio, Color color)
	{
		float x = ratio * 400F;
		GetComponent<RectTransform> ().localPosition = new Vector2 (x, 0);
		img.color = color;
	}
}
