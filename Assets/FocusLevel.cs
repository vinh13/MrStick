using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLevel : MonoBehaviour
{
	[SerializeField]Vector3 originalPos = Vector3.zero;
	Vector3 localEnd = Vector3.zero;
	[SerializeField]float rangeY = 10F;
	[SerializeField]float fSpeed = 10F;
	[SerializeField]Mover mover = null;
	bool bUp = false;

	void OnValidate ()
	{
		originalPos = mover.transform.localPosition;
		localEnd = originalPos;
		localEnd.y -= rangeY;
		bUp = true;
	}

	void OnEnable ()
	{
		bUp = true;
		Play ();
	}

	void OnDisable ()
	{
		mover.transform.localPosition = originalPos;
		StopAllCoroutines ();
	}

	void Play ()
	{
		bUp = !bUp;
		if (!bUp) {
			mover._Move (localEnd, fSpeed, Done);
		} else {
			mover._Move (originalPos, fSpeed, Done);
		}
	}

	void Done ()
	{
		TaskUtil.Schedule (this, Play, 0.1F);
	}
}
