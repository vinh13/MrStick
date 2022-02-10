using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JumpLeft : ButtonTdz
{
	[SerializeField]bool isLeft = false;
	[SerializeField]Transform tBlock = null;
	bool bClick = false;

	public override void Active (bool b = false)
	{
		throw new NotImplementedException ();
	}

	public override void Click ()
	{
		if (!bClick) {
			tBlock.gameObject.SetActive (true);
			bClick = true;
		}
	}

	public override void UnClick ()
	{
		if (bClick) {
			tBlock.gameObject.SetActive (false);
			bClick = false;
		}
	}

	void OnDisable ()
	{
		if (bClick) {
			tBlock.gameObject.SetActive (false);
			bClick = false;
		}
	}

	public override void RegisterClick (System.Action a, float timer = 0)
	{
		throw new NotImplementedException ();
	}

	public override void Block (bool b = false)
	{
		throw new NotImplementedException ();
	}

	void Update ()
	{
		if (!bClick)
			return;
		PlayerControl.Instance.Jumpmove (isLeft);
	}
}
