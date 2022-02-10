using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class zButtonAttack : ButtonTdz
{
	Action _Attack = null;
	[SerializeField]Image imgClick = null, imgFill = null;
	[SerializeField]Transform tBlock = null;
	float duration = 0;
	float _timer = 0;
	bool bBlock = false;

	public override void Click ()
	{
		if (bBlock)
			return;
		bBlock = true;
		_Attack.Invoke ();
		_timer = duration;
		StartCoroutine (_Click ());
	}

	public override void Active (bool b = false)
	{
		gameObject.SetActive (b);
		if (!b) {
			StopAllCoroutines ();
			imgFill.fillAmount = 0;
			Block (false);
			bBlock = false;
		}
	}

	public override void UnClick ()
	{
		
	}

	public override void RegisterClick (Action a, float timer = 0)
	{
		_Attack = a;
		duration = timer;
	}

	public override void Block (bool b = false)
	{
		imgClick.raycastTarget = !b;
		bBlock = b;
		tBlock.gameObject.SetActive (b);
	}

	IEnumerator _Click ()
	{
		bool done = false;
		while (!done) {
			_timer -= Time.deltaTime;
			_timer = Mathf.Clamp (_timer, 0, duration);
			float ratio = _timer / duration;
			imgFill.fillAmount = ratio;
			if (_timer <= 0)
				done = true;
			yield return null;
		}
		bBlock = false;
	}
}
