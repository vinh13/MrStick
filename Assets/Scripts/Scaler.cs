using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scaler : MonoBehaviour
{
	public void Scale (float value, float speed)
	{
		StopAllCoroutines ();
		StartCoroutine (_Scale (value, speed, null));
	}

	public void Scale (float value, float speed, Action a)
	{
		StopAllCoroutines ();
		StartCoroutine (_Scale (value, speed, a));
	}

	IEnumerator _Scale (float fTarget, float fSpeed, Action _done)
	{
		float curSx = transform.localScale.x;
		int direction = curSx > fTarget ? -1 : 1;
		bool done = false;
		while (!done) {
			curSx += fSpeed * direction * Time.unscaledDeltaTime;
			if (direction > 0) {
				curSx = Mathf.Clamp (curSx, 0, fTarget);
			} else {
				curSx = Mathf.Clamp (curSx, fTarget, 100);
			}
			if (curSx == fTarget)
				done = true;


			transform.localScale = new Vector3 (curSx, curSx, curSx);
			yield return null;
		}
		if (_done != null) {
			_done.Invoke ();
		}
	}
}
