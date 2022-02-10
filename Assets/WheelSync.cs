using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSync : MonoBehaviour
{
	[SerializeField]Rigidbody2D rg2d = null;
	Vector3 localPos;
	Quaternion localRo;

	void Start ()
	{
		localPos = transform.localPosition;
		localRo = transform.localRotation;
	}

	public void StopAll ()
	{
		rg2d.velocity = Vector2.zero;
		rg2d.angularVelocity = 0;
	}

	public void Reset ()
	{
		transform.localPosition = localPos;
		transform.localRotation = localRo;
	}
}
