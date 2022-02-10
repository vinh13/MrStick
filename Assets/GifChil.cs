using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class GifLevel
{
	public int ID = 0;
	public bool bRewarded = false;
}

public class GifChil : MonoBehaviour
{
	[SerializeField]Transform[] rects;
	[SerializeField]UIButton btn = null;
	Action<object> callback = null;
	public int ID = 0;
	string key = "";
	bool rewarded = false;

	public void Register (Action<object> a, int id, float ratio, string k, float range)
	{
		key = k;
		ID = id;
		callback = a;
		rewarded = GameData.GetLevelGif (key);
		btn.Block (rewarded);
		Vector3 localP = transform.localPosition;
		localP.x = ratio * range;
		transform.localPosition = localP;
	}

	void Start ()
	{
		btn.Register (Click);
	}

	public void Active (bool b)
	{
		rects [0].gameObject.SetActive (b);
		rects [1].gameObject.SetActive (!b);
	}

	public void Click ()
	{
		GifLevel gifLevel = new GifLevel ();
		if (!rewarded) {
			gifLevel.ID = ID;
			gifLevel.bRewarded = false; 
			callback.Invoke (gifLevel);
		} else {
			gifLevel.ID = ID;
			gifLevel.bRewarded = true; 
			callback.Invoke (gifLevel);
		}
	}
}
