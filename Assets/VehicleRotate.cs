using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRotate : MonoBehaviour
{
	[SerializeField]Rigidbody2D rg2d = null;
	Transform target = null;
	[SerializeField]bool bRotate = false;
	[SerializeField]VehicleStatus status = null;
	bool bFreeze = false;
	bool bCheck = false;
	bool bMoveCircle = false;
	bool bMove360 = false;
	float angleZ = 0;
	void Start ()
	{
		bCheck = true;
		bFreeze = true;
		rg2d.freezeRotation = true;
	}
	void Update ()
	{
		if (!bCheck)
			return;
		if (bRotate) {
			UnFreeze ();
			if (bMoveCircle)
				MoveCircle ();
			if (bMove360)
				Move360D ();
		} else {
			Freeze ();
		}
	}

	void Move360D ()
	{
		if (angleZ < 360) {
			angleZ += 20F;
		} else {
			bMove360 = false;
			bRotate = false;
			angleZ = 360F;
		}
		rg2d.MoveRotation (angleZ);

	}

	void MoveCircle ()
	{
		Vector2 dir = target.position - transform.position;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		angle -= 90;
		rg2d.MoveRotation (angle);
	}

	void UnFreeze ()
	{
		if (!bFreeze)
			return;
		status.bRotateIdle = true;
		bFreeze = false;
		rg2d.freezeRotation = false;
		StopAllCoroutines ();
	}

	void Freeze ()
	{
		if (bFreeze)
			return;
		bFreeze = true;
		float r = rg2d.rotation;
		StartCoroutine (ToFreeze (r < 0 ? 1 : -1));
	}

	IEnumerator ToFreeze (int dir)
	{
		bool done = false;
		while (!done) {
			float r = rg2d.rotation;
			r += dir * 2F * Time.deltaTime;
			r = Mathf.FloorToInt (r);
			if (dir > 0)
				r = Mathf.Clamp (r, -Mathf.Infinity, 0);
			else
				r = Mathf.Clamp (r, 0, Mathf.Infinity);
			rg2d.rotation = r;
			done = r == 0;
			yield return null;
		}
		rg2d.freezeRotation = true;
		status.bRotateIdle = false;
	}
}
