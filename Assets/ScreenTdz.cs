using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTdz : MonoBehaviour
{
	[SerializeField]RectTransform rect = null;
	[SerializeField]RectTransform rectBox = null;
	[SerializeField]RectTransform rectHud = null;

	public void SetLimit (float rangeX)
	{
		float ratio = rectHud.localScale.x;
		Vector2 size = rectHud.sizeDelta;
		rect.sizeDelta = size * ratio * rangeX;
		rectBox.sizeDelta = new Vector2 (rect.sizeDelta.x - 6, 6);

	}
}
