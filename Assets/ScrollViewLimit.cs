using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewLimit : MonoBehaviour
{
	[SerializeField]Transform rectContainer = null;
	[SerializeField]float min = 0, max = 0;
	[HideInInspector]public float ratio = 0;
	float x = 0;
	[SerializeField]bool bY = false;

	void Start ()
	{
		x = Mathf.Abs (max) > Mathf.Abs (min) ? max : min;
	}

	void Update ()
	{
		Vector3 temp = rectContainer.localPosition;
		if (!bY) {
			temp.x = Mathf.Clamp (temp.x, min, max);
			ratio = temp.x / x;
		} else {
			temp.y = Mathf.Clamp (temp.y, min, max);
			ratio = temp.y / x;
		}
		rectContainer.localPosition = temp;
	}
}
