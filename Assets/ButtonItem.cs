using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonItem : ButtonTdz
{
	[SerializeField]Image imgCD = null, imgClick = null, imgPreview = null;
	[SerializeField]Text textCount = null;
	[SerializeField]RectTransform rectBlock = null;
	BootType bootType = BootType.None;
	int count = 0;
	Action<BootType> callBoot = null;
	float duration = 0;
	float timer = 0;

	void UpdateText ()
	{
		textCount.text = "" + count;
		CharacterData.SetBoot (bootType, count);
	}

	public void Setup (BootType t, Action<BootType>  a, float d)
	{
		duration = d;
		callBoot = a;
		bootType = t;
		count = CharacterData.GetBoot (t);
		imgCD.fillAmount = 0;
		UpdateText ();
		Recheck ();
	}

	public override void Click ()
	{
		if (count == 0)
			return;
		count--;
		UpdateText ();
		if (count > 0) {
			Countdown ();
		} else {
			
		}
		Active (false);
		Block (true);
		callBoot.Invoke (bootType);
	}

	void Recheck ()
	{
		if (count > 0) {
			Active (true);
			Block (false);
		} else {
			Active (false);
			Block (true);
		}
	}

	void Countdown ()
	{
		timer = duration;
		imgCD.fillAmount = 1;
		stCd ();
	}

	void stCd ()
	{
		StartCoroutine (_Countdown ());
	}

	IEnumerator _Countdown ()
	{
		yield return new WaitForSeconds (0.2F);
		timer -= 0.2F;
		if (timer > 0) {
			stCd ();
		} else {
			Recheck ();
		}
		float ratio = timer / duration;
		imgCD.fillAmount = ratio;
	}

	public override void UnClick ()
	{
	}

	public override void RegisterClick (System.Action a, float timer = 0)
	{
	}

	public override void Block (bool b = false)
	{
		imgPreview.gameObject.SetActive (!b);
		rectBlock.gameObject.SetActive (b);
	}

	public override void Active (bool b = false)
	{
		imgClick.enabled = b;
	}
}
