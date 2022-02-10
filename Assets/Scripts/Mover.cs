using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mover : MonoBehaviour
{
	Vector3 localTarget;
	float fSpeed = 0;
	public void _Move (Vector3 _localTarget, float speed, Action _cb)
	{
		StopAllCoroutines ();
		fSpeed = speed;
		localTarget = _localTarget;
		if (gameObject.activeSelf)
			StartCoroutine (Move (_cb));
	}

	IEnumerator Move (Action callback)
	{
		bool done = false;
		while (!done) {
			Vector3 temp = transform.localPosition;
			temp = Vector3.MoveTowards (temp, localTarget, fSpeed * Time.unscaledDeltaTime);
			transform.localPosition = temp;
			if (temp == localTarget)
				done = true;
			yield return null;
		}
		if (callback != null) {
			callback.Invoke ();
			callback = null;
		}
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}
}
