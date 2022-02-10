using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonAttack : ButtonTdz
{
	Action _Attack = null;
	[SerializeField]Image imgClick = null;
	[SerializeField]Transform tBlock = null;
	bool bSelected = false;
	float duration = 0;
	float _timer = 0;
	bool bBlock = false;

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
	}

	public override void RegisterClick (Action a, float timer = 0)
	{
		_Attack = a;
		duration = timer;
	}

	public override void Click ()
	{
		if (bBlock)
			return;
		bSelected = true;
		_Attack.Invoke ();
	}

	public override void Block (bool b = false)
	{
		bBlock = b;
		imgClick.raycastTarget = !b;
		tBlock.gameObject.SetActive (b);
	}

	public override void UnClick ()
	{
		bSelected = false;
		_timer = 0;
	}

	void Update ()
	{
		if (!bSelected)
			return;
		_timer += Time.deltaTime;
		if (_timer >= duration) {
			_timer = 0;
			if (bBlock)
				return;
			_Attack.Invoke ();
		}
	}

	void OnDisable ()
	{
		bSelected = false;
		_timer = 0;
	}

}
