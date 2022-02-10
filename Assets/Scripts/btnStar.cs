using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class btnStar : MonoBehaviour
{
	[SerializeField]int indexStar = 0;
	[SerializeField]Transform rectActive = null;
	[SerializeField]UIButton btn = null;
	Action<int> rateNow = null;
	public void Setup (int index, Action<int> rate)
	{
		btn.Register (Click);
		rateNow = rate;
		indexStar = index;
		Disable ();
	}

	public void Click ()
	{
		rateNow.Invoke (indexStar);
	}

	public void Active ()
	{
		rectActive.gameObject.SetActive (true);
	}

	public void Disable ()
	{
		rectActive.gameObject.SetActive (false);
	}
}
