using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerCamera : MonoBehaviour
{
	bool bActive = false;
	Action cb = null;

	public void Register (Action a)
	{
		cb = a;	
	}

	public void Active (bool b)
	{
		if (b) {
			bActive = false;
			gameObject.SetActive (true);
		} else {
			bActive = true;
			gameObject.SetActive (false);
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (bActive)
			return;
		if (coll.CompareTag ("OutRange")) {
			bActive = true;
			cb.Invoke ();
			gameObject.SetActive (false);
		}
	}
}
