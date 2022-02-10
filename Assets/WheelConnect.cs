using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelConnect : MonoBehaviour
{
	[SerializeField]HingeJoint2D hj2d = null;
	[SerializeField]Rigidbody2D rg2d = null;
	Vector3 localPos;
	Quaternion localRo;

	void Start ()
	{
		localPos = transform.localPosition;
		localRo = transform.localRotation;
	}

	public void On ()
	{
		hj2d.useLimits = true;
	}

	public void Off ()
	{
		hj2d.useLimits = false;
	}

	public void Reset ()
	{
		
		transform.localPosition = localPos;
		transform.localRotation = localRo;
	}
	
}
