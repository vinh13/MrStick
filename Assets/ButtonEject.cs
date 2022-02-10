using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ButtonEject :ButtonTdz
{
	Action _Eject = null;
	[SerializeField]Image imgClick = null;
	[SerializeField]Transform tBlock = null;

	public override void Active (bool b = false)
	{
		throw new NotImplementedException ();
	}

	public override void Click ()
	{
		_Eject.Invoke ();
	}

	public override void RegisterClick (Action a, float timer = 0)
	{
		_Eject = a;
	}

	public override void Block (bool b = false)
	{
		imgClick.enabled = !b;
		tBlock.gameObject.SetActive (b);
	}

	public override void UnClick ()
	{
		
	}
}
