using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObFollowContainer : MonoBehaviour
{
	List<Action> _BreakOb = new List<Action> ();
	public Color colorHit = Color.white;
	public bool bEnd = false;
	public void BreakObject ()
	{
		for (int i = 0; i < _BreakOb.Count; i++) {
			_BreakOb [i].Invoke ();
		}
	}

	public void AddObject (Action a)
	{
		_BreakOb.Add (a);
	}
}
