using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ButtonBrake : ButtonTdz
{
	[SerializeField]Transform tBlock = null;
	bool bClick = false;

	public override void Active (bool b = false)
	{
		
	}

	public override void Click ()
	{
		PlayerControl.Instance.Breake (true);
		if (!bClick) {
			tBlock.gameObject.SetActive (true);
			bClick = true;
		}
	}

	public override void UnClick ()
	{
		PlayerControl.Instance.Breake (false);

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
			PlayerControl.Instance.Breake (false);
		}
	}

	public override void RegisterClick (Action a, float timer = 0)
	{
		throw new NotImplementedException ();
	}

	public override void Block (bool b = false)
	{
		throw new NotImplementedException ();
	}
}
