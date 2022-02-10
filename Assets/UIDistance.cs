using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDistance : MonoBehaviour
{
	[SerializeField]float fRange = 400;
	[SerializeField]RectTransform rectCursor = null;
	public void Change (float ratio)
	{
		float xNew = fRange * ratio;
		rectCursor.transform.localPosition = new Vector3 (xNew, 0, 0);

	}
}
