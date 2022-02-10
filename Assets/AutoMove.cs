using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
	float range = 0;
	[SerializeField]float originalRange = 0;
	[SerializeField]Transform bg = null;

	public void TakeLimit (float xW)
	{
		Vector3 pos = transform.position;
		pos.x = -xW;
		transform.position = pos;
		range = originalRange - xW;
		range *= 2;
	}

	public void SetRatio (float ratio)
	{
		Vector2 localPos = bg.localPosition;
		localPos.x = -ratio * range;
		bg.localPosition = localPos;
	}
}
