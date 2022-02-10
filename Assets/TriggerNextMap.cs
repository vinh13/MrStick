using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerNextMap : MonoBehaviour
{
	bool bBlock = false;
	Action _OnResetRace = null;

	void Awake ()
	{
		gameObject.layer = 0;
	}

	public void RegisterNextRace (Action a)
	{
		_OnResetRace = a;
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (bBlock)
			return;
		if (coll.CompareTag ("TriggerEnd")) {
			bBlock = true;
			_OnResetRace.Invoke ();
			TaskUtil.Schedule (this, Reset, 2F);
		}
	}

	void Reset ()
	{
		bBlock = false;
	}
}
