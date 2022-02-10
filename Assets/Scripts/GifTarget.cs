using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GifTarget : MonoBehaviour
{
	[SerializeField]UIButton btn = null;
	Action<object> _showGif = null;
	bool rewarded = false;
	[SerializeField]Transform[] rects = new Transform[2];
	[SerializeField]Transform[] stars = new Transform[2];
	int ID = 0;
	bool bUnloked = false;
	[SerializeField]Text textStar = null;

	void Start ()
	{
		btn.Register (Click);
	}

	public void Setup (Action<object> a, bool b, int i, bool _bUnloked, int star)
	{
		bUnloked = _bUnloked;
		_showGif = a;
		rewarded = b;
		ID = i;
		textStar.text = "" + star;
		Sync ();
	}

	public void Setup (bool b, int i, bool _bUnloked)
	{
		bUnloked = _bUnloked;
		rewarded = b;
		ID = i;
		Sync ();
	}

	void Sync ()
	{
		btn.Block (rewarded);
		rects [0].gameObject.SetActive (!rewarded);
		rects [1].gameObject.SetActive (rewarded);
		stars [0].gameObject.SetActive (bUnloked);
		stars [1].gameObject.SetActive (!bUnloked);
	}

	void Click ()
	{
		_showGif.Invoke (ID);
	}
}
