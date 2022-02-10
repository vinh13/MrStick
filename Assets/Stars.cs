using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddStarS
{
	public int star = 0;
	public bool bLate = false;
}

public class Stars : MonoBehaviour
{
	[SerializeField]Transform[] tStar = new Transform[3];
	[SerializeField]Transform[] tLock = new Transform[3];
	Action<object> callback = null;

	public void SetStar (int numberStar, bool[] b, Action<object> a, bool bS)
	{
		callback = a;
		bool bac = false;
		for (int i = 0; i < 3; i++) {
			if (i < numberStar) {
				bac = true;
			} else {
				bac = false;
			}
			if (numberStar != 0)
				StartCoroutine (AddStar (0.5F * i, i, bac, b [i], bS));
		}
		if (numberStar == 0) {
			AddStarS star = new AddStarS ();
			star.star = 0;
			star.bLate = true;
			a.Invoke (star);
		}
	}

	IEnumerator AddStar (float timer, int i, bool active, bool b, bool bS)
	{
		yield return new WaitForSecondsRealtime (timer);
		tStar [i].gameObject.SetActive (active);
		tLock [i].gameObject.SetActive (!b);
		AddStarS star = new AddStarS ();
		if (active) {
			star.star = 1;
			if (!bS)
				SFXManager.Instance.Play ("star");
		} else {
			star.star = 0;
		}
		if (i >= 2) {
			star.bLate = true;
		} else {
			star.bLate = false;
		}
		callback.Invoke (star);
	}
}
